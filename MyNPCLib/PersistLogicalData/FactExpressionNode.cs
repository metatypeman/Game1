using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class FactExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Fact;
        public override bool IsFact => true;
        public override FactExpressionNode AsFact => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new FactExpressionNode();
            FillForClone(result, context);
            return result;
        }
    }
}
