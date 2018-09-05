using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public class BusOfCGStorages: IBusOfCGStorages
    {
        private readonly object mStoragesDictLockObj = new object();
        private Dictionary<ulong, ICGStorage> mStoragesDict = new Dictionary<ulong, ICGStorage>();

        public void AddStorage(IHostLogicalObjectStorageForBus storage)
        {
            lock(mStoragesDictLockObj)
            {
                throw new NotImplementedException();
                //mStoragesDict[entityId] = storage;
            }
        }

        public void RemoveStorage(ulong entityKey)
        {
            lock (mStoragesDictLockObj)
            {
                throw new NotImplementedException();
                //mStoragesDict.Remove(entityId);
            }
        }

        public ICGStorage GetStorage(ulong entityKey)
        {
            lock (mStoragesDictLockObj)
            {
                throw new NotImplementedException();
                //if(mStoragesDict.ContainsKey(entityId))
                //{
                //    return mStoragesDict[entityId];
                //}

                //return null;
            }
        }
    }
}
