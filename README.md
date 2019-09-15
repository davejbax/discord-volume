Discord Volume
==============

# Obsolete and no longer works!
See [this gist](https://gist.github.com/davejbax/42abae54865f2ba1e3c649c7949fbbe1) for an alternative

## What is (was) this?

This is a C# application that allows you to adjust the volume of specific users in Discord **above the 200% limit**.

## Why?
While generally speaking, Discord does an excellent job of adjusting the volumes of users, and microphones do an excellent job simultaneously at making users loud enough to be heard and understood, sometimes one of these two steps fail. Most often, they can be resolved by tweaking client settings (e.g. input volume in Discord and/or Windows). However, in a few rare cases, this is insufficient, and the 200% volume is simply not enough to be able to hear particular users with quiet microphones.

This application therefore increases this limit to 3000%.

## Requirements
- Windows (tested on 7; 10 should work)
- .NET framework (a setup file is available that can install this)
- Discord (desktop application)

## [Download](https://github.com/davejbax/discord-volume/releases)