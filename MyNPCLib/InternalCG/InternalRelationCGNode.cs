using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public class InternalRelationCGNode: BaseInternalCGNode
    {
        public override KindOfCGNode Kind => KindOfCGNode.Relation;
        public override bool IsRelationNode => true;
        public override InternalRelationCGNode AsRelationNode => this;

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
            NSRemoveInputNode(node);
        }

        private void NSRemoveInputNode(BaseInternalCGNode node)
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
            NSRemoveOutputNode(node);
        }

        private void NSRemoveOutputNode(BaseInternalCGNode node)
        {
            NRemoveOutputNode(node);
            node.NRemoveInputNode(this);
        }

        public override void Destroy()
        {
            Parent = null;
            foreach(var node in Inputs)
            {
                NSRemoveInputNode(node);
            }

            foreach(var node in Outputs)
            {
                NSRemoveOutputNode(node);
            }
        }
    }
}
