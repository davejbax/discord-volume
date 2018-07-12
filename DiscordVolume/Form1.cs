using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DiscordVolume
{
    /*
     * WinAPI structs/enums
     */

    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }

    [Flags]
    public enum MemoryState : uint
    {
        MEM_COMMIT = 0x1000,
        MEM_FREE = 0x10000,
        MEM_RESERVE = 0x2000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInfo
    {
        public ushort processorArchitecture;
        ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;
        public IntPtr maximumApplicationAddress;
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;
    }

    /*
     * Program structs/enums
     */

    public enum State
    {
        INITIAL_SCAN,
        NEXT_SCAN_ZERO,
        NEXT_SCAN_200
    }

    /*
     * Class definition(s)
     */

    public partial class Form1 : Form
    {
        /*
         * WinAPI functions
         */

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SystemInfo lpSystemInfo);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        /*
         * Program variables
         */

        private State currentState = State.INITIAL_SCAN;
        private SystemInfo sysInfo;

        private List<Tuple<IntPtr, IntPtr>> memoryLocations = new List<Tuple<IntPtr, IntPtr>>();
        private Tuple<IntPtr, IntPtr> boundMemoryLocation = null;

        public Form1()
        {
            InitializeComponent();

            // Get system info so we can determine min & max process address space
            this.sysInfo = new SystemInfo();
            GetSystemInfo(out sysInfo);
        }

        private void btnIdentify_Click(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case State.INITIAL_SCAN:
                    // Carry out initial scan to find magic (200%) in memory of any Discord process
                    ScanInitial();
                    
                    // If no matches, couldn't find it -- either due to access rights or due to user's fault
                    if (memoryLocations.Count == 0)
                    {
                        lblInstruction.Text = "Could not find any matches! Are you sure user volume is 200%?";
                    }
                    // If one match, we assume it's the right one
                    else if (memoryLocations.Count == 1)
                    {
                        lblInstruction.Text = "Found user! Volume control now bound.";
                        BindVolumeControl(memoryLocations[0]);
                        
                    }
                    // If multiple matches, move on to next scan state (to narrow it down)
                    else if (memoryLocations.Count > 1)
                    {
                        lblInstruction.Text = "Found multiple; set volume of user to 0%";
                        currentState = State.NEXT_SCAN_ZERO;
                    }
                    break;

                case State.NEXT_SCAN_ZERO:
                case State.NEXT_SCAN_200:
                    // Carry out next scan to find whatever value we're looking for
                    ScanForValue(memoryLocations, currentState == State.NEXT_SCAN_200);

                    // If no matches, we couldn't find it.
                    if (memoryLocations.Count == 0)
                    {
                        MessageBox.Show("Error: could not find any matches! Set volume to 200% and press 'identify' again to restart.");
                        ResetControls();
                    }
                    // If one location, hey: we found it!
                    else if (memoryLocations.Count == 1)
                    {
                        lblInstruction.Text = "Found user! Volume control now bound.";
                        BindVolumeControl(memoryLocations[0]);
                    }
                    // If multiple locations, alternate between 0% and 200% to narrow down
                    else if (memoryLocations.Count > 1)
                    {
                        lblInstruction.Text = "Found multiple; set volume of user to "
                            + (currentState == State.NEXT_SCAN_200 ? "0%" : "200%");
                        currentState = currentState == State.NEXT_SCAN_200 ? State.NEXT_SCAN_ZERO : State.NEXT_SCAN_200;
                    }
                    break;
            }
        }

        private void BindVolumeControl(Tuple<IntPtr, IntPtr> memoryLocation)
        {
            boundMemoryLocation = memoryLocation;
            trackBarVolume.Enabled = true;
            btnIdentify.Enabled = false;
            lblBound.Text = "Bound to user!";
            lblBound.ForeColor = Color.Green;
        }

        private void ResetControls()
        {
            // Reset all state-related variables and reset UI
            lblInstruction.Text = "Set user volume to 200%";
            currentState = State.INITIAL_SCAN;
            memoryLocations.Clear();
            boundMemoryLocation = null;
            trackBarVolume.Enabled = false; // Disable trackbar when no user bound
            trackBarVolume.Value = 0;
            btnIdentify.Enabled = true;
            lblBound.Text = "Not bound to user";
            lblBound.ForeColor = Color.Red;
        }

        private void ScanInitial()
        {
            List<IntPtr> processes = new List<IntPtr>();

            // Clear existing list of memory locations
            memoryLocations.Clear();

            // Find all processes named 'Discord' (there are always multiple)
            foreach (Process proc in Process.GetProcessesByName("Discord").ToList())
            {
                IntPtr handle = OpenProcess(ProcessAccessFlags.All, false, proc.Id);
                if (handle == IntPtr.Zero)
                    Console.WriteLine("Failed to open a process (ID " + proc.Id + ")");
                else
                    processes.Add(handle);
            }

            Console.WriteLine("Scanning " + processes.Count + " processes");

            // Scan each process' memory for the magic (200%) value;
            // if not found, close handle. If found, add to memoryLocations
            foreach (IntPtr procHandle in processes)
            {
                if (!ScanProcess(procHandle, memoryLocations))
                    CloseHandle(procHandle);
            }

            Console.WriteLine("Found " + memoryLocations.Count + " matches!");
        }

        private bool ScanProcess(IntPtr procHandle, List<Tuple<IntPtr, IntPtr>> addressList)
        {
            // N.B. - This app & Discord must be 32-bit!
            long startAddr = (long) sysInfo.minimumApplicationAddress;
            long maxAddr = (long)sysInfo.maximumApplicationAddress;

            // Start at the minimum address
            long addr = startAddr;
            MemoryBasicInformation mbi;

            bool found = false;

            do
            {
                // Query for memory information about that region of memory
                int result = VirtualQueryEx(procHandle, (IntPtr)addr, out mbi, (uint)Marshal.SizeOf(typeof(MemoryBasicInformation)));

                // If an error occurred, stop.
                if (result == 0)
                {
                    Console.WriteLine("Failed to query process memory space");
                    break;
                }

                // Only scan if it's a committed (i.e. not free, not reserved) region, and is MEM_PRIVATE (= 0x20000)
                if (mbi.State == (uint) MemoryState.MEM_COMMIT && mbi.Type == 0x20000)
                {
                    if (ScanMemoryRegion(procHandle, (long)mbi.BaseAddress, (long)mbi.RegionSize, addressList))
                        found = true;
                }

                // Move to the next region of memory
                addr = (long)mbi.BaseAddress + (long)mbi.RegionSize;
            }
            while (addr < maxAddr);

            return found;
        }

        private bool ScanMemoryRegion(IntPtr procHandle, long startAddr, long size, List<Tuple<IntPtr, IntPtr>> addressList)
        {
            byte[] buf = new byte[size];
            int read = 0;
            bool found = false;

            // Read memory into buffer
            if (!ReadProcessMemory(procHandle, (IntPtr)startAddr, buf, (int)size, ref read))
                return false; // If we can't read it, fail silently

            for (int i = 0; i <= read - 4; i += 4)
            {
                // Check for our 'magic' constant (= value of volume @ 200%)
                if (buf[i] == (byte)0xAE
                    && buf[i + 1] == (byte)0x47
                    && buf[i + 2] == (byte)0x01
                    && buf[i + 3] == (byte)0x40)
                {
                    addressList.Add(new Tuple<IntPtr, IntPtr>(procHandle, (IntPtr)(startAddr + i)));
                    found = true;
                }
            }

            return found;
        }

        private void ScanForValue(List<Tuple<IntPtr, IntPtr>> memoryLocations, bool magic)
        {
            byte[] buf = new byte[4];

            Console.WriteLine("Searching for " + (magic ? "magic" : "zero") + "value in memory...");
            int initialCount = memoryLocations.Count;

            // Iterate backwards so we can remove items
            for (int i = memoryLocations.Count - 1; i >= 0; i--)
            {
                int read = 0;
                if (!ReadProcessMemory(memoryLocations[i].Item1, memoryLocations[i].Item2, buf, 4, ref read))
                {
                    Console.WriteLine("Failed to read process memory! Discarding memory location.");
                    memoryLocations.RemoveAt(i);
                    continue;
                }

                if (magic)
                {
                    // Check for magic bytes; if not there, remove from potential locations.
                    if (buf[0] == (byte)0xAE
                        && buf[1] == (byte)0x47
                        && buf[2] == (byte)0x01
                        && buf[3] == (byte)0x40)
                        continue;
                    else
                        memoryLocations.RemoveAt(i);

                    Console.WriteLine("Discarding location " + (i+1) + " of " + initialCount + ": no magic found");
                }
                else
                {
                    // Check for zero bytes; if not there, remove from potential locations.
                    if (buf[0] == (byte)0x00
                        && buf[1] == (byte)0x00
                        && buf[2] == (byte)0x00
                        && buf[3] == (byte)0x00)
                        continue;
                    else
                        memoryLocations.RemoveAt(i);

                    Console.WriteLine("Discarding location " + (i + 1) + " of " + initialCount + ": no zero value found");
                }
            }
        }

        private void SetVolume(float volume, Tuple<IntPtr, IntPtr> memoryLocation)
        {
            // Convert float to bytes and write to memory location
            byte[] buf = BitConverter.GetBytes(volume);
            WriteProcessMemory(memoryLocation.Item1, memoryLocation.Item2, buf, 4, out int written);
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            lblVolume.Text = (trackBarVolume.Value) + "%";
            SetVolume(trackBarVolume.Value / 100f, boundMemoryLocation);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }
    }
}
