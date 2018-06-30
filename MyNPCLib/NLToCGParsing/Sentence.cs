using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class Sentence: IObjectToString, IShortObjectToString
    {
        public NounPhrase NounPhrase { get; set; }

        public Sentence Fork()
        {
            var result = new Sentence();
            result.NounPhrase = NounPhrase?.Fork();
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
            if (NounPhrase == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounPhrase)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounPhrase)}");
                sb.Append(NounPhrase.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounPhrase)}");
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
            if (NounPhrase == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounPhrase)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounPhrase)}");
                sb.Append(NounPhrase.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounPhrase)}");
            }
            return sb.ToString();
        }
    }
}
