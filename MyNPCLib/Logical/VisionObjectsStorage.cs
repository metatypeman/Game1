using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObjectsStorage
    {
        public VisionObjectsStorage(IEntityDictionary entityDictionary, INPCHostContext npcHostContext, SystemPropertiesDictionary systemPropertiesDictionary)
        {
            mNPCHostContext = npcHostContext;
            mEntityDictionary = entityDictionary;
            mSystemPropertiesDictionary = systemPropertiesDictionary;
        }

        private readonly INPCHostContext mNPCHostContext;
        private readonly IEntityDictionary mEntityDictionary;
        private ILogicalStorage mLogicalStorage;
        private readonly SystemPropertiesDictionary mSystemPropertiesDictionary;
        private readonly object mLockObj = new object();
        private Dictionary<ulong, VisionObject> mVisibleObjectsDict = new Dictionary<ulong, VisionObject>();
        private Dictionary<ulong, VisionObjectImpl> mVisibleObjectsImplDict = new Dictionary<ulong, VisionObjectImpl>();

        public ILogicalStorage LogicalStorage
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
                //LogInstance.Log($"VisionObjectsStorage VisibleObjects hostVisibleObjectsList.Count = {hostVisibleObjectsList?.Count}");
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

                        return result;
                    }

                    var visibleEntitiesIdList = new List<ulong>();

                    foreach (var hostVisibleObject in hostVisibleObjectsList)
                    {
#if DEBUG
                        //LogInstance.Log($"VisionObjectsStorage VisibleObjects hostVisibleObject = {hostVisibleObject}");
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
                            item = new VisionObject(entityId, impl, mEntityDictionary, mLogicalStorage, mSystemPropertiesDictionary);
                            mVisibleObjectsDict[entityId] = item;
                            mVisibleObjectsImplDict[entityId] = impl;
                        }

                        result.Add(item);
                    }

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
                return mVisibleObjectsImplDict.Where(p => entitiesIdList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }
    }
}
