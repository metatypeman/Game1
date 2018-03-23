using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcessInfo : IDisposable
    {
        public StorageOfNPCProcessInfo(IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache)
        {
            mFactory = new NPCProcessInfoFactory(entityDictionary);
            mNPCProcessInfoCache = npcProcessInfoCache;
        }

        #region private members
        private object mLockObj = new object();
        private NPCProcessInfoCache mNPCProcessInfoCache;
        private NPCProcessInfoFactory mFactory;
        private Dictionary<Type, NPCProcessInfo> mNPCProcessInfoDictByType = new Dictionary<Type, NPCProcessInfo>();
        private Dictionary<ulong, NPCProcessInfo> mNPCProcessInfoDictByKey = new Dictionary<ulong, NPCProcessInfo>();
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
                if(mNPCProcessInfoDictByType.ContainsKey(type))
                {
                    return;
                }

                NPCProcessInfo info = null;

                if(mNPCProcessInfoCache != null)
                {
                    info = mNPCProcessInfoCache.Get(type);
                }

                if(info == null)
                {
                    info = mFactory.CreateInfo(type);

                    if (mNPCProcessInfoCache != null)
                    {
                        var resultOfPutToCache = mNPCProcessInfoCache.Set(info);

                        if(resultOfPutToCache == false)
                        {
                            info = mNPCProcessInfoCache.Get(type);
                        }
                    }             
                }

                if(info != null)
                {
                    var key = info.Key;

                    if (mNPCProcessInfoDictByKey.ContainsKey(key))
                    {
                        return;
                    }

                    mNPCProcessInfoDictByType[type] = info;
                    mNPCProcessInfoDictByKey[key] = info;
                }
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
                if(mNPCProcessInfoDictByType.ContainsKey(type))
                {
                    return mNPCProcessInfoDictByType[type];
                }

                return null;
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
                if(mNPCProcessInfoDictByKey.ContainsKey(key))
                {
                    return mNPCProcessInfoDictByKey[key];
                }

                return null;
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
