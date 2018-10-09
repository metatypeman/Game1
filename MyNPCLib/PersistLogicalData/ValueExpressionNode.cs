using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class ValueExpressionNode : BaseExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Value;
        public override bool IsValue => true;
        public override ValueExpressionNode AsValue => this;
        public object Value { get; set; }
        public KindOfValueType KindOfValueType { get; set; } = KindOfValueType.Unknown;
        
        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new ValueExpressionNode();
            result.Value = Value;
            result.KindOfValueType = KindOfValueType;
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.AppendLine($"{spaces}{nameof(KindOfValueType)} = {KindOfValueType}");
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.AppendLine($"{spaces}{nameof(KindOfValueType)} = {KindOfValueType}");
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
