using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public class InternalRelationCGNode: BaseInternalCGNode
    {
        public override KindOfCGNode Kind => KindOfCGNode.Relation;

        public void AddInputNode(BaseInternalConceptCGNode node)
        {
            NAddInputNode(node);
            node.NAddOutputNode(this);
        }

        public void AddInputNode(InternalRelationCGNode node)
        {
            NAddInputNode(node);
            node.NAddOutputNode(this);
        }

        public void RemoveInputNode(BaseInternalConceptCGNode node)
        {
            NRemoveInputNode(node);
            node.NRemoveOutputNode(this);
        }

        public void RemoveInputNode(InternalRelationCGNode node)
        {
            NRemoveInputNode(node);
            node.NRemoveOutputNode(this);
        }

        public void AddOutputNode(BaseInternalConceptCGNode node)
        {
            NAddOutputNode(node);
            node.NAddInputNode(this);
        }

        public void AddOutputNode(InternalRelationCGNode node)
        {
            NAddOutputNode(node);
            node.NAddInputNode(this);
        }

        public void RemoveOutputNode(BaseInternalConceptCGNode node)
        {
            NRemoveOutputNode(node);
            node.NRemoveInputNode(this);
        }

        public void RemoveOutputNode(InternalRelationCGNode node)
        {
            NRemoveOutputNode(node);
            node.NRemoveInputNode(this);
        }
    }
}
