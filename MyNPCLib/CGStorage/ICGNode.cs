using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public interface ICGNode: IObjectToString
    {
        KindOfCGNode Kind { get;  }
        string Name { get; set; }
        string ToStringAsShortBrief(uint n);
    }
}
