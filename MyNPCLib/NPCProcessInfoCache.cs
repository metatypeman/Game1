using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessInfoCache
    {
        private object mLockObj = new object();
        private Dictionary<Type, NPCProcessInfo> mDict = new Dictionary<Type, NPCProcessInfo>();

        public NPCProcessInfo Get(Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            lock(mLockObj)
            {
                if(mDict.ContainsKey(type))
                {
                    return mDict[type];
                }

                return null;
            }
        }

        public bool Set(NPCProcessInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            lock (mLockObj)
            {
                var type = info.Type;

                if(mDict.ContainsKey(type))
                {
                    return false;
                }

                mDict[type] = info;
                return true;
            }
        }
    }
}
