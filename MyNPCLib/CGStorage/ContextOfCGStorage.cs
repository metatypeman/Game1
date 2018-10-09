using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ContextOfCGStorage
    {
        public ContextOfCGStorage(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
            mGlobalCGStorage = new GlobalCGStorage(entityDictionary);
            mMainCGStorage = new ConsolidatedCGStorage(entityDictionary);

            var storageOptions = new SettingsOfStorageForSearchingInThisSession();
            storageOptions.Storage = mGlobalCGStorage;
            storageOptions.MaxDeph = null;
            storageOptions.UseFacts = true;
            storageOptions.UseAdditionalInstances = true;
            storageOptions.UseProductions = true;
            storageOptions.Priority = 1;

            mMainCGStorage.AddStorage(storageOptions);
        }

        private IEntityDictionary mEntityDictionary;

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        private GlobalCGStorage mGlobalCGStorage;

        public GlobalCGStorage GlobalCGStorage => mGlobalCGStorage;

        private ConsolidatedCGStorage mMainCGStorage;

        public ICGStorage MainCGStorage => mMainCGStorage;

        private ICGStorage mHostStorage;

        public ICGStorage HostStorage => mHostStorage;

        public void SetHostStorage(ICGStorage storage)
        {
            mHostStorage = storage;

            var storageOptions = new SettingsOfStorageForSearchingInThisSession();
            storageOptions.Storage = storage;
            storageOptions.MaxDeph = null;
            storageOptions.UseFacts = true;
            storageOptions.UseAdditionalInstances = true;
            storageOptions.UseProductions = false;
            storageOptions.Priority = 1;

            mMainCGStorage.AddStorage(storageOptions);
        }

        public void SetWorldHostStorage(ICGStorage storage)
        {
            var storageOptions = new SettingsOfStorageForSearchingInThisSession();
            storageOptions.Storage = storage;
            storageOptions.MaxDeph = null;
            storageOptions.UseFacts = true;
            storageOptions.UseAdditionalInstances = true;
            storageOptions.UseProductions = false;
            storageOptions.Priority = 1;

            mMainCGStorage.AddStorage(storageOptions);
        }
    }
}
