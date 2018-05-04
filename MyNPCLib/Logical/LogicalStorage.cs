using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalStorage : ILogicalStorage
    {
        public void PutPropertyValue(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalStorage PutPropertyValue entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            throw new NotImplementedException();
        }

        public IList<ulong> GetEntitiesIdsList(ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalStorage GetEntitiesIdsList propertyId = {propertyId} value = {value}");
#endif

            throw new NotImplementedException();
        }

        public IList<ulong> GetAllEntitiesIdsList()
        {
#if DEBUG
            LogInstance.Log("LogicalStorage GetAllEntitiesIdsList");
#endif

            throw new NotImplementedException();
        }

        public IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan)
        {
#if DEBUG
            LogInstance.Log("LogicalStorage GetEntitiesIdList");
#endif

            throw new NotImplementedException();
        }

        public event Action OnChanged;
    }
}
