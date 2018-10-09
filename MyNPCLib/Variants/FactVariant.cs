using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class FactVariant : BaseVariant
    {
        public FactVariant(RuleInstance ruleInstance)
        {
            RuleInstance = ruleInstance;
        }

        public override KindOfVariant Kind => KindOfVariant.Fact;

        public override bool IsFact => true;
        public override FactVariant AsFact => this;

        public RuleInstance RuleInstance { get; private set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            if (RuleInstance == null)
            {
                sb.AppendLine($"{spaces}{nameof(RuleInstance)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RuleInstance)}");
                sb.Append(RuleInstance.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RuleInstance)}");
            }
            return sb.ToString();
        }
    }
}
