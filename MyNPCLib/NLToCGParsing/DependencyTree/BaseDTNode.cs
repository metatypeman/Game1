using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public abstract class BaseDTNode : IObjectToString, IShortObjectToString
    {
        public virtual bool IsSentenceDTNode => false;
        public virtual SentenceDTNode AsSentenceDTNode => null;
        public virtual bool IsVerbDTNode => false;
        public virtual VerbDTNode AsVerbDTNode => null;
        public virtual bool IsNounDTNode => false;
        public virtual NounDTNode AsNounDTNode => null;
        public virtual bool IsPrepositionalDTNode => false;
        public virtual PrepositionalDTNode AsPrepositionalDTNode => null;
        public virtual bool IsAdjectiveDTNode => false;
        public virtual AdjectiveDTNode AsAdjectiveDTNode => null;
        public virtual bool IsDeterminerDTNode => false;
        public virtual DeterminerDTNode AsDeterminerDTNode => null;

        public ATNExtendedToken ExtendedToken { get; set; }

        private BaseDTNode mParent;

        public BaseDTNode Parent
        {
            get
            {
                return mParent;
            }
        }

        internal void NSetParent(BaseDTNode parent)
        {
            if (mParent != parent)
            {
                mParent = parent;
            }
        }

        internal void NRemoveParentIfNot(BaseDTNode parent)
        {
            if (mParent == parent)
            {
                return;
            }

            if (mParent != null)
            {
                mParent.OnRemoveObjFromProp(this);
            }

            mParent = null;
        }

        internal void NRemoveParent(BaseDTNode parent)
        {
            if (mParent != parent)
            {
                return;
            }

            if (mParent != null)
            {
                mParent.OnRemoveObjFromProp(this);
            }

            mParent = null;
        }

        protected abstract void OnRemoveObjFromProp(BaseDTNode obj);

        public abstract KindOfDTChild GetKindOfDTChild(BaseDTNode obj);

        public abstract void SetValue(BaseDTNode obj, KindOfDTChild kindOfDTChild);

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
            if (ExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(ExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ExtendedToken)}");
                sb.Append(ExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ExtendedToken)}");
            }
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
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (ExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(ExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ExtendedToken)}");
                sb.Append(ExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ExtendedToken)}");
            }
            return sb.ToString();
        }
    }
}
