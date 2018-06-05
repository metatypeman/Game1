﻿using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public abstract class BaseInternalCGNode: ICGNode
    {
        public abstract KindOfCGNode Kind { get; }
        public string Name { get; set; }
        public ulong Key { get; set; }
        public virtual bool IsConceptualGraph => false;
        public virtual InternalConceptualGraph AsConceptualGraph => null;
        public virtual bool IsConceptNode => false;
        public virtual InternalConceptCGNode AsConceptNode => null;
        public virtual bool IsRelationNode => false;
        public virtual InternalRelationCGNode AsRelationNode => null;
        public virtual bool IsGraphOrConceptNode => false;
        public virtual BaseInternalConceptCGNode AsGraphOrConceptNode => null;

        private InternalConceptualGraph mParent;

        public InternalConceptualGraph Parent
        {
            get
            {
                return mParent;
            }

            set
            {
                if (mParent == value)
                {
                    return;
                }

                if (mParent != null)
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

        internal void NSetParent(InternalConceptualGraph parent)
        {
            if (mParent != parent)
            {
                mParent = parent;
            }
        }

        internal void NRemoveParent(InternalConceptualGraph parent)
        {
            if (mParent == parent)
            {
                mParent = null;
            }
        }

        public virtual IList<ICGNode> ChildrenNodes => new List<ICGNode>();

        private IList<BaseInternalCGNode> mInputsNodes = new List<BaseInternalCGNode>();
        private IList<BaseInternalCGNode> mOutputsNodes = new List<BaseInternalCGNode>();

        public IList<BaseInternalCGNode> Inputs
        {
            get
            {
                return mInputsNodes;
            }
        }

        public IList<ICGNode> InputNodes => mInputsNodes.Cast<ICGNode>().ToList();

        public IList<BaseInternalCGNode> Outputs
        {
            get
            {
                return mOutputsNodes;
            }
        }

        public IList<ICGNode> OutputNodes => mOutputsNodes.Cast<ICGNode>().ToList();

        internal void NAddInputNode(BaseInternalCGNode node)
        {
            if (!mInputsNodes.Contains(node))
            {
                mInputsNodes.Add(node);
            }
        }

        internal void NRemoveInputNode(BaseInternalCGNode node)
        {
            if (mInputsNodes.Contains(node))
            {
                mInputsNodes.Remove(node);
            }
        }

        internal void NAddOutputNode(BaseInternalCGNode node)
        {
            if (!mOutputsNodes.Contains(node))
            {
                mOutputsNodes.Add(node);
            }
        }

        internal void NRemoveOutputNode(BaseInternalCGNode node)
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
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");     
            if (Parent == null)
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
            foreach (var inputNode in Inputs)
            {
                sb.Append(inputNode.PropertiesToShortSting(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Inputs)}");

            sb.AppendLine($"{spaces}Begin {nameof(Outputs)}");
            foreach (var outputNode in Outputs)
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

        public virtual string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }
    }
}
