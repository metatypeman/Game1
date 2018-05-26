using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public interface ICGNode: IObjectToString
    {
        KindOfCGNode Kind { get;  }
        string Name { get; set; }
        IList<ICGNode> ChildrenNodes { get; }
        IList<ICGNode> InputNodes { get; }
        IList<ICGNode> OutputNodes { get; }

        string ToStringAsShortBrief(uint n);
    }
}
