using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public interface ICGStorage : IObjectToString
    {
        KindOfCGStorage Kind { get; }
        IList<IndexedRulePart> GetIndexedRulePartByKeyOfRelation(ulong key);
    }
}
