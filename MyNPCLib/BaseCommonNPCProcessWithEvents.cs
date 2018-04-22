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

        protected bool StateTransitionChecker(StateOfNPCProcess currentState, StateOfNPCProcess newState)
        {
            if (mState == value)
            {
                return;
            }
        }

        protected void EmitChangingOfState(StateOfNPCProcess state)
        {
            EmitOnStateChanged(state);

            switch (state)
            {
                case StateOfNPCProcess.Created:
                    break;
                case StateOfNPCProcess.Running:
                    EmitOnRunningChanged();
                    break;

                case StateOfNPCProcess.RanToCompletion:
                    EmitOnRanToCompletionChanged();
                    break;

                case StateOfNPCProcess.Canceled:
                    EmitOnCanceledChanged();
                    break;

                case StateOfNPCProcess.Faulted:
                    EmitOnFaultedChanged();
                    break;

                case StateOfNPCProcess.Destroyed:
                    EmitOnDestroyedChanged();
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private event NPCProcessStateChanged mOnStateChanged;
        public event NPCProcessStateChanged OnStateChanged
        {
            add
            {
                mOnStateChanged += value;
                if (State != StateOfNPCProcess.Created)
                {
                    Task.Run(() => {
                        value(this, State);
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
            Task.Run(() => {
                mOnStateChanged?.Invoke(this, state);
            });
        }

        private NPCProcessConcreteStateChanged mOnRunningChanged;
        public event NPCProcessConcreteStateChanged OnRunningChanged
        {
            add
            {
                mOnRunningChanged += value;

                if (State == StateOfNPCProcess.Running)
                {
                    Task.Run(() => {
                        value(this);
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
            Task.Run(() => {
                mOnRunningChanged?.Invoke(this);
            });
        }

        private NPCProcessConcreteStateChanged mOnRanToCompletionChanged;
        public event NPCProcessConcreteStateChanged OnRanToCompletionChanged
        {
            add
            {
                mOnRanToCompletionChanged += value;

                if (State == StateOfNPCProcess.RanToCompletion)
                {
                    Task.Run(() => {
                        value(this);
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
            Task.Run(() => {
                mOnRanToCompletionChanged?.Invoke(this);
            });
        }

        private NPCProcessConcreteStateChanged mOnCanceledChanged;
        public event NPCProcessConcreteStateChanged OnCanceledChanged
        {
            add
            {
                mOnCanceledChanged += value;

                if (State == StateOfNPCProcess.Canceled)
                {
                    Task.Run(() => {
                        value(this);
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
            Task.Run(() => {
                mOnCanceledChanged?.Invoke(this);
            });
        }

        private NPCProcessConcreteStateChanged mOnFaultedChanged;
        public event NPCProcessConcreteStateChanged OnFaultedChanged
        {
            add
            {
                mOnFaultedChanged += value;

                if (State == StateOfNPCProcess.Faulted)
                {
                    Task.Run(() => {
                        value(this);
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
            Task.Run(() => {
                mOnFaultedChanged?.Invoke(this);
            });
        }

        private NPCProcessConcreteStateChanged mOnDestroyedChanged;
        public event NPCProcessConcreteStateChanged OnDestroyedChanged
        {
            add
            {
                mOnDestroyedChanged += value;

                if (State == StateOfNPCProcess.Destroyed)
                {
                    Task.Run(() => {
                        value(this);
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
            Task.Run(() => {
                mOnDestroyedChanged?.Invoke(this);
            });
        }

        public abstract KindOfNPCProcess Kind { get; }

        public abstract void Dispose();

        public abstract ulong Id { get; set; }
        public abstract Task Task { get; set; }
        public abstract float LocalPriority { get; set; }
        public abstract float GlobalPriority { get; }
    }
}
