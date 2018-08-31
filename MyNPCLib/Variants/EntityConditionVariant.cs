using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class EntityConditionVariant : BaseVariant
    {
        public EntityConditionVariant(RuleInstance ruleInstance)
        {
            RuleInstance = ruleInstance;
        }

        public override KindOfVariant Kind => KindOfVariant.EntityCondition;

        public override bool IsEntityCondition => true;
        public override EntityConditionVariant AsEntityCondition => this;

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
