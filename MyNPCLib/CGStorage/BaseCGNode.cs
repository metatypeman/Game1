using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
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

        private IList<BaseCGNode> mInputsNodes = new List<BaseCGNode>();
        private IList<BaseCGNode> mOutpusNodes = new List<BaseCGNode>();

        public IList<BaseCGNode> InputsNodes
        {
            get
            {
                return mInputsNodes;
            }
        }

        public IList<BaseCGNode> OutpusNodes
        {
            get
            {
                return mOutpusNodes;
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
                sb.Append(Parent.ToStringAsShortBrief(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
            }

            sb.AppendLine($"{spaces}Begin {nameof(InputsNodes)}");
            foreach(var inputNode in InputsNodes)
            {
                sb.Append(inputNode.ToStringAsShortBrief(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(InputsNodes)}");

            sb.AppendLine($"{spaces}Begin {nameof(OutpusNodes)}");
            foreach(var outputNode in OutpusNodes)
            {
                sb.Append(outputNode.ToStringAsShortBrief(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(OutpusNodes)}");
            return sb.ToString();
        }

        public string ToStringAsShortBrief(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            var nameOfType = GetType().FullName;
            sb.AppendLine($"{spaces}Begin {nameOfType}");
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}End {nameOfType}");
            return sb.ToString();
        }
    }
}
