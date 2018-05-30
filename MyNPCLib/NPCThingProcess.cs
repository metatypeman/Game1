using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCThingProcess : BaseCommonNPCProcessWithEvents
    {
        public NPCThingProcess(IEntityLogger entityLogger)
            : base(entityLogger)
        {
        }

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

                EmitChangingOfState(mState);
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
        public override float LocalPriority { get; set; }
        public override float GlobalPriority => 0f;
        public override void Dispose()
        {
        }
    }
}
