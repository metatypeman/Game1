using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCSimpleDI
    {
        private object mLockObj = new object();
        private Dictionary<Type, object> mInstancesDict = new Dictionary<Type, object>();

        public void RegisterInstance<T>(object instance) where T : class
        {
            RegisterInstance(instance, typeof(T));
        }

        public void RegisterInstance(object instance, params Type[] types)
        {
            if (instance == null)
            {
                return;
            }

            lock (mLockObj)
            {
                foreach (var type in types)
                {
                    mInstancesDict[type] = instance;
                }
            }
        }

        public void RemoveInstance<T>() where T : class
        {
            lock (mLockObj)
            {
                var type = typeof(T);

                if (mInstancesDict.ContainsKey(type))
                {
                    mInstancesDict.Remove(type);
                }
            }
        }

        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);

            lock (mLockObj)
            {
                if (mInstancesDict.ContainsKey(type))
                {
                    return mInstancesDict[type] as T;
                }

                return null;
            }
        }
    }
}
