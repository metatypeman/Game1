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
        private object mLockObj = new object();
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

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            lock (mLockObj)
            {
                throw new NotImplementedException();
            }         
        }

        public NPCProcessInfo GetNPCProcessInfo(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcessInfo AddTypeOfProcess type = {type?.FullName}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            lock (mLockObj)
            {
                throw new NotImplementedException();
            }
        }

        public NPCProcessInfo GetNPCProcessInfo(ulong key)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcessInfo AddTypeOfProcess key = {key}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            if (key == 0u)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (mLockObj)
            {
                throw new NotImplementedException();
            }
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
