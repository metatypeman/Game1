using MyNPCLib.CGStorage;
using MyNPCLib.LogicalHostEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public class VisionObjectsStorage: IDisposable
    {
        public VisionObjectsStorage(IEntityLogger entityLogger, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, SystemPropertiesDictionary systemPropertiesDictionary, StorageOfSpecialEntities storageOfSpecialEntities)
        {
            mEntityLogger = entityLogger;
            mNPCHostContext = npcHostContext;
            mEntityDictionary = entityDictionary;
            mSystemPropertiesDictionary = systemPropertiesDictionary;
            mStorageOfSpecialEntities = storageOfSpecialEntities;

            Task.Run(() => { ThUpdateVisibleEntitiesIdList(); });
        }

        private IEntityLogger mEntityLogger;
        private readonly INPCHostContext mNPCHostContext;
        private readonly IEntityDictionary mEntityDictionary;
        private IBusOfCGStorages mBusOfHostStorage;
        private ICGStorage mLogicalStorage;
        private readonly SystemPropertiesDictionary mSystemPropertiesDictionary;
        private readonly object mLockObj = new object();
        private Dictionary<ulong, VisionObject> mVisibleObjectsDict = new Dictionary<ulong, VisionObject>();
        private Dictionary<ulong, VisionObjectImpl> mVisibleObjectsImplDict = new Dictionary<ulong, VisionObjectImpl>();
        private StorageOfSpecialEntities mStorageOfSpecialEntities;

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

        public IBusOfCGStorages BusOfHostStorage
        {
            set
            {
                mBusOfHostStorage = value;
            }
        }

        public ICGStorage LogicalStorage
        {
            set
            {
                mLogicalStorage = value;
            }
        }

        public IList<VisionObject> VisibleObjects
        {
            get
            {
                var hostVisibleObjectsList = mNPCHostContext.VisibleObjects;

#if DEBUG
                //Log($"hostVisibleObjectsList.Count = {hostVisibleObjectsList?.Count}");
#endif
                var result = new List<VisionObject>();
                
                lock (mLockObj)
                {
                    if (hostVisibleObjectsList.IsEmpty())
                    {
                        foreach (var item in mVisibleObjectsImplDict)
                        {
                            item.Value.VisionItems = new List<IVisionItem>();
                        }

                        mStorageOfSpecialEntities.SetVisibleEntitiesId(new List<ulong>());

                        return result;
                    }

                    var visibleEntitiesIdList = new List<ulong>();

                    foreach (var hostVisibleObject in hostVisibleObjectsList)
                    {
#if DEBUG
                        //Log($"hostVisibleObject = {hostVisibleObject}");
#endif

                        var entityId = hostVisibleObject.EntityId;

                        visibleEntitiesIdList.Add(entityId);

                        VisionObject item = null;

                        if (mVisibleObjectsDict.ContainsKey(entityId))
                        {
                            item = mVisibleObjectsDict[entityId];

                            var impl = mVisibleObjectsImplDict[entityId];

                            impl.VisionItems = hostVisibleObject.VisionItems;
                        }
                        else
                        {
                            var impl = new VisionObjectImpl(entityId, hostVisibleObject.VisionItems);
                            item = new VisionObject(mEntityLogger, entityId, impl, mEntityDictionary, mLogicalStorage, mSystemPropertiesDictionary);
                            mVisibleObjectsDict[entityId] = item;
                            mVisibleObjectsImplDict[entityId] = impl;
                        }

                        result.Add(item);
                    }

                    mStorageOfSpecialEntities.SetVisibleEntitiesId(visibleEntitiesIdList);

                    foreach (var item in mVisibleObjectsImplDict)
                    {
                        if (visibleEntitiesIdList.Contains(item.Key))
                        {
                            continue;
                        }

                        item.Value.VisionItems = new List<IVisionItem>();
                    }
                }

                return result;
            }
        }

        private void UpdateVisibleEntitiesIdList()
        {
            var hostVisibleObjectsList = mNPCHostContext.VisibleObjects;

            if (hostVisibleObjectsList.IsEmpty())
            {
                mStorageOfSpecialEntities.SetVisibleEntitiesId(new List<ulong>());
                return;
            }

            var visibleEntitiesIdList = new List<ulong>();

            foreach (var hostVisibleObject in hostVisibleObjectsList)
            {
#if DEBUG
                //Log($"hostVisibleObject = {hostVisibleObject}");
#endif

                var entityId = hostVisibleObject.EntityId;

                visibleEntitiesIdList.Add(entityId);
            }

            mStorageOfSpecialEntities.SetVisibleEntitiesId(visibleEntitiesIdList);
            return;
        }

        public VisionObjectImpl GetVisionObjectImpl(ulong entityId)
        {
            lock (mLockObj)
            {
                if(mVisibleObjectsImplDict.ContainsKey(entityId))
                {
                    return mVisibleObjectsImplDict[entityId];
                }

                return null;
            }
        }

        public Dictionary<ulong, VisionObjectImpl> GetVisionObjectsImplDict(IList<ulong> entitiesIdList)
        {
            lock (mLockObj)
            {
                return mVisibleObjectsImplDict.Where(p => p.Value.IsVisible && entitiesIdList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }

        private void ThUpdateVisibleEntitiesIdList()
        {
            while(true)
            {
                lock (mNeedStopLockObj)
                {
                    if(mNeedStop)
                    {
                        return;
                    }
                }

                UpdateVisibleEntitiesIdList();

                Thread.Sleep(500);
            }
        }

        private volatile bool mNeedStop;
        private readonly object mNeedStopLockObj = new object();

        public void Dispose()
        {
            lock(mNeedStopLockObj)
            {
                mNeedStop = true;
            }
        }
    }
}
