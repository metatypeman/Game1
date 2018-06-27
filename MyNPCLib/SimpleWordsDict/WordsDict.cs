using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    public class WordsDict: IWordsDict
    {
        public WordsDict()
        {
            var data = TmpFactoryOfWordsDictData.Data;//tmp
            mWordsDict = data.WordsDict;
        }

        private IDictionary<string, WordFrame> mWordsDict;

        public WordFrame GetWordFrame(string word)
        {
            if(mWordsDict.ContainsKey(word))
            {
                return mWordsDict[word];
            }

            return null;
        }
    }
}
