using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseBlackBoard
    {
        private INPCContext mContext;
        public INPCContext Context
        {
            get
            {
                return mContext;
            }

            set
            {
                if (mContext == value)
                {
                    return;
                }

                mContext = value;

                OnSetContext();
            }
        }

        public BaseAbstractLogicalObject GetLogicalObject(string query)
        {
            return Context.GetLogicalObject(query);
        }

        public IList<VisionObject> VisibleObjects
        {
            get
            {
                return new List<VisionObject>() { new VisionObject() };//tmp
            }
        }

        protected virtual void OnSetContext()
        {
        }

        public virtual void Bootstrap()
        {
#if DEBUG
            LogInstance.Log("BaseBlackBoard Bootstrap");
#endif
        }
    }
}
