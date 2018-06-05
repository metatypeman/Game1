using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public class InternalConceptualGraph: BaseInternalConceptCGNode
    {
        public override KindOfCGNode Kind => KindOfCGNode.Graph;

        private IList<BaseInternalCGNode> mChildren = new List<BaseInternalCGNode>();

        public IList<BaseInternalCGNode> Children
        {
            get
            {
                return mChildren;
            }
        }

        public override IList<ICGNode> ChildrenNodes => mChildren.Cast<ICGNode>().ToList();

        public void AddChild(BaseInternalCGNode child)
        {
            if (child == null)
            {
                return;
            }

            if (mChildren.Contains(child))
            {
                return;
            }

            NAddChild(child);
            child.NSetParent(this);
        }

        internal void NAddChild(BaseInternalCGNode child)
        {
            if (!mChildren.Contains(child))
            {
                mChildren.Add(child);
            }
        }

        public void RemoveChild(BaseInternalCGNode child)
        {
            if (child == null)
            {
                return;
            }

            if (!mChildren.Contains(child))
            {
                return;
            }

            NRemoveChild(child);
            NRemoveParent(this);
        }

        internal void NRemoveChild(BaseInternalCGNode child)
        {
            if (mChildren.Contains(child))
            {
                mChildren.Remove(child);
            }
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}Begin {nameof(Children)}");
            foreach (var child in Children)
            {
                sb.Append(child.ToStringAsShortBrief(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Children)}");
            return sb.ToString();
        }
    }
}
