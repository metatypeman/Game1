using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class RelationExpressionNode : BaseExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Relation;
        public override bool IsRelation => true;
        public override RelationExpressionNode AsRelation => this;
    }
}
