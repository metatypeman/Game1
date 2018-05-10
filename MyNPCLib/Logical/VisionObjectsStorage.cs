using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObjectsStorage
    {
        public VisionObjectsStorage(IEntityDictionary entityDictionary, INPCHostContext npcHostContext)
        {
            mNPCHostContext = npcHostContext;
            mEntityDictionary = entityDictionary;
        }

        private INPCHostContext mNPCHostContext;
        private readonly IEntityDictionary mEntityDictionary;
        private ILogicalStorage mLogicalStorage;
        private readonly object mLockObj = new object();
        private Dictionary<ulong, VisionObject> mVisibleObjectsDict = new Dictionary<ulong, VisionObject>();

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
                LogInstance.Log($"VisionObjectsStorage VisibleObjects hostVisibleObjectsList.Count = {hostVisibleObjectsList?.Count}");
#endif
                var result = new List<VisionObject>();

                lock (mLockObj)
                {
                    if (hostVisibleObjectsList.IsEmpty())
                    {
                        foreach (var item in mVisibleObjectsDict)
                        {
                            item.Value.VisionItems = new List<IVisionItem>();
                        }

                        return result;
                    }

                    var visibleEntitiesIdList = new List<ulong>();

                    foreach (var hostVisibleObject in hostVisibleObjectsList)
                    {
#if DEBUG
                        LogInstance.Log($"VisionObjectsStorage VisibleObjects hostVisibleObject = {hostVisibleObject}");
#endif

                        var entityId = hostVisibleObject.EntityId;

                        visibleEntitiesIdList.Add(entityId);

                        VisionObject item = null;

                        if (mVisibleObjectsDict.ContainsKey(entityId))
                        {
                            item = mVisibleObjectsDict[entityId];
                            item.VisionItems = hostVisibleObject.VisionItems;
                        }
                        else
                        {
                            item = new VisionObject(hostVisibleObject.EntityId, hostVisibleObject.VisionItems, mEntityDictionary, mLogicalStorage);
                            mVisibleObjectsDict[entityId] = item;
                        }

                        result.Add(item);
                    }

                    foreach (var item in mVisibleObjectsDict)
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
    }
}
