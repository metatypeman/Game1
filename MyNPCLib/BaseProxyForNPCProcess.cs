using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseProxyForNPCProcess : BaseCommonNPCProcess
    {
        protected BaseProxyForNPCProcess(IEntityLogger entityLogger, ulong id, INPCContext context)
            : base(entityLogger)
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
                if (!StateTransitionChecker(mState, value))
                {
                    return;
                }

                mState = value;

                EmitChangingOfState(mState);
            }
        }

        protected override void EndOfProcessChanged()
        {
            NPCProcessHelpers.UnRegProcess(Context, this);
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
    }
}
