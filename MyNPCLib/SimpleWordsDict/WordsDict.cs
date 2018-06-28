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
            mNamesList = data.NamesList;
        }

        private IDictionary<string, WordFrame> mWordsDict;
        private IList<string> mNamesList;

        public WordFrame GetWordFrame(string word)
        {
            if(mWordsDict.ContainsKey(word))
            {
                return mWordsDict[word];
            }

            return null;
        }

        public bool IsName(string word)
        {
            if(mNamesList.Contains(word))
            {
                return true;
            }

            return false;
        }
    }
}
