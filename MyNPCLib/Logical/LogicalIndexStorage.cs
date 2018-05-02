using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalIndexStorage: ILogicalStorage
    {
        private readonly object mLockObj = new object();
        private Dictionary<ulong, LogicalIndexingFrame> mDataDict = new Dictionary<ulong, LogicalIndexingFrame>();

        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            lock(mLockObj)
            {
                LogicalIndexingFrame indexingFrame = null;

                if (mDataDict.ContainsKey(propertyId))
                {
                    indexingFrame = mDataDict[propertyId];
                }
                else
                {
                    indexingFrame = new LogicalIndexingFrame(propertyId);
                    mDataDict[propertyId] = indexingFrame;
                }

                indexingFrame[value] = entityId;
            }
        }

        public List<ulong> GetEntitiesIdListByAST(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage GetEntitiesIdListByAST queryNode = {queryNode}");
#endif

            return new List<ulong>();//tmp
        }
    }
}
