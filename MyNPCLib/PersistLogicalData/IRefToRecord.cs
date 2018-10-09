using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public interface IRefToRecord
    {
        string Name { get; set; }
        ulong Key { get; set; }
    }
}
