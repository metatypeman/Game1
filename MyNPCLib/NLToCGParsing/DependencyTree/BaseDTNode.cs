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

        internal void RemoveParent(BaseDTNode parent)
        {
            if (mParent == parent)
            {
                mParent = null;

                //Here we need to remove this from its old place.
                throw new NotImplementedException();
            }
        }

        internal void RemoveFromProperty(BaseDTNode child)
        {
            throw new NotImplementedException();
        }

        public abstract void SetObject(BaseDTNode obj);

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public abstract string PropertiesToSting(uint n);

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public abstract string PropertiesToShortSting(uint n);
    }
}
