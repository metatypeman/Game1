using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public interface ICGStorage : IObjectToString
    {
        KindOfCGStorage KindOfStorage { get; }
        IEntityDictionary EntityDictionary { get; }
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
        IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(string varName);
        IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(ulong keyOfVar);
        IList<BaseVariant> GetResultsListOfVarAsVariant(string varName);
        IList<BaseVariant> GetResultsListOfVarAsVariant(ulong keyOfVar);
        IList<object> GetResultsListOfVarAsObject(string varName);
        IList<object> GetResultsListOfVarAsObject(ulong keyOfVar);
        ResultOfVarOfQueryToRelation GetResultOfVar(string varName);
        ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar);
        BaseVariant GetResultOfVarAsVariant(string varName);
        BaseVariant GetResultOfVarAsVariant(ulong keyOfVar);
        object GetResultOfVarAsObject(string varName);
        object GetResultOfVarAsObject(ulong keyOfVar);
        event Action OnChanged;
        IList<ulong> GetEntitiesIdList(ICGStorage query);
        ICGStorage Search(LogicalSearchOptions options);
        ICGStorage Search(ICGStorage query);
        BaseVariant GetPropertyValueAsVariant(ulong entityId, ulong propertyId);
        BaseVariant GetPropertyValueAsVariant(ulong entityId, string propertyName);
        object GetPropertyValueAsObject(ulong entityId, ulong propertyId);
        object GetPropertyValueAsObject(ulong entityId, string propertyName);
        void SetPropertyValueAsAsVariant(ulong entityId, ulong propertyId, BaseVariant value);
        void SetPropertyValueAsAsVariant(ulong entityId, string propertyName, BaseVariant value);
        void SetPropertyValueAsAsObject(ulong entityId, ulong propertyId, object value);
        void SetPropertyValueAsAsObject(ulong entityId, string propertyName, object value);
    }
}
