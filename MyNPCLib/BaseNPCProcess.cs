using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseNPCProcess : INPCProcess
    {
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private object mStateLockObj = new object();

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

        protected virtual void FillProcessInfo(NPCProcessInfo processInfo)
        {
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
