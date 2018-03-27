using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseNPCProcess : INPCProcess
    {
        #region private members
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private object mStateLockObj = new object();
        private ulong mId;
        #endregion

        public StateOfNPCProcess State
        {
            get
            {
                lock(mStateLockObj)
                {
                    return mState;
                }
            }
        }

        private INPCContext mContext;
        public INPCContext Context
        {
            get
            {
                return mContext;
            }

            set;
        }

        public ulong Id
        {
            get
            {
                return mId;
            }

            set
            {
                lock (mStateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        throw new ElementIsNotActiveException();
                    }

                    if(mState != StateOfNPCProcess.Created)
                    {
                        throw new ElementIsModifiedAfterActivationException();
                    }              
                }

                mId = value;
            }
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("BaseNPCContext Dispose");
#endif
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCProcess.Destroyed;
            }

            throw new NotImplementedException();
        }
    }
}
