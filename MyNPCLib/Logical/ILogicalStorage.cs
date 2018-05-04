using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalStorage
    {
        void PutPropertyValue(ulong entityId, ulong propertyId, object value);
        IList<ulong> GetEntitiesIdsList(ulong propertyId, object value);
        IList<ulong> GetAllEntitiesIdsList();
        IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan);
        event Action OnChanged;
    }
}
