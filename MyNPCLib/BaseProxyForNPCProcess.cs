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

                EmitChangingOfState(mState);
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
