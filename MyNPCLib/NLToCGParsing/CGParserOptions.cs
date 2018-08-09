using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class CGParserOptions
    {
        public IWordsDict WordsDict { get; set; }
        public string BasePath { get; set; }
    }
}
