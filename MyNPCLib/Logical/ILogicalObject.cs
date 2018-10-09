using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface ILogicalObject
    {
        T GetValue<T>(ulong propertyKey);
        T GetValue<T>(string propertyName);
        object this[ulong propertyKey] { get; set; }
        object this[string propertyName] { get; set; }
    }
}
