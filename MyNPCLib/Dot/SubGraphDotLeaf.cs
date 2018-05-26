using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Dot
{
    public class SubGraphDotLeaf : BaseContainerLeaf
    {
        public SubGraphDotLeaf(DotContext context, ICGNode node)
            : base(context, node)
        {
        }

        protected override void PringBegin()
        {
            Sb.Append("subgraph ");
            Sb.Append(Name);
            Sb.AppendLine("{");
        }
    }
}
