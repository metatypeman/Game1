using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseCommonNPCProcessWithEvents : INPCProcess
    {
        protected readonly object StateLockObj = new object();
        protected StateOfNPCProcess mState = StateOfNPCProcess.Created;

        public abstract StateOfNPCProcess State { get; set; }
        private event NPCProcessStateChanged mOnStateChanged;
        public event NPCProcessStateChanged OnStateChanged
        {
            add
            {
                mOnStateChanged += value;
                if(State != StateOfNPCProcess.Created)
                {
                    Task.Run(() => {
                        value(State);
                    });       
                }
            }

            remove
            {
                mOnStateChanged -= value;
            }
        }

        protected void EmitOnStateChanged(StateOfNPCProcess state)
        {
            mOnStateChanged?.Invoke(state);
        }

        private Action mOnRunningChanged;
        public event Action OnRunningChanged
        {
            add
            {
                mOnRunningChanged += value;

                if (State == StateOfNPCProcess.Running)
                {
                    Task.Run(() => {
                        value();
                    });            
                }
            }

            remove
            {
                mOnRunningChanged -= value;
            }
        }

        protected void EmitOnRunningChanged()
        {
            mOnRunningChanged?.Invoke();
        }

        private Action mOnRanToCompletionChanged;
        public event Action OnRanToCompletionChanged
        {
            add
            {
                mOnRanToCompletionChanged += value;

                if (State == StateOfNPCProcess.RanToCompletion)
                {
                    Task.Run(() => {
                        value();
                    });                  
                }
            }

            remove
            {
                mOnRanToCompletionChanged -= value;
            }
        }

        protected void EmitOnRanToCompletionChanged()
        {
            mOnRanToCompletionChanged?.Invoke();
        }

        private Action mOnCanceledChanged;
        public event Action OnCanceledChanged
        {
            add
            {
                mOnCanceledChanged += value;

                if (State == StateOfNPCProcess.Canceled)
                {
                    Task.Run(() => {
                        value();
                    });
                }
            }

            remove
            {
                mOnCanceledChanged -= value;
            }
        }

        protected void EmitOnCanceledChanged()
        {
            mOnCanceledChanged?.Invoke();
        }

        private Action mOnFaultedChanged;
        public event Action OnFaultedChanged
        {
            add
            {
                mOnFaultedChanged += value;

                if (State == StateOfNPCProcess.Faulted)
                {
                    Task.Run(() => {
                        value();
                    });
                }
            }

            remove
            {
                mOnFaultedChanged -= value;
            }
        }

        protected void EmitOnFaultedChanged()
        {
            mOnFaultedChanged?.Invoke();
        }

        private Action mOnDestroyedChanged;
        public event Action OnDestroyedChanged
        {
            add
            {
                mOnDestroyedChanged += value;

                if (State == StateOfNPCProcess.Destroyed)
                {
                    Task.Run(() => {
                        value();
                    });
                }
            }

            remove
            {
                mOnDestroyedChanged -= value;
            }
        }

        protected void EmitOnDestroyedChanged()
        {
            mOnDestroyedChanged?.Invoke();
        }

        public abstract KindOfNPCProcess Kind { get; }

        public abstract void Dispose();

        public abstract ulong Id { get; set; }
        public abstract Task Task { get; set; }
        public abstract float LocalPriority { get; set; }
        public abstract float GlobalPriority { get; }
    }
}
