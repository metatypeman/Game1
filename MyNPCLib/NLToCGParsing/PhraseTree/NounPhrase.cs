using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing.PhraseTree
{
    public class NounPhrase : BaseNounLikePhrase
    {
        public override bool IsNounPhrase => true;
        public override NounPhrase AsNounPhrase => this;

        public ATNExtendedToken Noun { get; set; }
        public List<ATNExtendedToken> Determiners { get; set; } = new List<ATNExtendedToken>();


        public override BaseNounLikePhrase Fork()
        {
            var result = new NounPhrase();
            result.Noun = Noun;
            result.Determiners = Determiners.ToList();
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Noun == null)
            {
                sb.AppendLine($"{spaces}{nameof(Noun)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Noun)}");
                sb.Append(Noun.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Noun)}");
            }
            if (Determiners == null)
            {
                sb.AppendLine($"{spaces}{nameof(Determiners)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Determiners)}");
                foreach (var determiner in Determiners)
                {
                    sb.Append(determiner.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Determiners)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Noun == null)
            {
                sb.AppendLine($"{spaces}{nameof(Noun)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Noun)}");
                sb.Append(Noun.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Noun)}");
            }
            if (Determiners == null)
            {
                sb.AppendLine($"{spaces}{nameof(Determiners)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Determiners)}");
                foreach (var determiner in Determiners)
                {
                    sb.Append(determiner.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Determiners)}");
            }
            return sb.ToString();
        }
    }
}
