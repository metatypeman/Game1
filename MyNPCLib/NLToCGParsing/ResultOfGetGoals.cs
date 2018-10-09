using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ResultOfGetGoals: IObjectToString
    {
        public ATNExtendedToken ExtendedToken { get; set; }
        public IList<GoalOfATNExtendToken> Goals { get; set; }

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
            var nextNSpace = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            if (ExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(ExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ExtendedToken)}");
                sb.Append(ExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ExtendedToken)}");
            }
            if (Goals == null)
            {
                sb.AppendLine($"{spaces}{nameof(Goals)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Goals)}");
                foreach (var goal in Goals)
                {
                    sb.AppendLine($"{nextNSpace}{goal}");
                }
                sb.AppendLine($"{spaces}End {nameof(Goals)}");
            }
            return sb.ToString();
        }
    }
}
