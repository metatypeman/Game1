using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCThingProcess : BaseCommonNPCProcessWithEvents
    {
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

        public override ulong Id
        {
            get
            {
                return 0ul;
            }

            set
            {
            }
        } 

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Resource;
        public override Task Task
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        public override float LocalPriority { get; set; }
        public override float GlobalPriority => 0f;
        public override void Dispose()
        {
        }
    }
}
