using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public class LogicalStorage : IOldLogicalStorage
    {
        public LogicalStorage(IEntityLogger entityLogger, IEntityDictionary entityDictionary, IOldLogicalStorage hostLogicalStorage, StorageOfSpecialEntities storageOfSpecialEntities)
        {
            mEntityLogger = entityLogger;
            
#if DEBUG
            //Log("1");
#endif
            mStorageOfSpecialEntities = storageOfSpecialEntities;
            mHostLogicalStorage = hostLogicalStorage;

#if DEBUG
            //Log("2");
#endif

            mEntityDictionary = entityDictionary;

#if DEBUG
            //Log("3");
#endif

            mStorageOfPassiveLogicalObjects = new StorageOfPassiveLogicalObjects(entityLogger);

#if DEBUG
            //Log("4");
#endif

            mLogicalIndexStorage = new LogicalIndexStorage(entityLogger, mStorageOfSpecialEntities);

#if DEBUG
            //Log("5");
#endif
            mLogicalIndexStorage.OnChanged += MHostLogicalStorage_OnChanged;

#if DEBUG
            //Log("6");
            //Log($"(mHostLogicalStorage == null) = {mHostLogicalStorage == null}");
#endif

            mHostLogicalStorage.OnChanged += MHostLogicalStorage_OnChanged;

#if DEBUG
            //Log("7");
#endif
        }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        private void MHostLogicalStorage_OnChanged()
        {
#if DEBUG
            //Log("Begin");
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
                    Error($"e = {e}");
#endif
                }
            });
        }

        private IEntityLogger mEntityLogger;
        private IOldLogicalStorage mHostLogicalStorage;
        private IEntityDictionary mEntityDictionary;
        private StorageOfPassiveLogicalObjects mStorageOfPassiveLogicalObjects;
        private LogicalIndexStorage mLogicalIndexStorage;
        private StorageOfSpecialEntities mStorageOfSpecialEntities;

        public void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            //Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mLogicalIndexStorage.PutPropertyValueAsIndex(entityId, propertyId, value);
        }

        public void PutAccessPolicyToFactAsIndex(ulong entityId, ulong propertyId, AccessPolicyToFact value)
        {
#if DEBUG
            //Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mLogicalIndexStorage.PutAccessPolicyToFactAsIndex(entityId, propertyId, value);
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            return mHostLogicalStorage.GetAccessPolicyToFact(entityId, propertyId);
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            //Log($"propertyId = {propertyId} value = {value}");
#endif

            return mLogicalIndexStorage.GetEntitiesIdsList(propertyId, value);
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
#if DEBUG
            //Log("Begin");
#endif

            return mLogicalIndexStorage.GetAllEntitiesIdsList();
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
#if DEBUG
            //Log("Begin");
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
            //Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mStorageOfPassiveLogicalObjects.SetPropertyValue(entityId, propertyId, value);
        }

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //Log($"entityId = {entityId} propertyId = {propertyId}");
#endif
          
            var result = mStorageOfPassiveLogicalObjects.GetPropertyValue(entityId, propertyId);

            if(result == null)
            {
                var policy = mHostLogicalStorage.GetAccessPolicyToFact(entityId, propertyId);

                switch(policy)
                {
                    case AccessPolicyToFact.Public:
                        break;

                    case AccessPolicyToFact.ForVisible:
                        if(!mStorageOfSpecialEntities.IsVisible(entityId))
                        {
                            return null;
                        }
                        break;

                    case AccessPolicyToFact.Private:
                        if(entityId != mStorageOfSpecialEntities.SelfEntityId)
                        {
                            return null;
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
                }

                return mHostLogicalStorage.GetPropertyValue(entityId, propertyId);
            }

            return result;
        }
    }
}
