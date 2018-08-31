using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IEntityDictionary
    {
        string Name { get; }
        ulong GetKey(string name);
        string GetName(ulong key);
        KindOfKey GetKindOfKey(ulong key);
        bool IsEntity(ulong key);
    }
}
