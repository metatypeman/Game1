using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public class LogicalIndexStorage: IOldLogicalStorage
    {
        public LogicalIndexStorage(IEntityLogger entityLogger)
        {
            mEntityLogger = entityLogger;
        }

        public LogicalIndexStorage(IEntityLogger entityLogger, StorageOfSpecialEntities storageOfSpecialEntities)
        {
            mEntityLogger = entityLogger;
            mStorageOfSpecialEntities = storageOfSpecialEntities;
        }

        private IEntityLogger mEntityLogger;
        private readonly object mLockObj = new object();
        private Dictionary<ulong, LogicalIndexingFrame> mDataDict { get; set; } = new Dictionary<ulong, LogicalIndexingFrame>();
        private Dictionary<ulong, IReadOnlyLogicalObject> mObjectsDict { get; set; } = new Dictionary<ulong, IReadOnlyLogicalObject>();
        private StorageOfSpecialEntities mStorageOfSpecialEntities;

        public event Action OnChanged;

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

        public void RegisterObject(IReadOnlyLogicalObject value)
        {
            lock (mLockObj)
            {
                var entityId = value.EntityId;
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
            Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            lock(mLockObj)
            {
                var indexingFrame = NGetOrCreateLogicalIndexingFrameByPropertyId(propertyId);

                indexingFrame.Set(value, entityId);

                Task.Run(() => {
                    try
                    {
                        OnChanged?.Invoke();
                    }
                    catch(Exception e)
                    {
#if DEBUG
                        Error($"e = {e}");
#endif
                    }              
                });           
            }
        }

        public void PutAccessPolicyToFactAsIndex(ulong entityId, ulong propertyId, AccessPolicyToFact value)
        {
#if DEBUG
            Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
            lock(mLockObj)
            {
                var indexingFrame = NGetOrCreateLogicalIndexingFrameByPropertyId(propertyId);
                indexingFrame.SetAccessPolicyToFact(entityId, value);
            }
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong entityId, ulong propertyId)
        {
            lock (mLockObj)
            {
                if (mDataDict.ContainsKey(propertyId))
                {
                    var targetIndexingFrame = mDataDict[propertyId];
                    return targetIndexingFrame.GetAccessPolicyToFact(entityId);
                }

                return AccessPolicyToFact.Public;
            }
        }

        private LogicalIndexingFrame NGetOrCreateLogicalIndexingFrameByPropertyId(ulong propertyId)
        {
            if (mDataDict.ContainsKey(propertyId))
            {
                return mDataDict[propertyId];
            }

            var indexingFrame = new LogicalIndexingFrame(propertyId);
            mDataDict[propertyId] = indexingFrame;

            return indexingFrame;
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            Log($"propertyId = {propertyId} value = {value}");
#endif

            lock (mLockObj)
            {
                if(mDataDict.ContainsKey(propertyId))
                {
                    var targetIndexingFrame = mDataDict[propertyId];
                    var entitiesIdList = targetIndexingFrame.Get(value);

                    if(mStorageOfSpecialEntities == null)
                    {
                        return entitiesIdList;
                    }

                    var policiesDict = targetIndexingFrame.GetAccessPolicyToFact(entitiesIdList);

                    if(policiesDict.Count == 0)
                    {
                        return entitiesIdList;
                    }

                    var finalEntitiesIdList = new List<ulong>();

                    var visibleEntitiesIdList = mStorageOfSpecialEntities.GetVisibleEntitiesId();

                    foreach (var entityId in entitiesIdList)
                    {
                        if(policiesDict.ContainsKey(entityId))
                        {
                            var policy = policiesDict[entityId];

                            switch(policy)
                            {
                                case AccessPolicyToFact.Public:
                                    finalEntitiesIdList.Add(entityId);
                                    break;

                                case AccessPolicyToFact.ForVisible:
                                    if(visibleEntitiesIdList.Contains(entityId))
                                    {
                                        finalEntitiesIdList.Add(entityId);
                                    }
                                    break;

                                case AccessPolicyToFact.Private:
                                    if(entityId == mStorageOfSpecialEntities.SelfEntityId)
                                    {
                                        finalEntitiesIdList.Add(entityId);
                                    }
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
                            }
                            
                            continue;
                        }

                        finalEntitiesIdList.Add(entityId);
                    }

                    return finalEntitiesIdList;
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

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
#if DEBUG
            Log($"entityId = {entityId} propertyId = {propertyId}");
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
    }
}
