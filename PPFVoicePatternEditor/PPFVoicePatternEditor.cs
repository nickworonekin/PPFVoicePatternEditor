﻿/*
 * PPF Voice Pattern Editor v3.0
 * Written by Nick Woronekin
 * <http://puyonexus.net>
 * 
 * This program allows you to change the voice patterns in
 * Puyo Puyo Fever PC and Puyo Puyo Fever 2 PS2.
 * 
 * This program is being released as open source with no
 * specific license. Feel free to do whatever you want to do
 * with it, as long as you do not claim that it is your program.
 * If you're distrubuting this program, or any modifications of it,
 * I would like it if you would mention my name, but I do not
 * require it. Anyway, enjoy this program!
 * 
 */

using System;
using System.Windows.Forms;

namespace PPFVoicePatternEditor
{
    public class PPFVoicePatternEditor
    {
        public const string ProgramName      = "PPF Voice Pattern Editor";
        public const string ProgramVersion   = "3.0";
        public const string ProgramCopyright = "© 2008-2012 Nick Woronekin";

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainWindow());
        }
    }
}