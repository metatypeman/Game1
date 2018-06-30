using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class NounPhrase : IObjectToString, IShortObjectToString
    {
        public ATNExtendedToken Noun { get; set; }
        public List<ATNExtendedToken> Determiners { get; set; } = new List<ATNExtendedToken>();

        public NounPhrase Fork()
        {
            var result = new NounPhrase();
            result.Noun = Noun;
            return result;
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

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
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
