using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalIndexStorage: ILogicalStorage
    {
        private readonly object mLockObj = new object();
        private Dictionary<ulong, LogicalIndexingFrame> mDataDict = new Dictionary<ulong, LogicalIndexingFrame>();
        private Dictionary<ulong, IReadOnlyLogicalObject> mObjectsDict = new Dictionary<ulong, IReadOnlyLogicalObject>();
        
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

        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
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

        public IList<ulong> GetEntitiesIdListByAST(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage GetEntitiesIdListByAST queryNode = {queryNode}");
#endif

            var plan = QueryResolverASTNodeFactory.CreatePlan(queryNode);

            return plan.GetEntitiesIdList(this);
        }
    }
}
