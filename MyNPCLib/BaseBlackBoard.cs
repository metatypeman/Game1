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
