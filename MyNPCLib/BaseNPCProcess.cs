﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void RunAsync(NPCInternalCommand command, NPCProcessEntryPointInfo entryPointInfo)
        {
#if DEBUG
            LogInstance.Log($"Begin BaseNPCProcess RunAsync command = {command}");
            LogInstance.Log($"Begin BaseNPCProcess RunAsync entryPointInfo = {entryPointInfo}");
#endif

            StateChecker();

            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if(entryPointInfo == null)
            {
                throw new ArgumentNullException(nameof(entryPointInfo));
            }

            Task.Run(() => {
                NRun(entryPointInfo, command);
            });
            //throw new NotImplementedException();

#if DEBUG
            LogInstance.Log($"End BaseNPCProcess RunAsync command = {command}");
#endif
        }

        private void NRun(NPCProcessEntryPointInfo entryPointInfo, NPCInternalCommand command)
        {
#if DEBUG
            LogInstance.Log($"Begin BaseNPCProcess NRun command = {command}");
#endif

            try
            {
                lock (mStateLockObj)
                {
                    mState = StateOfNPCProcess.Running;
                }

                mActivator.CallEntryPoint(this, entryPointInfo, command.Params);

                lock (mStateLockObj)
                {
                    if (mState == StateOfNPCProcess.Running)
                    {
                        mState = StateOfNPCProcess.RanToCompletion;
                    }
                }
            }
            catch(Exception e)
            {
                if (mState == StateOfNPCProcess.Running)
                {
                    mState = StateOfNPCProcess.Faulted;
                }

#if DEBUG
                LogInstance.Log($"End BaseNPCProcess NRun e = {e}");
#endif
            }

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
