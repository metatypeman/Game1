using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class LogicalObjectsBus: ILogicalStorage
    {
        public LogicalObjectsBus()
        {
            mLogicalIndexStorage = new LogicalIndexStorage();
            mLogicalIndexStorage.OnChanged += MLogicalIndexStorage_OnChanged;
        }

        private void MLogicalIndexStorage_OnChanged()
        {
            Task.Run(() => {
                try
                {
                    OnChanged?.Invoke();
                }
                catch (Exception e)
                {
#if DEBUG
                    LogInstance.Log($"LogicalObjectsBus MLogicalIndexStorage_OnChanged e = {e}");
#endif
                }
            });          
        }

        private LogicalIndexStorage mLogicalIndexStorage;
        private readonly object mLockObj = new object();
        private Dictionary<int, ulong> mInstanceIdToEntityIdDict = new Dictionary<int, ulong>();
        private Dictionary<ulong, int> mEntityIdToInstanceIdDict = new Dictionary<ulong, int>();

        public event Action OnChanged;

        public void RegisterObject(int instanceId, IReadOnlyLogicalObject value)
        {
            lock(mLockObj)
            {
                var entityId = value.EntityId;
                mInstanceIdToEntityIdDict[instanceId] = entityId;
                mEntityIdToInstanceIdDict[entityId] = instanceId;

                mLogicalIndexStorage.RegisterObject(value);
            }
        }

        public ulong GetEntityIdByInstanceId(int instanceId)
        {
            lock (mLockObj)
            {
                if(mInstanceIdToEntityIdDict.ContainsKey(instanceId))
                {
                    return mInstanceIdToEntityIdDict[instanceId];
                }

                return 0ul;
            }
        }

        public IDictionary<int, ulong> GetEntitiesIdListByInstancesIdList(IList<int> instancesIdList)
        {
            if(instancesIdList.IsEmpty())
            {
                return new Dictionary<int, ulong>();
            }

            lock (mLockObj)
            {
                return mInstanceIdToEntityIdDict.Where(p => instancesIdList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
            return mLogicalIndexStorage.GetAllEntitiesIdsList();
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
            return mLogicalIndexStorage.GetEntitiesIdList(plan);
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
            return mLogicalIndexStorage.GetEntitiesIdsList(propertyId, value);
        }

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
            return mLogicalIndexStorage.GetPropertyValue(entityId, propertyId);
        }

        public void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value)
        {
            mLogicalIndexStorage.PutPropertyValueAsIndex(entityId, propertyId, value);
        }

        public void PutAccessPolicyToFactAsIndex(ulong entityId, ulong propertyId, AccessPolicyToFact value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalObjectsBus PutAccessPolicyToFactAsIndex entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            mLogicalIndexStorage.PutAccessPolicyToFactAsIndex(entityId, propertyId, value);
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //LogInstance.Log($"LogicalObjectsBus GetAccessPolicyToFact entityId = {entityId} propertyId = {propertyId}");
#endif

            return mLogicalIndexStorage.GetAccessPolicyToFact(entityId, propertyId);
        }

        public void SetPropertyValue(ulong entityId, ulong propertyId, object value)
        {
            mLogicalIndexStorage.SetPropertyValue(entityId, propertyId, value);
        }
    }
}
