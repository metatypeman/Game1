using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public interface IIndexedLogicalyAnnotated
    {
        IList<IndexedLogicalAnnotation> Annotations { get; }
    }
}
