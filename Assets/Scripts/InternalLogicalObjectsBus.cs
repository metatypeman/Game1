using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class InternalLogicalObjectsBus
    {
        private static readonly object mLockObj = new object();
        private static Dictionary<int, IInternalLogicalObject> mObjectsDict = new Dictionary<int, IInternalLogicalObject>();

        public static void RegisterObject(int instanceId, IInternalLogicalObject value)
        {
            lock (mLockObj)
            {
                mObjectsDict[instanceId] = value;
            }
        }

        public static IInternalLogicalObject GetObject(int instanceId)
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

        public static Dictionary<int, IInternalLogicalObject> GetObjects(List<int> instancesIdsList)
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
    }
}
