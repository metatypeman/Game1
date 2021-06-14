//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts
//{
//    public class HandThingsBus
//    {
//        private readonly object mLockObj = new object();
//        private Dictionary<ulong, IHandThing> mHandThingsDict = new Dictionary<ulong, IHandThing>();

//        public void RegisterThing(ulong entityId, IHandThing value)
//        {
//            lock(mLockObj)
//            {
//                mHandThingsDict[entityId] = value;
//            }
//        }

//        public IHandThing GetThing(ulong entityId)
//        {
//            lock (mLockObj)
//            {
//                if(mHandThingsDict.ContainsKey(entityId))
//                {
//                    return mHandThingsDict[entityId];
//                }

//                return null;
//            }
//        }
//    }
//}
