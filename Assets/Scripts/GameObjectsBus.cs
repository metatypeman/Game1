using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib;

namespace Assets.Scripts
{
    public class GameObjectsBus
    {
        private readonly object mLockObj = new object();
        private Dictionary<int, GameObject> mObjectsDict = new Dictionary<int, GameObject>();

        public void RegisterObject(int instanceId, GameObject value)
        {
            lock (mLockObj)
            {
                mObjectsDict[instanceId] = value;
            }
        }

        public GameObject GetObject(int instanceId)
        {
            lock (mLockObj)
            {
                if (mObjectsDict.ContainsKey(instanceId))
                {
                    return mObjectsDict[instanceId];
                }

                return null;
            }
        }
    }
}
