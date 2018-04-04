using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseProxyForNPCProcess : INPCProcess
    {
        protected BaseProxyForNPCProcess(ulong id)
        {
            mId = id;
        }

        public abstract KindOfNPCProcess Kind { get; }

        #region private members
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private object mStateLockObj = new object();
        private ulong mId;
        #endregion

        public StateOfNPCProcess State
        {
            get
            {
                lock (mStateLockObj)
                {
                    return mState;
                }
            }

            set
            {
                if (mState == value)
                {
                    return;
                }

                mState = value;

                var state = mState;
                Task.Run(() => {
                    OnStateChanged?.Invoke(state);

                    switch (state)
                    {
                        case StateOfNPCProcess.Created:
                            break;
                        case StateOfNPCProcess.Running:
                            OnRunningChanged?.Invoke();
                            break;

                        case StateOfNPCProcess.RanToCompletion:
                            OnRanToCompletionChanged?.Invoke();
                            break;

                        case StateOfNPCProcess.Canceled:
                            OnCanceledChanged?.Invoke();
                            break;

                        case StateOfNPCProcess.Faulted:
                            OnFaultedChanged?.Invoke();
                            break;

                        case StateOfNPCProcess.Destroyed:
                            OnDestroyedChanged?.Invoke();
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
                    }
                });
            }
        }

        public event NPCProcessStateChanged OnStateChanged;
        public event Action OnRunningChanged;
        public event Action OnRanToCompletionChanged;
        public event Action OnCanceledChanged;
        public event Action OnFaultedChanged;
        public event Action OnDestroyedChanged;

        public void Dispose() { }

        public ulong Id
        {
            get
            {
                return mId;
            }
        }

        public Task Task { get; set; }
    }
}
