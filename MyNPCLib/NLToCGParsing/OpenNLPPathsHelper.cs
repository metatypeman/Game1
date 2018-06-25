using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public static class OpenNLPPathsHelper
    {
        static OpenNLPPathsHelper()
        {
            mModelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Models/");
            mEnglishSDnbinPath = Path.Combine(mModelsPath, "EnglishSD.nbin");
        }

        private static string mModelsPath;
        private static string mEnglishSDnbinPath;

        public static string ModelsPath => mModelsPath;
        public static string EnglishSDnbinPath => mEnglishSDnbinPath;
    }
}
