using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InternalLogicalObjectsBus: ILogicalStorage
    {
        private readonly object mLockObj = new object();
        private Dictionary<int, IReadOnlyLogicalObject> mObjectsDict = new Dictionary<int, IReadOnlyLogicalObject>();

        public void RegisterObject(int instanceId, IReadOnlyLogicalObject value)
        {
            lock (mLockObj)
            {
                mObjectsDict[instanceId] = value;
            }
        }

        public IReadOnlyLogicalObject GetObjectByInstanceId(int instanceId)
        {
            lock (mLockObj)
            {
                if(mObjectsDict.ContainsKey(instanceId))
                {
                    return mObjectsDict[instanceId];
                }

                return null;
            }
        }

        public Dictionary<int, IReadOnlyLogicalObject> GetObjectsByInstancesId(List<int> instancesIdsList)
        {
            if(instancesIdsList.IsEmpty())
            {
                return new Dictionary<int, IReadOnlyLogicalObject>();
            }

            lock (mLockObj)
            {
                return mObjectsDict.Where(p => instancesIdsList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"InternalLogicalObjectsBus PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
        }
    }
}
