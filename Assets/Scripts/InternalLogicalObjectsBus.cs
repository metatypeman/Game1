using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InternalLogicalObjectsBus: ILogicalIndexingBus
    {
        private readonly object mLockObj = new object();
        private Dictionary<int, IInternalLogicalObject> mObjectsDict = new Dictionary<int, IInternalLogicalObject>();

        public void RegisterObject(int instanceId, IInternalLogicalObject value)
        {
            lock (mLockObj)
            {
                mObjectsDict[instanceId] = value;
            }
        }

        public IInternalLogicalObject GetObject(int instanceId)
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

        public Dictionary<int, IInternalLogicalObject> GetObjects(List<int> instancesIdsList)
        {
            if(instancesIdsList.IsEmpty())
            {
                return new Dictionary<int, IInternalLogicalObject>();
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
