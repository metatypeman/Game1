using System;
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
            mWordsDictData.NamesList = new List<string>();

            DefineWords();
        }

        private static WordsDictData mWordsDictData;
        public static WordsDictData Data => mWordsDictData;

        private static void DefineWords()
        {
            DefineSpecialWords();
            DefineUsualWords();
        }

        private static void DefineSpecialWords()
        {
            DefineToBeWords();
            DefineToDoWords();
            DefineToHaveWords();
            DefineModalWerbs();
            DefinePronouns();
            DefineArticles();
        }

        private static void DefineToBeWords()
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
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new VerbGrammaticalWordFrame()
                    {
                        IsFormOfToBe = true,
                        Tense = GrammaticalTenses.Future
                    }
                }
            };
        }

        private static void DefineToDoWords()
        {

        }
        private static void DefineToHaveWords()
        {

        }

        private static void DefineModalWerbs()
        {

        }

        private static void DefinePronouns()
        {
            var wordName = "i";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new PronounGrammaticalWordFrame()
                    {
                        TypeOfPronoun = TypeOfPronoun.Personal,
                        Case = CaseOfPersonalPronoun.Subject,
                        Person = GrammaticalPerson.First,
                        Number = GrammaticalNumberOfWord.Singular
                    }
                }
            };
        }

        private static void DefineArticles()
        {
            var wordName = "the";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new ArticleGrammaticalWordFrame()
                }
            };
        }

        private static void DefineUsualWords()
        {
            DefineUsualNouns();
            DefineUsualVerbs();
            DefineUsualAdjectives();
            DefineNames();
        }

        private static void DefineUsualNouns()
        {
            var wordName = "dog";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new NounGrammaticalWordFrame()
                    {
                        Number = GrammaticalNumberOfWord.Singular,
                        IsCountable = true
                    }
                }
            };
        }

        private static void DefineUsualVerbs()
        {
            var wordName = "know";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new VerbGrammaticalWordFrame()
                }
            };
        }

        private static void DefineUsualAdjectives()
        {
            var wordName = "sorry";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new AdjectiveGrammaticalWordFrame()
                }
            };
        }

        private static void DefineNames()
        {
            DefineHumanNames();
        }

        private static void DefineHumanNames()
        {
            var wordName = "Tom";
            mWordsDictData.NamesList.Add(wordName);
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new NounGrammaticalWordFrame()
                    {
                        IsName = true,
                        IsCountable = true,
                        Number = GrammaticalNumberOfWord.Singular,
                        Gender = GrammaticalGender.Masculine
                    }
                }
            };


        }
    }
}
