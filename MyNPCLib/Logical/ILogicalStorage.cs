using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalStorage
    {
        void SetPropertyValue(ulong entityId, ulong propertyId, object value);
        void SetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId, object value);
        object GetPropertyValue(ulong entityId, ulong propertyId);
        object GetPropertyValue(IList<ulong> entitiesIdsList, ulong propertyId);
        void PutPropertyValueAsIndex(ulong entityId, ulong propertyId, object value);
        IList<ulong> GetEntitiesIdsList(ulong propertyId, object value);
        IList<ulong> GetAllEntitiesIdsList();
        IList<ulong> GetEntitiesIdList(BaseQueryResolverASTNode plan);
        event Action OnChanged;
        IReadOnlyLogicalObject GetObjectByInstanceId(int instanceId);
        IDictionary<int, IReadOnlyLogicalObject> GetObjectsByInstancesId(IList<int> instancesIdsList);
    }
}
