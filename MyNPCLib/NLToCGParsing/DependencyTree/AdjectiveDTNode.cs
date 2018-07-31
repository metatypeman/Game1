using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class AdjectiveDTNode : BaseDTNode
    {
        public override bool IsAdjectiveDTNode => true;
        public override AdjectiveDTNode AsAdjectiveDTNode => this;

        public ATNExtendedToken AdjectiveExtendedToken { get; set; }

        public override void SetObject(BaseDTNode obj)
        {
            throw new NotImplementedException();
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (AdjectiveExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(AdjectiveExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AdjectiveExtendedToken)}");
                sb.Append(AdjectiveExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AdjectiveExtendedToken)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (AdjectiveExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(AdjectiveExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AdjectiveExtendedToken)}");
                sb.Append(AdjectiveExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AdjectiveExtendedToken)}");
            }
            return sb.ToString();
        }
    }
}
