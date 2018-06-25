using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNParser
    {
        public Sentence Run(Parse node)
        {
#if DEBUG
            LogInstance.Log($"node = {OpenNLPParseNodeHelper.ToString(node)}");
#endif
            var children = node.GetChildren();
            var sentenceNode = children.Single();

            var atnNode = new ATNSentenceNode(sentenceNode);
            var sentence = atnNode.Run(); 
            return sentence;
        }
    }
}
