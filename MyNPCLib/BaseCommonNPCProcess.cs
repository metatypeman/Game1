using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseCommonNPCProcess : BaseCommonNPCProcessWithEvents
    {
        protected BaseCommonNPCProcess()
        {
        }

        protected BaseCommonNPCProcess(IEntityLogger entityLogger)
            : base(entityLogger)
        {
        }

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
                if(mState == StateOfNPCProcess.Created)
                {
                    return;
                }

                if (mState == StateOfNPCProcess.Running)
                {
                    return;
                }

                //if (mState == StateOfNPCProcess.Destroyed)
                //{
                //    throw new ElementIsNotActiveException();
                //}

                //if (!(mState == StateOfNPCProcess.Created && mState == StateOfNPCProcess.Running))
                //{
                //    //throw new ElementIsModifiedAfterActivationException();
                //    throw new ElementIsNotActiveException();
                //}

                throw new ElementIsNotActiveException();
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
                Log("Begin");
#endif

                lock (mPriorityLockObj)
                {

#if DEBUG
                    Log("NEXT");
#endif

                    var result = mLocalPriority;

                    var parentProcess = mContext.GetParentProcess(Id);

#if DEBUG
                    Log($"parentProcess == null = {parentProcess == null}");
#endif

                    if (parentProcess != null)
                    {
                        result *= parentProcess.GlobalPriority;
                    }

                    return result;
                }
            }
        }

        protected void TryAsCancel()
        {
#if DEBUG
            Log("Begin");
#endif

            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();
        }

        protected CancellationToken? GetCancellationToken()
        {
            var currTaskId = Task.CurrentId;

            if (currTaskId.HasValue)
            {
                var cancelationToken = mContext.GetCancellationToken(currTaskId.Value);

                if (cancelationToken.HasValue)
                {
                    return cancelationToken;
                }
            }

            return null;
        }
    }
}
