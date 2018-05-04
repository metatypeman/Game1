using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalStorage : ILogicalStorage
    {
        public LogicalStorage(IEntityDictionary entityDictionary, ILogicalStorage hostLogicalStorage)
        {
            mHostLogicalStorage = hostLogicalStorage;

            mHostLogicalStorage.OnChanged += MHostLogicalStorage_OnChanged;

            mEntityDictionary = entityDictionary;
            mStorageOfPassiveLogicalObjects = new StorageOfPassiveLogicalObjects();
            mLogicalIndexStorage = new LogicalIndexStorage();
            mLogicalIndexStorage.OnChanged += MHostLogicalStorage_OnChanged;
        }

        private void MHostLogicalStorage_OnChanged()
        {
#if DEBUG
            LogInstance.Log("LogicalStorage MHostLogicalStorage_OnChanged");
#endif

            OnChanged?.Invoke();
        }

        private ILogicalStorage mHostLogicalStorage;
        private IEntityDictionary mEntityDictionary;
        private StorageOfPassiveLogicalObjects mStorageOfPassiveLogicalObjects;
        private LogicalIndexStorage mLogicalIndexStorage;

        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mLogicalIndexStorage.PutPropertyValue(entityId, propertyId, value);
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalStorage GetEntitiesIdsList propertyId = {propertyId} value = {value}");
#endif

            return mLogicalIndexStorage.GetEntitiesIdsList(propertyId, value);
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
#if DEBUG
            LogInstance.Log("LogicalStorage GetAllEntitiesIdsList");
#endif

            return mLogicalIndexStorage.GetAllEntitiesIdsList();
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
#if DEBUG
            LogInstance.Log("LogicalStorage GetEntitiesIdList");
#endif

            var entitiesIdList = plan.GetEntitiesIdList(mLogicalIndexStorage);

            var entitiesIdListOfHost = mHostLogicalStorage.GetEntitiesIdList(plan);

            if(entitiesIdListOfHost.Count > 0)
            {
                foreach(var entityId in entitiesIdList)
                {
                    if(entitiesIdList.Contains(entityId))
                    {
                        continue;
                    }

                    entitiesIdList.Add(entityId);
                }
            }

            return entitiesIdList;
        }

        public event Action OnChanged;
    }
}
