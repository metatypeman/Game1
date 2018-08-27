using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public class EntityExpressionNode: BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Entity;
        public override bool IsEntity => true;
        public override EntityExpressionNode AsEntity => this;
        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new EntityExpressionNode();
            FillForClone(result, context);
            return result;
        }
    }
}
