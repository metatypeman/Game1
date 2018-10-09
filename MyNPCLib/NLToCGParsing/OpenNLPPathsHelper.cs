using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public static class OpenNLPPathsHelper
    {
        public static string ModelsPath(string basePath)
        {
            return Path.Combine(basePath, "Resources/Models/");
        }

        public static string EnglishSDnbinPath(string basePath)
        {
            return Path.Combine(ModelsPath(basePath), "EnglishSD.nbin");
        }
    }
}
