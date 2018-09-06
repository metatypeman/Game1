using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    [Obsolete]
    public interface IOldLogicalStorage
    {
        void SetPropertyValue(ulong entityId, ulong propertyId, object value);
        object GetPropertyValue(ulong entityId, ulong propertyId);
        void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value);
        AccessPolicyToFact GetAccessPolicyToFact(ulong entityId, ulong propertyId);
        void PutAccessPolicyToFactAsIndex(ulong entityId, ulong propertyId, AccessPolicyToFact value);
        IList<ulong> GetEntitiesIdsList(ulong propertyId, object value);
        IList<ulong> GetAllEntitiesIdsList();
        IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan);
        event Action OnChanged;
    }
}
