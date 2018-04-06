using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseCommonNPCProcess: INPCProcess
    {
        protected readonly object StateLockObj = new object();
        protected StateOfNPCProcess mState = StateOfNPCProcess.Created;

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

                mContext = value;
            }
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

        public abstract StateOfNPCProcess State { get; set; }
        public event NPCProcessStateChanged OnStateChanged;

        protected void EmitOnStateChanged(StateOfNPCProcess state)
        {
            OnStateChanged?.Invoke(state);
        }

        public event Action OnRunningChanged;

        protected void EmitOnRunningChanged()
        {
            OnRunningChanged?.Invoke();
        }

        public event Action OnRanToCompletionChanged;

        protected void EmitOnRanToCompletionChanged()
        {
            OnRanToCompletionChanged?.Invoke();
        }

        public event Action OnCanceledChanged;

        protected void EmitOnCanceledChanged()
        {
            OnCanceledChanged?.Invoke();
        }

        public event Action OnFaultedChanged;

        protected void EmitOnFaultedChanged()
        {
            OnFaultedChanged?.Invoke();
        }

        public event Action OnDestroyedChanged;

        protected void EmitOnDestroyedChanged()
        {
            OnDestroyedChanged?.Invoke();
        }

        public abstract KindOfNPCProcess Kind { get; }

        public abstract void Dispose();

        public abstract ulong Id { get; set; }
        public abstract Task Task { get; set; }

        private float mLocalPriority = NPCProcessPriorities.Normal;
        private object mPriorityLockObj = new object();

        public float LocalPriority
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

        public float GlobalPriority
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

                lock (mPriorityLockObj)
                {
                    var result = mLocalPriority;

                    var parentProcess = mContext.GetParentProcess(Id);

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
