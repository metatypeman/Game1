using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class WordsDictData
    {
        public IDictionary<string, WordFrame> WordsDict { get; set; }
        public IList<string> NamesList { get; set; }
    }
}
