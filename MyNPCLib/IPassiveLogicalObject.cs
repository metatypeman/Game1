using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IPassiveLogicalObject
    {
        ulong EntityId { get; }
        object this[ulong propertyKey] { get; set; }
        object this[string propertyName] { get; set; }
    }
}
