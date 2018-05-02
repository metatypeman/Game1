using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalStorage
    {
        void PutPropertyValue(ulong entityId, ulong propertyId, object value);
    }
}
