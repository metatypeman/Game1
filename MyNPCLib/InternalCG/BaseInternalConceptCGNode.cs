using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public abstract class BaseInternalConceptCGNode : BaseInternalCGNode
    {
        public void AddInputNode(InternalRelationCGNode node)
        {
            NAddInputNode(node);
            node.NAddOutputNode(this);
        }

        public void RemoveInputNode(InternalRelationCGNode node)
        {
            NRemoveInputNode(node);
            node.NRemoveOutputNode(this);
        }

        public void AddOutputNode(InternalRelationCGNode node)
        {
            NAddOutputNode(node);
            node.NAddInputNode(this);
        }

        public void RemoveOutputNode(InternalRelationCGNode node)
        {
            NRemoveOutputNode(node);
            node.NRemoveInputNode(this);
        }
    }
}
