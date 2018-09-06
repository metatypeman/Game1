using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.Logical
{
    public delegate void UpdateVisibleEntitiesIdListAction(IList<ulong> visibleEntitiesId, IList<ulong> oldVisibleEntitiesId);

    public class StorageOfSpecialEntities
    {
        private object mLockObj = new object();
        public ulong SelfEntityId { get; set; }
        public event UpdateVisibleEntitiesIdListAction OnUpdateVisibleEntitiesIdList;

        public void SetVisibleEntitiesId(IList<ulong> visibleEntitiesId)
        {
            lock(mLockObj)
            {
                var differenceList = mVisibleEntitiesIdList.Where(p => !visibleEntitiesId.Contains(p)).ToList();

                if(differenceList.Count == 0)
                {
                    return;
                }

                var newEntitiesIdList = visibleEntitiesId.ToList();
                var oldEntitiesIdList = mVisibleEntitiesIdList.ToList();

                Task.Run(() => {
                    OnUpdateVisibleEntitiesIdList?.Invoke(newEntitiesIdList, oldEntitiesIdList);
                });

                mVisibleEntitiesIdList = visibleEntitiesId;
            }
        }

        private IList<ulong> mVisibleEntitiesIdList = new List<ulong>();

        public bool IsVisible(ulong entityId)
        {
            lock (mLockObj)
            {
                if(mVisibleEntitiesIdList.Contains(entityId))
                {
                    return true;
                }

                return false;
            }
        }

        public IList<ulong> GetVisibleEntitiesId()
        {
            lock (mLockObj)
            {
                return mVisibleEntitiesIdList.ToList();
            }
        }
    }
}
