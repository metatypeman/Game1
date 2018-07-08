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

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new FuzzyLogicValueExpressionNode();
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
            return result;
        }
    }
}
