using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class PrepositionalDTNode : IObjectToString, IShortObjectToString
    {
        public ATNExtendedToken PrepositionalExtendedToken { get; set; }
        public NounDTNode NounObject { get; set; }

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
            if (PrepositionalExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalExtendedToken)}");
                sb.Append(PrepositionalExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(PrepositionalExtendedToken)}");
            }

            if (NounObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
                sb.Append(NounObject.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounObject)}");
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
            if (PrepositionalExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalExtendedToken)}");
                sb.Append(PrepositionalExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(PrepositionalExtendedToken)}");
            }

            if (NounObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
                sb.Append(NounObject.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounObject)}");
            }

            return sb.ToString();
        }
    }
}
