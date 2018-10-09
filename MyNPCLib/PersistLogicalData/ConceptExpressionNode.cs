using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class ConceptExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Concept;
        public override bool IsConcept => true;
        public override ConceptExpressionNode AsConcept => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new ConceptExpressionNode();
            FillForClone(result, context);
            return result;
        }
    }
}
