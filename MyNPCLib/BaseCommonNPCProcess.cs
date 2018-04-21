using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseCommonNPCProcess: BaseCommonNPCProcessWithEvents
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
                StateChecker();

                if(mContext == value)
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

        protected void StateChecker()
        {
            lock (StateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    throw new ElementIsNotActiveException();
                }

                if (mState != StateOfNPCProcess.Created)
                {
                    throw new ElementIsModifiedAfterActivationException();
                }
            }
        }

        private float mLocalPriority = NPCProcessPriorities.Normal;
        private object mPriorityLockObj = new object();

        public override float LocalPriority
        {
            get
            {
                lock (mPriorityLockObj)
                {
                    return mLocalPriority;
                }
            }

            set
            {
                lock (StateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        lock (mPriorityLockObj)
                        {
                            mLocalPriority = 0f;
                            return;
                        }                        
                    }
                }

                lock (mPriorityLockObj)
                {
                    mLocalPriority = value;
                }
            }
        }

        public override float GlobalPriority
        {
            get
            {
                lock (StateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        lock (mPriorityLockObj)
                        {
                            return 0f;
                        }
                    }
                }

#if DEBUG
                LogInstance.Log("BaseCommonNPCProcess GlobalPriority");
#endif

                lock (mPriorityLockObj)
                {

#if DEBUG
                    LogInstance.Log("BaseCommonNPCProcess GlobalPriority NEXT");
#endif

                    var result = mLocalPriority;

                    var parentProcess = mContext.GetParentProcess(Id);

#if DEBUG
                    LogInstance.Log($"BaseCommonNPCProcess GlobalPriority parentProcess == null = {parentProcess == null}");
#endif

                    if (parentProcess != null)
                    {
                        result *= parentProcess.GlobalPriority;
                    }

                    return result;
                }
            }
        }
    }
}
