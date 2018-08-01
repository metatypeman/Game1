using System;
using System.Collections.Generic;
using System.Linq;
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

            DefineCommonClasses();
            DefineWords();

            CalculateFullMeaningsDict();
            PrepareRootWords();
        }

        private static void DefineMeaning(string word, string meaning)
        {
            DefineMeanings(word, new List<string>() { meaning });
        }

        private static void DefineMeanings(string word, List<string> listOfMeanings)
        {
            if(mTmpMeaningsDict.ContainsKey(word))
            {
                var tmplistOfMeanings = mTmpMeaningsDict[word];
                tmplistOfMeanings.AddRange(listOfMeanings);
                tmplistOfMeanings = tmplistOfMeanings.Distinct().ToList();
                mTmpMeaningsDict[word] = tmplistOfMeanings;
            }
            else
            {
                mTmpMeaningsDict[word] = listOfMeanings;
            }
        }

        private static void CalculateFullMeaningsDict()
        {
#if DEBUG
            //LogInstance.Log($"mTmpMeaningsDict.Count = {mTmpMeaningsDict.Count}");
#endif

            var mMeaningsDict = new Dictionary<string, IList<string>>();

            foreach (var tmpMeaningsDictKVPItem in mTmpMeaningsDict)
            {
                var word = tmpMeaningsDictKVPItem.Key;
#if DEBUG
                //LogInstance.Log($"word = {word}");
#endif
                
                var wasVisited = new List<string>();
                wasVisited.Add(word);
                var tmplistOfMeanings = tmpMeaningsDictKVPItem.Value;

                NCalculateFullMeaningsDict(word, ref tmplistOfMeanings, wasVisited);

                tmplistOfMeanings = tmplistOfMeanings.Distinct().ToList();
#if DEBUG
                //LogInstance.Log($"tmplistOfMeanings.Count = {tmplistOfMeanings.Count}");
                //foreach(var meaning in tmplistOfMeanings)
                //{
                //    LogInstance.Log($"meaning = {meaning}");
                //}
#endif
                mMeaningsDict[word] = tmplistOfMeanings;
            }

            var wordsDict = mWordsDictData.WordsDict;

            foreach(var wordsDictKVPItem in wordsDict)
            {
                var wordFrame = wordsDictKVPItem.Value;

                foreach(var grammaticalWordFrame in wordFrame.GrammaticalWordFrames)
                {
#if DEBUG
                    //LogInstance.Log($"grammaticalWordFrame = {grammaticalWordFrame}");
#endif

                    var logicalMeaningsList = grammaticalWordFrame.LogicalMeaning;

                    if (logicalMeaningsList.IsEmpty())
                    {
                        continue;
                    }

                    var completeLogicalMeaningsList = new List<string>();

                    foreach (var logicalMeaning in logicalMeaningsList)
                    {
#if DEBUG
                        //LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                        completeLogicalMeaningsList.Add(logicalMeaning);

                        if (mMeaningsDict.ContainsKey(logicalMeaning))
                        {
                            var targetLogicalMeaningsList = mMeaningsDict[logicalMeaning];
                            completeLogicalMeaningsList.AddRange(targetLogicalMeaningsList);
                        }
                    }

                    completeLogicalMeaningsList = completeLogicalMeaningsList.Distinct().ToList();
#if DEBUG
                    //LogInstance.Log($"completeLogicalMeaningsList.Count = {completeLogicalMeaningsList.Count}");
                    //foreach (var meaning in completeLogicalMeaningsList)
                    //{
                    //    LogInstance.Log($"meaning = {meaning}");
                    //}
#endif

                    grammaticalWordFrame.FullLogicalMeaning = completeLogicalMeaningsList;
                }
            }
        }

        private static void NCalculateFullMeaningsDict(string word, ref List<string> listOfMeanings, List<string> wasVisited)
        {
#if DEBUG
            //LogInstance.Log($"word = {word}");
#endif

            var tmpSourceListOfMeanings = listOfMeanings.ToList();
            foreach (var meaning in tmpSourceListOfMeanings)
            {
#if DEBUG
                //LogInstance.Log($"meaning = {meaning}");
#endif

                if(wasVisited.Contains(meaning))
                {
                    continue;
                }

#if DEBUG
                //LogInstance.Log($"NEXT meaning = {meaning}");
#endif

                if(mTmpMeaningsDict.ContainsKey(meaning))
                {
                    var tmplistOfMeanings = mTmpMeaningsDict[meaning];
                    listOfMeanings.AddRange(tmplistOfMeanings);
                    wasVisited.Add(meaning);

                    NCalculateFullMeaningsDict(meaning, ref listOfMeanings, wasVisited);
                }
            }
        }

        private static void PrepareRootWords()
        {
            foreach(var wordsDictDataKVPItem in mWordsDictData.WordsDict)
            {
                var wordFrame = wordsDictDataKVPItem.Value;
                var wordName = wordFrame.Word;

                foreach(var grammaticalWordFrame in wordFrame.GrammaticalWordFrames)
                {
                    if(string.IsNullOrWhiteSpace(grammaticalWordFrame.RootWord))
                    {
                        grammaticalWordFrame.RootWord = wordName;
                    }
                }
            }
        }

        private static WordsDictData mWordsDictData;
        public static WordsDictData Data => mWordsDictData;
        private static Dictionary<string, List<string>> mTmpMeaningsDict = new Dictionary<string, List<string>>();

        private static void DefineCommonClasses()
        {
            DefineMeaning("act", "event");
            DefineMeaning("animate", "entity");
            DefineMeaning("phisobj", "entity");
            DefineMeanings("animal", new List<string>() { "animate", "phisobj" });
            DefineMeaning("moving", "act");
            DefineMeaning("place", "phisobj");
        }

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
                        Number = GrammaticalNumberOfWord.Singular,
                        LogicalMeaning = new List<string>()
                        {
                            "animate"
                        }
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
            DefineUsualPrepositions();
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
                        IsCountable = true,
                        LogicalMeaning = new List<string>()
                        {
                            "animate"
                        }
                    }
                }
            };

            wordName = "waypoint";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new NounGrammaticalWordFrame()
                    {
                        Number = GrammaticalNumberOfWord.Singular,
                        IsCountable = true,
                        LogicalMeaning = new List<string>()
                        {
                            "place"
                        }
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
                    {
                        LogicalMeaning = new List<string>()
                        {
                            "state"
                        }
                    }
                }
            };

            wordName = "go";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new VerbGrammaticalWordFrame()
                    {
                        LogicalMeaning = new List<string>()
                        {
                            "act",
                            "moving"
                        }
                    }
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

            wordName = "green";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new AdjectiveGrammaticalWordFrame()
                    {
                        LogicalMeaning = new List<string>()
                        {
                            "color"
                        }
                    }
                }
            };
        }

        private static void DefineUsualPrepositions()
        {
            var wordName = "to";
            mWordsDictData.WordsDict[wordName] = new WordFrame()
            {
                Word = wordName,
                GrammaticalWordFrames = new List<BaseGrammaticalWordFrame>()
                {
                    new PrepositionGrammaticalWordFrame()
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
                        Gender = GrammaticalGender.Masculine,
                        LogicalMeaning = new List<string>()
                        {
                            "animate"
                        }
                    }
                }
            };
        }
    }
}
