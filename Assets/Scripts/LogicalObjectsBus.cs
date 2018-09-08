using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class OldLogicalObjectsBus
    {
        public OldLogicalObjectsBus()
        {
        }

        private void MLogicalIndexStorage_OnChanged()
        {
            Task.Run(() => {
                try
                {
                    OnChanged?.Invoke();
                }
                catch (Exception e)
                {
#if DEBUG
                    LogInstance.Error($"e = {e}");
#endif
                }
            });          
        }

        private readonly object mLockObj = new object();
        private Dictionary<int, ulong> mInstanceIdToEntityIdDict = new Dictionary<int, ulong>();
        private Dictionary<ulong, int> mEntityIdToInstanceIdDict = new Dictionary<ulong, int>();

        public event Action OnChanged;

        public void RegisterObject(int instanceId, ulong entityId)
        {
            lock(mLockObj)
            {
                mInstanceIdToEntityIdDict[instanceId] = entityId;
                mEntityIdToInstanceIdDict[entityId] = instanceId;
            }
        }

        public ulong GetEntityIdByInstanceId(int instanceId)
        {
            lock (mLockObj)
            {
                if(mInstanceIdToEntityIdDict.ContainsKey(instanceId))
                {
                    return mInstanceIdToEntityIdDict[instanceId];
                }

                return 0ul;
            }
        }

        public IDictionary<int, ulong> GetEntitiesIdListByInstancesIdList(IList<int> instancesIdList)
        {
            if(instancesIdList.IsEmpty())
            {
                return new Dictionary<int, ulong>();
            }

            lock (mLockObj)
            {
                return mInstanceIdToEntityIdDict.Where(p => instancesIdList.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }
        }
    }
}
