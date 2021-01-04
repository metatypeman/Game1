using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymOntoClay.Helpers
{
    public static class QuickLogger
    {
        private static string _fileName;

        static QuickLogger()
        {
            _fileName = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), $"Game1_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_ffff")}.log");
        }

        public static void Log(string txt)
        {
            File.AppendAllText(_fileName, txt);
        }
    }
}
