using MyNPCLib.CG;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.SentenceDetect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class CGParser
    {
        public CGParser()
        {
            var path = Directory.GetCurrentDirectory();
#if DEBUG
            LogInstance.Log($"path = {path}");
#endif
            
            var englishSDNbinRelativePath = "Resources/Models/EnglishSD.nbin";

            var modelPath = Path.Combine(path, englishSDNbinRelativePath);

#if DEBUG
            LogInstance.Log($"modelPath = {modelPath}");
#endif

            mSentenceDetector = new EnglishMaximumEntropySentenceDetector(modelPath);

            var relativePath = "Resources/Models/";
            modelPath = Path.Combine(path, relativePath);

#if DEBUG
            LogInstance.Log($"modelPath = {modelPath}");
#endif

            mTreebankParser = new EnglishTreebankParser(modelPath);
        }

        private EnglishMaximumEntropySentenceDetector mSentenceDetector;
        private EnglishTreebankParser mTreebankParser;
        private readonly object mRunLockObj = new object();

        public GCParsingResult Run(string text)
        {
            lock(mRunLockObj)
            {
#if DEBUG
                LogInstance.Log($"text = {text}");
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
                LogInstance.Log($"sentencesList.Length = {sentencesList.Length}");
#endif

                var isFirst = true;
                ConceptualGraph prevGraph = null;

                foreach (var sentence in sentencesList)
                {
#if DEBUG
                    LogInstance.Log($"sentence = {sentence}");
#endif

                    var itemResult = RunSentence(sentence);
                    itemsList.Add(itemResult);

                    if(isFirst)
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

                return result;
            }
        }

        private ConceptualGraph RunSentence(string text)
        {
#if DEBUG
            LogInstance.Log($"text = {text}");
#endif

            var node = mTreebankParser.DoParse(text);

#if DEBUG
            DisplayNode(0u, node);
#endif

#if DEBUG
            var result = new ConceptualGraph();//tmp
            result.Name = NamesHelper.CreateEntityName();//tmp
            return result;//tmp
#endif
        }

#if DEBUG
        private void DisplayNode(uint n, Parse node)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;

            LogInstance.Log($"{spaces}Begin node.Type = {node.Type} node.Value = {node.Value} node.Label = {node.Label} node.IsPosTag = {node.IsPosTag} = {node.IsLeaf} node.IsComplete = {node.IsComplete}");

            var children = node.GetChildren();

            foreach (var child in children)
            {
                DisplayNode(nextN, child);
            }
            LogInstance.Log($"{spaces}End node.Type = {node.Type} node.Value = {node.Value}");
        }
#endif
    }
}
