using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class EntityRefExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.EntityRef;
        public override bool IsEntityRef => true;
        public override EntityRefExpressionNode AsEntityRef => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new EntityRefExpressionNode();
            FillForClone(result, context);
            return result;
        }
    }
}
