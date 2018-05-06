using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public class LogicalIndexStorage: ILogicalStorage
    {
        private readonly object mLockObj = new object();
        private Dictionary<ulong, LogicalIndexingFrame> mDataDict = new Dictionary<ulong, LogicalIndexingFrame>();
        private Dictionary<ulong, IReadOnlyLogicalObject> mObjectsDict = new Dictionary<ulong, IReadOnlyLogicalObject>();
        private Dictionary<int, IReadOnlyLogicalObject> mObjectsByInstanceIdAsKeyDict = new Dictionary<int, IReadOnlyLogicalObject>();

        public event Action OnChanged;

        public void RegisterObject(ulong entityId, IReadOnlyLogicalObject value)
        {
            lock (mLockObj)
            {
                mObjectsDict[entityId] = value;
            }
        }

        public IReadOnlyLogicalObject GetObjectByEntityId(ulong entityId)
        {
            lock (mLockObj)
            {
                if(mObjectsDict.ContainsKey(entityId))
                {
                    return mObjectsDict[entityId];
                }

                return null;
            }
        }

        public IDictionary<ulong, IReadOnlyLogicalObject> GetObjectsByEntitiesIdList(IList<ulong> entitiesIdsList)
        {
            if (entitiesIdsList.IsEmpty())
            {
                return new Dictionary<ulong, IReadOnlyLogicalObject>();
            }

            lock (mLockObj)
            {
                return mObjectsDict.Where(p => entitiesIdsList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            lock(mLockObj)
            {
                LogicalIndexingFrame indexingFrame = null;

                if (mDataDict.ContainsKey(propertyId))
                {
                    indexingFrame = mDataDict[propertyId];
                }
                else
                {
                    indexingFrame = new LogicalIndexingFrame(propertyId);
                    mDataDict[propertyId] = indexingFrame;
                }

                indexingFrame.Set(value, entityId);

                Task.Run(() => {
                    OnChanged?.Invoke();
                });           
            }
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage GetEntitiesIdsList propertyId = {propertyId} value = {value}");
#endif

            lock (mLockObj)
            {
                if(mDataDict.ContainsKey(propertyId))
                {
                    var targetIndexingFrame = mDataDict[propertyId];
                    return targetIndexingFrame.Get(value);
                }

                return new List<ulong>();
            }            
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
            lock (mLockObj)
            {
                return mObjectsDict.Keys.ToList();
            }
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
            return plan.GetEntitiesIdList(this);
        }

        public void SetPropertyValue(ulong entityId, ulong propertyId, object value)
        {
        }

        public void SetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId, object value)
        {
        }

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage GetPropertyValue entityId = {entityId} propertyId = {propertyId}");
#endif

            lock (mLockObj)
            {
                if (mObjectsDict.ContainsKey(entityId))
                {
                    var logicalObject = mObjectsDict[entityId];
                    return logicalObject[propertyId];
                }

                return null;
            }             
        }

        public object GetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage GetPropertyValue entitiesIdsList.Count = {entitiesIdsList.Count} propertyId = {propertyId}");
            foreach (var entityId in entitiesIdsList)
            {
                LogInstance.Log($"LogicalIndexStorage GetPropertyValue entityId = {entityId}");
            }
#endif

            List<IReadOnlyLogicalObject> targetLogicalObjects = null;

            lock (mLockObj)
            {

                targetLogicalObjects = mObjectsDict.Where(p => entitiesIdsList.Contains(p.Key)).Select(p => p.Value).ToList();
            }

            foreach(var logicalObject in targetLogicalObjects)
            {
                var value = logicalObject[propertyId];

                if(value == null)
                {
                    continue;
                }

                return value;
            }

            return null;
        }

        public void RegisterObjectByInstanceId(int instanceId, IReadOnlyLogicalObject value)
        {
            lock (mLockObj)
            {
                mObjectsByInstanceIdAsKeyDict[instanceId] = value;
                var entityId = value.EntityId;
                mObjectsDict[entityId] = value;
            }
        }

        public IReadOnlyLogicalObject GetObjectByInstanceId(int instanceId)
        {
            lock (mLockObj)
            {
                if (mObjectsByInstanceIdAsKeyDict.ContainsKey(instanceId))
                {
                    return mObjectsByInstanceIdAsKeyDict[instanceId];
                }

                return null;
            }
        }

        public IDictionary<int, IReadOnlyLogicalObject> GetObjectsByInstancesId(IList<int> instancesIdsList)
        {
            if (instancesIdsList.IsEmpty())
            {
                return new Dictionary<int, IReadOnlyLogicalObject>();
            }

            lock (mLockObj)
            {
                return mObjectsByInstanceIdAsKeyDict.Where(p => instancesIdsList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }
    }
}
