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
        ContextOfCGStorage Context { get; }
        IList<RuleInstance> AllRuleInstances { get; }
        IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key);
        IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key);
        RuleInstance GetRuleInstanceByKey(ulong key);
        IList<ResolverForRelationExpressionNode> GetAllRelations();
        IList<ResolverForRelationExpressionNode> AllRelationsForProductions { get; }
    }
}
