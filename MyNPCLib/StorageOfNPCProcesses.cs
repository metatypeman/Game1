using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcesses : IDisposable
    {
        public StorageOfNPCProcesses(IIdFactory idFactory, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mStorageOfNPCProcessInfo = new StorageOfNPCProcessInfo(entityDictionary, npcProcessInfoCache);
        }

        #region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private StorageOfNPCProcessInfo mStorageOfNPCProcessInfo;
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;
        #endregion

        public void AddTypeOfProcess(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses AddTypeOfProcess type = {type?.FullName}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            mStorageOfNPCProcessInfo.AddTypeOfProcess(type);
        }

        public BaseNPCProcess GetProcess(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses GetProcess command = {command}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }


        }

        public void Dispose()
        {
            lock(mDisposeLockObj)
            {
                if(mIsDisposed)
                {
                    return;
                }

                mIsDisposed = true;
            }

            mStorageOfNPCProcessInfo.Dispose();
        }
    }
}
