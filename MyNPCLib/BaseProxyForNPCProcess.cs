using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseProxyForNPCProcess : BaseCommonNPCProcess
    {
        protected BaseProxyForNPCProcess(ulong id, INPCContext context)
        {
            mId = id;
            Context = context;
        }

        #region private members
        private ulong mId;
        #endregion

        public override StateOfNPCProcess State
        {
            get
            {
                lock (StateLockObj)
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
                });
            }
        }

        public override void Dispose() { }

        public override ulong Id
        {
            get
            {
                return mId;
            }

            set
            {
            }
        }

        public override Task Task { get; set; }
    }
}
