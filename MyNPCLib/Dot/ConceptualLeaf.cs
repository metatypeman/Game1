using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Dot
{
    public class ConceptualLeaf : BaseLeaf
    {
        public ConceptualLeaf(DotContext context, ICGNode node)
            : base(context)
        {
            mNode = node;
            Name = Context.GetNodeName();
            Context.RegLeaf(mNode, this);
        }

        private ICGNode mNode;

        public ICGNode Node
        {
            get
            {
                return mNode;
            }
        }

        protected override void OnRun()
        {
            Sb.Append(Name);
            Sb.Append("[shape=box,label=\"");
            Sb.Append(Node.Name);
            Sb.Append("\"];");
        }
    }
}
