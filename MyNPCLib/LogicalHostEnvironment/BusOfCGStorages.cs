using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public class BusOfCGStorages : IBusOfCGStorages
    {
        public BusOfCGStorages(IEntityDictionary entityDictionary)
        {
            mStorageWithPublicFacts = new ConsolidatedCGStorage(entityDictionary);
        }

        private readonly object mStoragesDictLockObj = new object();
        private ConsolidatedCGStorage mStorageWithPublicFacts;

        public ICGStorage GeneralStorageWithPublicFacts => mStorageWithPublicFacts;

        private Dictionary<ulong, ICGStorage> mStoragesWithVisibleFactsDict = new Dictionary<ulong, ICGStorage>();

        public void AddStorage(IHostLogicalObjectStorageForBus storage)
        {
            lock(mStoragesDictLockObj)
            {
                var entityKey = storage.EntityId;

#if DEBUG
                //LogInstance.Log($"entityKey = {entityKey}");
#endif

                mStoragesWithVisibleFactsDict[entityKey] = storage.VisibleHost;

                var storageOptions = new SettingsOfStorageForSearchingInThisSession();
                storageOptions.Storage = storage.PublicHost;
                storageOptions.MaxDeph = null;
                storageOptions.UseFacts = true;
                storageOptions.UseAdditionalInstances = true;
                storageOptions.UseProductions = false;
                storageOptions.Priority = 1;

                mStorageWithPublicFacts.AddStorage(storageOptions);
            }
        }

        public ICGStorage GetStorageWithVisibleFacts(ulong entityKey)
        {
            lock (mStoragesDictLockObj)
            {
                if (mStoragesWithVisibleFactsDict.ContainsKey(entityKey))
                {
                    return mStoragesWithVisibleFactsDict[entityKey];
                }

                return null;
            }
        }
    }
}
