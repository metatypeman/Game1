using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IEntityDictionary
    {
        ulong GetKey(string name);
    }
}
