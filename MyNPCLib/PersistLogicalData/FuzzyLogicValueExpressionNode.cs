using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class FuzzyLogicValueExpressionNode : BaseExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.FuzzyLogicValue;
        public override bool IsFuzzyLogicValue => true;
        public override FuzzyLogicValueExpressionNode AsFuzzyLogicValue => this;
        public float Value { get; set; }

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new FuzzyLogicValueExpressionNode();
            result.Value = Value;
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
