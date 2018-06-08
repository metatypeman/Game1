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
    }
}
