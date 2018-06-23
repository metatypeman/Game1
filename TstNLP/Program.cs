using OpenNLP.Tools.Parser;
using System;
using System.IO;
using System.Text;

namespace TstNLP
{
    class Program
    {
        static void Main(string[] args)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Hello World!");

            var path = Directory.GetCurrentDirectory();

            NLog.LogManager.GetCurrentClassLogger().Info($"Hello World! path = {path}");

            var sentence = "- Sorry Mrs Hudson, I'll skip the tea.";

            ParseSentence(sentence);

            sentence = "Kill the dog!";

            ParseSentence(sentence);

            sentence = "This is a green forest.";

            ParseSentence(sentence);

            sentence = "The third story arc centers on the longstanding brotherhood charged with defending the realm against the ancient threats of the fierce peoples and legendary creatures that lie far north, and an impending winter that threatens the realm.";

            ParseSentence(sentence);
        }

        private static void ParseSentence(string sentence)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"ParseSentence sentence = '{sentence}'");

            var path = Directory.GetCurrentDirectory();

            var relativePath = "bin/Debug/netcoreapp2.0/Resources/Models/";

            var modelPath = Path.Combine(path, relativePath);

            NLog.LogManager.GetCurrentClassLogger().Info($"ParseSentence modelPath = {modelPath}");

            //var modelPath = "c:/Users/Сергей/Source/Repos/KillingApp/KillingApp/bin/Debug/Resources/Models/";

            var parser = new EnglishTreebankParser(modelPath);
            var node = parser.DoParse(sentence);

            DisplayNode(0u, node);
        }

        private static void DisplayNode(uint n, Parse node)
        {
            var spaces = Spaces(n);
            var nextN = n + 4;

            NLog.LogManager.GetCurrentClassLogger().Info($"{spaces}DisplayNode node.Type = {node.Type} node.Value = {node.Value}");

            var children = node.GetChildren();

            foreach (var child in children)
            {
                DisplayNode(nextN, child);
            }
        }

        public static string Spaces(uint n)
        {
            if (n == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < n; i++)
            {
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
