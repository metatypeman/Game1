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

        public Sentence Run(string text)
        {
#if DEBUG
            LogInstance.Log($"text = {text}");
#endif

            var context = new ContextOfATNParsing(text, mWordsDict);

            var atnNode = new ATNSentenceNode(context);
            atnNode.Run();

            var sentence = new Sentence();
            return sentence;
        }
    }
}
