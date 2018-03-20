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
            mNPCProcessInfoCache = npcProcessInfoCache;
        }

        #region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private NPCProcessInfoCache mNPCProcessInfoCache;
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;
        #endregion

        public void AddTypeOfProcess(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses AddTypeOfProcess type = {type?.FullName}");
#endif

            throw new NotImplementedException();
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
        }
    }
}
