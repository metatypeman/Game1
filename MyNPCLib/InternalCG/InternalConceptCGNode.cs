using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public class InternalConceptCGNode: BaseInternalConceptCGNode
    {
        public override KindOfCGNode Kind => KindOfCGNode.Concept;
        public override bool IsConceptNode => true;
        public override InternalConceptCGNode AsConceptNode => this;

        public override void Destroy()
        {
            Parent = null;
            foreach (var node in Inputs)
            {
                NSRemoveInputNode(node);
            }

            foreach (var node in Outputs)
            {
                NSRemoveOutputNode(node);
            }
        }
    }
}
