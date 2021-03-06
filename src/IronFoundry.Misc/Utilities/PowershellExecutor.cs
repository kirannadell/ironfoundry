﻿namespace IronFoundry.Misc.Utilities
{
    using System;
    using System.IO;
    using IronFoundry.Misc.Properties;
    using Microsoft.Win32;

    [System.ComponentModel.DesignerCategory(@"Code")]
    public class PowershellExecutor : RedirectedProcess
    {
        private const string powershellArgsFmt = @"-NoProfile -NoLogo -NonInteractive -WindowStyle Hidden -ExecutionPolicy Unrestricted -File ""{0}""";
        private static readonly string powershellExe;

        static PowershellExecutor()
        {
            string powershellDir = null;
            using (RegistryKey subKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\PowerShell\1\PowerShellEngine"))
            {
                if (null != subKey)
                {
                    powershellDir = subKey.GetValue("ApplicationBase").ToString();
                }
            }
            if (false == Directory.Exists(powershellDir))
            {
                throw new Exception(Resources.PowershellExecutor_CantFindDir_Message);
            }

            powershellExe = Path.Combine(powershellDir, "powershell.exe");
            if (false == File.Exists(powershellExe))
            {
                throw new Exception(Resources.PowershellExecutor_CantFindExe_Message);
            }
        }

        public PowershellExecutor(string scriptPath)
            : base(powershellExe, String.Format(powershellArgsFmt, scriptPath)) { }
    }
}