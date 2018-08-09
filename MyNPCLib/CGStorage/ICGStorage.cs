using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public interface ICGStorage : IObjectToString
    {
        KindOfCGStorage KindOfStorage { get; }
        IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key);
        IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key);
        RuleInstance GeyRuleInstanceByKey(ulong key);
        IList<ResolverForRelationExpressionNode> GetAllRelations();
    }
}
