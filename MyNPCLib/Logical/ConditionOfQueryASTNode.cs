using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ConditionOfQueryASTNode : BaseQueryASTNode
    {
        public override bool IsCondition => true;
        public override ConditionOfQueryASTNode AsCondition => this;

        public ulong PropertyId { get; set; }
        public object Value { get; set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(PropertyId)} = {PropertyId}");
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            return sb.ToString();
        }
    }
}
