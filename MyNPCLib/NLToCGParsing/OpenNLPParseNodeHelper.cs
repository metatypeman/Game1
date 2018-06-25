using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public static class OpenNLPParseNodeHelper
    {
        public static string ToString(Parse node)
        {
            return ToString(0u, node);
        }

        public static string ToString(uint n, Parse node)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin node.Type = {node.Type} node.Value = {node.Value} node.Label = {node.Label} node.IsPosTag = {node.IsPosTag} node.IsLeaf = {node.IsLeaf} node.IsComplete = {node.IsComplete} Probability = {node.Probability}");
            var children = node.GetChildren();
            foreach (var child in children)
            {
                sb.Append(ToString(nextN, child));
            }
            sb.AppendLine($"{spaces}End node.Type = {node.Type} node.Value = {node.Value}");
            return sb.ToString();
        }
    }
}
