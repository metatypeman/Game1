using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalObject
    {
        object this[ulong propertyKey] { get; set; }
        object this[string propertyName] { get; set; }
    }
}
