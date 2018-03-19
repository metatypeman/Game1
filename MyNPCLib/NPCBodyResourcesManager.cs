﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCResourcesManager
    {
        public NPCBodyResourcesManager(IIdFactory idFactory)
        {
            mIdFactory = idFactory;
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        #endregion

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("NPCBodyResourcesManager Dispose");
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
    }
}
