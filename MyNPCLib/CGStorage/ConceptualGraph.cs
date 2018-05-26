using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ConceptualGraph : BaseConceptCGNode
    {
        public override KindOfCGNode Kind => KindOfCGNode.Graph;

        private IList<BaseCGNode> mChildren = new List<BaseCGNode>();

        public IList<BaseCGNode> Children
        {
            get
            {
                return mChildren;
            }
        }

        public override IList<ICGNode> ChildrenNodes => mChildren.Cast<ICGNode>().ToList();

        public void AddChild(BaseCGNode child)
        {
            if(child == null)
            {
                return;
            }

            if(mChildren.Contains(child))
            {
                return;
            }

            NAddChild(child);
            child.NSetParent(this);
        }

        internal void NAddChild(BaseCGNode child)
        {
            if (!mChildren.Contains(child))
            {
                mChildren.Add(child);
            }
        }

        public void RemoveChild(BaseCGNode child)
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

        internal void NRemoveChild(BaseCGNode child)
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
            foreach(var child in Children)
            {
                sb.Append(child.ToStringAsShortBrief(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Children)}");
            return sb.ToString();
        }
    }
}
