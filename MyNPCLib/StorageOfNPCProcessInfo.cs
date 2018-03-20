using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcessInfo : IDisposable
    {
        public StorageOfNPCProcessInfo(IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache)
        {
            mEntityDictionary = entityDictionary;
            mNPCProcessInfoCache = npcProcessInfoCache;
        }

        #region private members
        private IEntityDictionary mEntityDictionary;
        private NPCProcessInfoCache mNPCProcessInfoCache;
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;
        #endregion

        public void AddTypeOfProcess(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcessInfo AddTypeOfProcess type = {type?.FullName}");
#endif

            throw new NotImplementedException();
        }

        public NPCProcessInfo GetNPCProcessInfo(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcessInfo AddTypeOfProcess type = {type?.FullName}");
#endif

            throw new NotImplementedException();
        }

        public NPCProcessInfo GetNPCProcessInfo(ulong key)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcessInfo AddTypeOfProcess key = {key}");
#endif

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }

                mIsDisposed = true;
            }
        }
    }
}
