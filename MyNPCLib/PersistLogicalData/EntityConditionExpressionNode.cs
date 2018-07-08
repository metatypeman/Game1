using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class EntityConditionExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.EntityCondition;
        public override bool IsEntityCondition => true;
        public override EntityConditionExpressionNode AsEntityCondition => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new EntityConditionExpressionNode();
            FillForClone(result, context);
            return result;
        }
    }
}
