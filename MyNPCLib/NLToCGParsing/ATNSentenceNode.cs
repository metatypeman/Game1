using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNSentenceNode
    {
        public ATNSentenceNode(Parse node)
        {
#if DEBUG
            LogInstance.Log($"node.Type = {node.Type} node.Value = {node.Value} node.Label = {node.Label} node.IsPosTag = {node.IsPosTag} node.IsLeaf = {node.IsLeaf} node.IsComplete = {node.IsComplete}");
#endif
        }

        public Sentence Run()
        {
#if DEBUG
            var sentence = new Sentence();//tmp
            return sentence;//tmp
#endif
        }
    }
}
