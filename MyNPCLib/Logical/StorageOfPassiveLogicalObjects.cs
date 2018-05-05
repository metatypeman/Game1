using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class StorageOfPassiveLogicalObjects
    {
        private readonly object mLockObj = new object();
        private Dictionary<ulong, LogicalFrame> mObjectsDict = new Dictionary<ulong, LogicalFrame>();

        public void SetPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"StorageOfPassiveLogicalObjects StorageOfPassiveLogicalObjects entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
            lock (mLockObj)
            {
                LogicalFrame logicalObject = null;

                if (mObjectsDict.ContainsKey(entityId))
                {
                    logicalObject = mObjectsDict[entityId];
                }
                else
                {
                    logicalObject = new LogicalFrame(entityId);
                    mObjectsDict[entityId] = logicalObject;

                }

                logicalObject[propertyId] = value;
            }        
        }

        public void SetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"StorageOfPassiveLogicalObjects SetPropertyValue entitiesIdsList.Count = {entitiesIdsList.Count} propertyId = {propertyId} value = {value}");
            foreach (var entityId in entitiesIdsList)
            {
                LogInstance.Log($"StorageOfPassiveLogicalObjects entityId = {entityId}");
            }
#endif

            foreach (var entityId in entitiesIdsList)
            {
#if DEBUG
                LogInstance.Log($"StorageOfPassiveLogicalObjects entityId = {entityId}");
#endif

                SetPropertyValue(entityId, propertyId, value);
            }
        }

        public object GetPropertyValue(ulong entityId, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"StorageOfPassiveLogicalObjects GetPropertyValue entityId = {entityId} propertyId = {propertyId}");
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
            LogInstance.Log($"StorageOfPassiveLogicalObjects GetPropertyValue entitiesIdsList.Count = {entitiesIdsList.Count} propertyId = {propertyId}");
            foreach (var entityId in entitiesIdsList)
            {
                LogInstance.Log($"StorageOfPassiveLogicalObjects GetPropertyValue entityId = {entityId}");
            }
#endif

            List<LogicalFrame> targetLogicalObjects = null;

            lock (mLockObj)
            {

                targetLogicalObjects = mObjectsDict.Where(p => entitiesIdsList.Contains(p.Key)).Select(p => p.Value).ToList();
            }

            foreach (var logicalObject in targetLogicalObjects)
            {
                var value = logicalObject[propertyId];

                if (value == null)
                {
                    continue;
                }

                return value;
            }

            return null;
        }
    }
}
