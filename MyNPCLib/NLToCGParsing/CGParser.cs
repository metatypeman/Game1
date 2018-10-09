using MyNPCLib.CG;
using MyNPCLib.SimpleWordsDict;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.SentenceDetect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class CGParser
    {
        public CGParser(CGParserOptions options)
        {
            var modelPath = OpenNLPPathsHelper.EnglishSDnbinPath(options.BasePath);
#if DEBUG
            //LogInstance.Log($"modelPath = {modelPath}");
#endif

            mSentenceDetector = new EnglishMaximumEntropySentenceDetector(modelPath);

            mATNParser = new ATNParser(options.WordsDict);
            mSemanticAnalyzer = new SemanticAnalyzer();
        }

        private EnglishMaximumEntropySentenceDetector mSentenceDetector;
        private ATNParser mATNParser;
        private SemanticAnalyzer mSemanticAnalyzer;
        private readonly object mRunLockObj = new object();

        public GCParsingResult Run(string text)
        {
            lock(mRunLockObj)
            {
#if DEBUG
                //LogInstance.Log($"text = {text}");
#endif

                var result = new GCParsingResult();

                if (string.IsNullOrWhiteSpace(text))
                {
                    return result;
                }

                var itemsList = new List<ConceptualGraph>();
                result.Items = itemsList;

                var sentencesList = mSentenceDetector.SentenceDetect(text);

#if DEBUG
                //LogInstance.Log($"sentencesList.Length = {sentencesList.Length}");
#endif

                var isFirst = true;
                ConceptualGraph prevGraph = null;

                foreach (var sentence in sentencesList)
                {
#if DEBUG
                    //LogInstance.Log($"sentence = {sentence}");
#endif

                    var itemsResultList = RunSentence(sentence);

                    foreach(var itemResult in itemsResultList)
                    {
                        itemsList.Add(itemResult);

                        if (isFirst)
                        {
                            isFirst = false;
                            result.FistItem = itemResult;
                        }
                        else
                        {
                            itemResult.PrevGraph = prevGraph;
                        }

                        prevGraph = itemResult;
                    } 
                }

                return result;
            }
        }

        private IList<ConceptualGraph> RunSentence(string text)
        {
#if DEBUG
            //LogInstance.Log($"text = {text}");
#endif

            var result = new List<ConceptualGraph>();
            var sentencesList = mATNParser.Run(text);

#if DEBUG
            //LogInstance.Log($"sentencesList.Count = {sentencesList.Count}");
#endif
            foreach (var sentence in sentencesList)
            {
#if DEBUG
                //LogInstance.Log($"sentence = {sentence}");
#endif

                var dtSentenceNode = DTNodeConverter.Convert(sentence);

#if DEBUG
                //LogInstance.Log($"dtSentenceNode = {dtSentenceNode}");
#endif

                var itemResult = mSemanticAnalyzer.Run(dtSentenceNode);
                result.Add(itemResult);
            }

            return result;
        }
    }
}
