using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ContextOfATNParsing: IObjectToString
    {
        public ContextOfATNParsing(string text, IWordsDict wordsDict)
        {
            mATNExtendedLexer = new ATNExtendedLexer(text, wordsDict);
        }

        private ContextOfATNParsing()
        {
        }

        private ATNExtendedLexer mATNExtendedLexer;
        public StateOfATNParsing State { get; set; } = StateOfATNParsing.Undefined;
        public Sentence Sentence { get; set; }
        public Queue<NounPhrase> OperativeNounPhrasesQueue = new Queue<NounPhrase>();

        public ContextOfATNParsing Fork()
        {
            var result = new ContextOfATNParsing();
            result.mATNExtendedLexer = mATNExtendedLexer.Fork();
            result.State = State;
            result.Sentence = Sentence?.Fork();
            result.OperativeNounPhrasesQueue = new Queue<NounPhrase>(OperativeNounPhrasesQueue.ToList());
            return result;
        }

        public IList<ATNExtendedToken> GetСlusterOfExtendedTokens()
        {
            return mATNExtendedLexer.GetСlusterOfExtendedTokens();
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Sentence == null)
            {
                sb.AppendLine($"{spaces}{nameof(Sentence)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Sentence)}");
                sb.Append(Sentence.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Sentence)}");
            }
            if (OperativeNounPhrasesQueue == null)
            {
                sb.AppendLine($"{spaces}{nameof(OperativeNounPhrasesQueue)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(OperativeNounPhrasesQueue)}");
                var operativeNounPhrasesList = OperativeNounPhrasesQueue.ToList();
                foreach (var operativeNounPhrase in operativeNounPhrasesList)
                {
                    sb.Append(operativeNounPhrase.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(OperativeNounPhrasesQueue)}");
            }
            return sb.ToString();
        }
    }
}
