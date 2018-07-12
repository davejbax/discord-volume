namespace DiscordVolume
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.lblStatusTag = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.btnIdentify = new System.Windows.Forms.Button();
            this.lblVolumeTag = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblBound = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Enabled = false;
            this.trackBarVolume.Location = new System.Drawing.Point(12, 47);
            this.trackBarVolume.Maximum = 3000;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(409, 45);
            this.trackBarVolume.TabIndex = 0;
            this.trackBarVolume.Scroll += new System.EventHandler(this.trackBarVolume_Scroll);
            // 
            // lblStatusTag
            // 
            this.lblStatusTag.AutoSize = true;
            this.lblStatusTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusTag.Location = new System.Drawing.Point(13, 13);
            this.lblStatusTag.Name = "lblStatusTag";
            this.lblStatusTag.Size = new System.Drawing.Size(47, 13);
            this.lblStatusTag.TabIndex = 1;
            this.lblStatusTag.Text = "Status:";
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(57, 13);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(180, 13);
            this.lblInstruction.TabIndex = 2;
            this.lblInstruction.Text = "Set volume to 200% and click button";
            // 
            // btnIdentify
            // 
            this.btnIdentify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIdentify.Location = new System.Drawing.Point(16, 129);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(75, 23);
            this.btnIdentify.TabIndex = 3;
            this.btnIdentify.Text = "Identify";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // lblVolumeTag
            // 
            this.lblVolumeTag.AutoSize = true;
            this.lblVolumeTag.Location = new System.Drawing.Point(13, 98);
            this.lblVolumeTag.Name = "lblVolumeTag";
            this.lblVolumeTag.Size = new System.Drawing.Size(45, 13);
            this.lblVolumeTag.TabIndex = 4;
            this.lblVolumeTag.Text = "Volume:";
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(57, 98);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(21, 13);
            this.lblVolume.TabIndex = 5;
            this.lblVolume.Text = "0%";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(97, 129);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblBound
            // 
            this.lblBound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBound.AutoSize = true;
            this.lblBound.ForeColor = System.Drawing.Color.Red;
            this.lblBound.Location = new System.Drawing.Point(329, 134);
            this.lblBound.Name = "lblBound";
            this.lblBound.Size = new System.Drawing.Size(92, 13);
            this.lblBound.TabIndex = 9;
            this.lblBound.Text = "Not bound to user";
            this.lblBound.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 164);
            this.Controls.Add(this.lblBound);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.lblVolumeTag);
            this.Controls.Add(this.btnIdentify);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.lblStatusTag);
            this.Controls.Add(this.trackBarVolume);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Discord Volume Changer";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Label lblStatusTag;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.Label lblVolumeTag;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblBound;
    }
}

