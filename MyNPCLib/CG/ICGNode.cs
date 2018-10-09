using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CG
{
    public interface ICGNode: IObjectToString, IShortObjectToString, IObjectToBriefString
    {
        KindOfCGNode Kind { get;  }
        string Name { get; set; }
        IList<ICGNode> ChildrenNodes { get; }
        IList<ICGNode> InputNodes { get; }
        IList<ICGNode> OutputNodes { get; }
    }
}
