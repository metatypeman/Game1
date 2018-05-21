using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class StorageOfSpecialEntities
    {
        private object mLockObj = new object();
        public ulong SelfEntityId { get; set; }
        public void SetVisibleEntitiesId(IList<ulong> visibleEntitiesId)
        {
            lock(mLockObj)
            {
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
    }
}
