using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalIndexStorage: ILogicalIndexingBus
    {
        private readonly object mLockObj = new object();

        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalIndexStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
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
