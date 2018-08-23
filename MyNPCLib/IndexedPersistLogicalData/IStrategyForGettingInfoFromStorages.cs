using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Obsolete]
    public interface IStrategyForGettingInfoFromStorages
    {
        [Obsolete]
        LogicalSearchContext Context { get; }
        [Obsolete]
        IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key);
        [Obsolete]
        IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key);
    }
}
