﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    public static class TmpFactoryOfWordsDictData
    {
        static TmpFactoryOfWordsDictData()
        {
            mWordsDictData = new WordsDictData();
            mWordsDictData.WordsDict = new Dictionary<string, WordFrame>();

            DefineWords();
        }

        private static WordsDictData mWordsDictData;
        public static WordsDictData Data => mWordsDictData;

        private static void DefineWords()
        {
            var wordName = "be";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new VerbGrammaticalWordFrame()
                    {
                        IsFormOfToBe = true
                    }
                }
            };

            wordName = "will";
        }
    }
}
