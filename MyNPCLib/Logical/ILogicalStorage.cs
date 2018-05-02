using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalStorage
    {
        IReadOnlyLogicalObject GetObjectByEntityId(ulong entityId);
        IDictionary<ulong, IReadOnlyLogicalObject> GetObjectsByEntitiesIdList(IList<ulong> entitiesIdsList);
        void PutPropertyValue(ulong entityId, ulong propertyId, object value);
    }
}
