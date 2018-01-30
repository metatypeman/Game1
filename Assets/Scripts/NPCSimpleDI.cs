using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class NPCSimpleDI
    {
        private object mLockObj = new object();
        private Dictionary<Type, object> mInstancesDict = new Dictionary<Type, object>();

        public void RegisterInstance<T>(object instance) where T : class
        {
            if(instance == null)
            {
                return;
            }

            lock(mLockObj)
            {
                var type = typeof(T);

                mInstancesDict[type] = instance;
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

        public T GetInstance<T>() where T: class
        {
            var type = typeof(T);

            lock (mLockObj)
            {
                if(mInstancesDict.ContainsKey(type))
                {
                    return mInstancesDict[type] as T;
                }

                return null;
            }
        }
    }
}
