using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseNPCProcess : INPCProcess
    {
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private object mStateLocObj = new object();

        public StateOfNPCProcess State
        {
            get
            {
                lock(mStateLocObj)
                {
                    return mState;
                }
            }
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("BaseNPCContext Dispose");
#endif

            throw new NotImplementedException();
        }
    }
}
