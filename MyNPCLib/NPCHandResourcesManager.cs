using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCHandResourcesManager: INPCResourcesManager
    {
        public NPCHandResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        #endregion

        public void Bootstrap()
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                if (mState == StateOfNPCContext.Working)
                {
                    return;
                }

                mState = StateOfNPCContext.Working;
            }
        }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            throw new NotImplementedException();
        }

        public void UnRegProcess(ulong processId)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager UnRegProcess processId = {processId}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            //throw new NotImplementedException();
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("NPCHandResourcesManager Dispose");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCContext.Destroyed;
            }
        }
    }
}
