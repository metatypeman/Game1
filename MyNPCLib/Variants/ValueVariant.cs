using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class ValueVariant : BaseVariant
    {
        public ValueVariant(ValueExpressionNode expressionNode)
        {
            mExpressionNode = expressionNode;
            Value = mExpressionNode.Value;
        }

        public override KindOfVariant Kind => KindOfVariant.Value;

        public override bool IsValue => true;
        public override ValueVariant AsValue => this;

        private ValueExpressionNode mExpressionNode;
        public object Value { get; private set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            return sb.ToString();
        }
    }
}
