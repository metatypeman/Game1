using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum KindOfHand
    {
        Right,
        Left
    }

    public class NPCHandResourcesManager: INPCResourcesManager
    {
        public NPCHandResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, KindOfHand kindOfHand, INPCContext context)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;

            switch(kindOfHand)
            {
                case KindOfHand.Right:
                    mNPCHandHost = npcHostContext.RightHandHost;
                    break;

                case KindOfHand.Left:
                    mNPCHandHost = npcHostContext.LeftHandHost;
                    break;
            }
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private INPCHandHost mNPCHandHost;
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

        public object Get(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Get propertyName = {propertyName}");
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
