using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLogHelpers
{
    public class NLogConfiguratOptions
    {
        public bool UseFile { get; set; }
        public bool UseConsole { get; set; }
        public NLogConfiguratOptionsTargetDirectory TargetDirectory { get; set; } = NLogConfiguratOptionsTargetDirectory.None;
        public NLogConfiguratOptionsSeparateFiles SeparateFiles = NLogConfiguratOptionsSeparateFiles.ByDate;
        public string TargetPath { get; set; } = string.Empty;
    }
}
