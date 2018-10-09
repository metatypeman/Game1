using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public interface ILogicalyAnnotated
    {
        IList<LogicalAnnotation> Annotations { get; }
    }
}
