using MyNPCLib.NLToCGParsing.PhraseTree;
using MyNPCLib.SimpleWordsDict;
using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNParser
    {
        public ATNParser(IWordsDict wordsDict)
        {
            mWordsDict = wordsDict;
        }

        private IWordsDict mWordsDict;

        public IList<Sentence> Run(string text)
        {
#if DEBUG
            //LogInstance.Log($"text = {text}");
#endif

            var commonContext = new CommonContextOfATNParsing();
            var context = new ContextOfATNParsing(text, mWordsDict, commonContext);

            var atnNode = new ATNSentenceNode(context);
            atnNode.Run();

            var sentencesList = commonContext.SentencesList;

#if DEBUG
            //LogInstance.Log($"sentencesList.Count = {sentencesList.Count}");
            //foreach(var tmpSentences in sentencesList)
            //{
            //    LogInstance.Log($"tmpSentences = {tmpSentences}");
            //}
#endif

            return sentencesList;
        }
    }
}
