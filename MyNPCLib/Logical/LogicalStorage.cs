using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public class LogicalStorage : ILogicalStorage
    {
        public LogicalStorage(IEntityDictionary entityDictionary, ILogicalStorage hostLogicalStorage)
        {
#if DEBUG
            //LogInstance.Log("LogicalStorage()1");
#endif

            mHostLogicalStorage = hostLogicalStorage;

#if DEBUG
            //LogInstance.Log("LogicalStorage()2");
#endif

            mEntityDictionary = entityDictionary;

#if DEBUG
            //LogInstance.Log("LogicalStorage()3");
#endif

            mStorageOfPassiveLogicalObjects = new StorageOfPassiveLogicalObjects();

#if DEBUG
            //LogInstance.Log("LogicalStorage()4");
#endif

            mLogicalIndexStorage = new LogicalIndexStorage();

#if DEBUG
            //LogInstance.Log("LogicalStorage()5");
#endif
            mLogicalIndexStorage.OnChanged += MHostLogicalStorage_OnChanged;

#if DEBUG
            //LogInstance.Log("LogicalStorage()6");
            //LogInstance.Log($"LogicalStorage() (mHostLogicalStorage == null) = {mHostLogicalStorage == null}");
#endif

            mHostLogicalStorage.OnChanged += MHostLogicalStorage_OnChanged;

#if DEBUG
            //LogInstance.Log("LogicalStorage()7");
#endif
        }

        private void MHostLogicalStorage_OnChanged()
        {
#if DEBUG
            //LogInstance.Log("LogicalStorage MHostLogicalStorage_OnChanged");
#endif
            Task.Run(() =>
            {
                try
                {
                    OnChanged?.Invoke();
                }
                catch (Exception e)
                {
#if DEBUG
                    LogInstance.Log($"LogicalStorage MHostLogicalStorage_OnChanged e = {e}");
#endif
                }
            });
        }

        private ILogicalStorage mHostLogicalStorage;
        private IEntityDictionary mEntityDictionary;
        private StorageOfPassiveLogicalObjects mStorageOfPassiveLogicalObjects;
        private LogicalIndexStorage mLogicalIndexStorage;

        public void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mLogicalIndexStorage.PutPropertyValueAsIndex(entityId, propertyId, value);
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage GetEntitiesIdsList propertyId = {propertyId} value = {value}");
#endif

            return mLogicalIndexStorage.GetEntitiesIdsList(propertyId, value);
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
#if DEBUG
            //LogInstance.Log("LogicalStorage GetAllEntitiesIdsList");
#endif

            return mLogicalIndexStorage.GetAllEntitiesIdsList();
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
#if DEBUG
            //LogInstance.Log("LogicalStorage GetEntitiesIdList");
#endif

            var entitiesIdList = plan.GetEntitiesIdList(mLogicalIndexStorage);

            var entitiesIdListOfHost = mHostLogicalStorage.GetEntitiesIdList(plan);

            if(entitiesIdListOfHost.Count > 0)
            {
                foreach(var entityId in entitiesIdListOfHost)
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

        public void SetPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage SetPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mStorageOfPassiveLogicalObjects.SetPropertyValue(entityId, propertyId, value);
        }

        public void SetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId, object value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage SetPropertyValue entitiesIdsList.Count = {entitiesIdsList.Count} propertyId = {propertyId} value = {value}");
            //foreach (var entityId in entitiesIdsList)
            //{
            //    LogInstance.Log($"LogicalStorage SetPropertyValue entityId = {entityId}");
            //}
#endif

            mStorageOfPassiveLogicalObjects.SetPropertyValue(entitiesIdsList, propertyId, value);
        }

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage GetPropertyValue entityId = {entityId} propertyId = {propertyId}");
#endif
          
            var result = mStorageOfPassiveLogicalObjects.GetPropertyValue(entityId, propertyId);

            if(result == null)
            {
                return mHostLogicalStorage.GetPropertyValue(entityId, propertyId);
            }

            return result;
        }

        public object GetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId)
        {
#if DEBUG
            //LogInstance.Log($"LogicalStorage GetPropertyValue entitiesIdsList.Count = {entitiesIdsList.Count} propertyId = {propertyId}");
            //foreach (var entityId in entitiesIdsList)
            //{
            //    LogInstance.Log($"LogicalStorage GetPropertyValue entityId = {entityId}");
            //}
#endif
            var result = mStorageOfPassiveLogicalObjects.GetPropertyValue(entitiesIdsList, propertyId);

            if(result == null)
            {
                return mHostLogicalStorage.GetPropertyValue(entitiesIdsList, propertyId);
            }

            return result;
        }
    }
}
