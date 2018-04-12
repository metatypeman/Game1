using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            mContext = context;

            switch (kindOfHand)
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
        private INPCContext mContext;
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
            LogInstance.Log($"NPCHandResourcesManager Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(id, mContext);

            var task = new Task(() => {
                NExecute(command, process);
            });

            process.Task = task;

            task.Start();

            return process;
        }

        private void NExecute(INPCCommand command, ProxyForNPCResourceProcess process)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager Begin NExecute command = {command}");
#endif



#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager End NExecute command = {command}");
#endif
        }

        public object Get(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager Get propertyName = {propertyName}");
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
            LogInstance.Log($"NPCHandResourcesManager UnRegProcess processId = {processId}");
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
