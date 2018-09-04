using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public class BusOfCGStorages
    {
        private readonly object mStoragesDictLockObj = new object();
        private Dictionary<ulong, ICGStorage> mStoragesDict = new Dictionary<ulong, ICGStorage>();

        public void AddStorage(ulong entityId, ICGStorage storage)
        {
            lock(mStoragesDictLockObj)
            {
                mStoragesDict[entityId] = storage;
            }
        }

        public void RemoveStorage(ulong entityId)
        {
            lock (mStoragesDictLockObj)
            {
                mStoragesDict.Remove(entityId);
            }
        }

        public ICGStorage GetStorage(ulong entityId)
        {
            lock (mStoragesDictLockObj)
            {
                if(mStoragesDict.ContainsKey(entityId))
                {
                    return mStoragesDict[entityId];
                }

                return null;
            }
        }
    }
}
