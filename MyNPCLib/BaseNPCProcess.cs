using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseNPCProcess : INPCProcess
    {
        public KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;

        #region private members
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private object mStateLockObj = new object();
        private ulong mId;
        private ActivatorOfNPCProcessEntryPointInfo mActivator = new ActivatorOfNPCProcessEntryPointInfo();
        #endregion

        public StateOfNPCProcess State
        {
            get
            {
                lock(mStateLockObj)
                {
                    return mState;
                }
            }
        }

        private void StateChecker()
        {
            lock (mStateLockObj)
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

        public ulong Id
        {
            get
            {
                return mId;
            }

            set
            {
                StateChecker();

                mId = value;
            }
        }

        private NPCProcessInfo mNPCProcessInfo;

        public NPCProcessInfo Info
        {
            get
            {
                return mNPCProcessInfo;
            }

            set
            {
                mNPCProcessInfo = value;
            }
        }

        public void RunAsync(NPCInternalCommand command)
        {
#if DEBUG
            LogInstance.Log($"Begin BaseNPCProcess RunAsync command = {command}");
#endif

            StateChecker();

            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var targetEntryPoints = mActivator.GetRankedEntryPoints(Info, command.Params);

            if(targetEntryPoints.Count == 0)
            {
                lock (mStateLockObj)
                {
                    mState = StateOfNPCProcess.Faulted;
                }

                return;
            }

            var targetEntryPoint = targetEntryPoints.First();

#if DEBUG
            LogInstance.Log($"Begin BaseNPCProcess RunAsync targetEntryPoint = {targetEntryPoint}");
#endif

            //throw new NotImplementedException();

#if DEBUG
            LogInstance.Log($"End BaseNPCProcess RunAsync command = {command}");
#endif
        }

        private void NRun(NPCInternalCommand command)
        {
#if DEBUG
            LogInstance.Log($"Begin BaseNPCProcess NRun command = {command}");
#endif

            //throw new NotImplementedException();

#if DEBUG
            LogInstance.Log($"End BaseNPCProcess NRun command = {command}");
#endif
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("BaseNPCContext Dispose");
#endif
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCProcess.Destroyed;
            }

            throw new NotImplementedException();
        }
    }
}
