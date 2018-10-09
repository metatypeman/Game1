using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface IReadOnlyLogicalObject
    {
        ulong EntityId { get; }
        object this[ulong propertyKey] { get; }
    }
}
