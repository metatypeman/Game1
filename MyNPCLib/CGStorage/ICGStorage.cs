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
        void Append(RuleInstance ruleInstance);
        void Append(ICGStorage storage);
        void Append(RuleInstancePackage ruleInstancePackage);
        IList<RuleInstance> AllRuleInstances { get; }
        IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key);
        IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key);
        RuleInstance GetRuleInstanceByKey(ulong key);
        IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key);
        IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key);
        IList<ResolverForRelationExpressionNode> GetAllRelations();
        IList<ResolverForRelationExpressionNode> AllRelationsForProductions { get; }
        RuleInstance MainRuleInstance { get; }
        IndexedRuleInstance MainIndexedRuleInstance { get; }
        ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar);
        event Action OnChanged;
        IList<ulong> GetEntitiesIdList(ICGStorage query);
        ICGStorage Search(ICGStorage query);
    }
}
