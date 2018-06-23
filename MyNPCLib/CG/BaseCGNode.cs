using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CG
{
    public abstract class BaseCGNode: ICGNode
    {
        public abstract KindOfCGNode Kind { get; }
        public string Name { get; set; }

        private ConceptualGraph mParent;

        public ConceptualGraph Parent
        {
            get
            {
                return mParent;
            }

            set
            {
                if(mParent == value)
                {
                    return;
                }

                if(mParent != null)
                {
                    mParent.NRemoveChild(this);
                }

                mParent = value;

                if (mParent != null)
                {
                    mParent.NAddChild(this);
                }
            }
        }

        internal void NSetParent(ConceptualGraph parent)
        {
            if (mParent != parent)
            {
                mParent = parent;
            }           
        }

        internal void NRemoveParent(ConceptualGraph parent)
        {
            if(mParent == parent)
            {
                mParent = null;
            }
        }

        public virtual IList<ICGNode> ChildrenNodes => new List<ICGNode>();

        private IList<BaseCGNode> mInputsNodes = new List<BaseCGNode>();
        private IList<BaseCGNode> mOutputsNodes = new List<BaseCGNode>();

        public IList<BaseCGNode> Inputs
        {
            get
            {
                return mInputsNodes;
            }
        }

        public IList<ICGNode> InputNodes => mInputsNodes.Cast<ICGNode>().ToList();

        public IList<BaseCGNode> Outputs
        {
            get
            {
                return mOutputsNodes;
            }
        }

        public IList<ICGNode> OutputNodes => mOutputsNodes.Cast<ICGNode>().ToList();

        internal void NAddInputNode(BaseCGNode node)
        {
            if(!mInputsNodes.Contains(node))
            {
                mInputsNodes.Add(node);
            }
        }

        internal void NRemoveInputNode(BaseCGNode node)
        {
            if (mInputsNodes.Contains(node))
            {
                mInputsNodes.Remove(node);
            }
        }

        internal void NAddOutputNode(BaseCGNode node)
        {
            if (!mOutputsNodes.Contains(node))
            {
                mOutputsNodes.Add(node);
            }
        }

        internal void NRemoveOutputNode(BaseCGNode node)
        {
            if (mOutputsNodes.Contains(node))
            {
                mOutputsNodes.Remove(node);
            }
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            if(Parent == null)
            {
                sb.AppendLine($"{spaces}{nameof(Parent)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Parent)}");
                sb.Append(Parent.PropertiesToShortSting(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
            }

            sb.AppendLine($"{spaces}Begin {nameof(Inputs)}");
            foreach(var inputNode in Inputs)
            {
                sb.Append(inputNode.PropertiesToShortSting(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Inputs)}");

            sb.AppendLine($"{spaces}Begin {nameof(Outputs)}");
            foreach(var outputNode in Outputs)
            {
                sb.Append(outputNode.PropertiesToShortSting(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Outputs)}");
            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }

        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }
    }
}
