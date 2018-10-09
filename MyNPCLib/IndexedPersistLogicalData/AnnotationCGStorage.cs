using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class AnnotationCGStorage: BaseProxyStorage
    {
        public AnnotationCGStorage(ICGStorage parentStorage, IndexedLogicalAnnotation annotationOfStored)
            : base(parentStorage?.EntityDictionary)
        {
            mParentStorage = parentStorage;
            mIndexedLogicalAnnotation = annotationOfStored;
        }

        private ICGStorage mParentStorage;
        private IndexedLogicalAnnotation mIndexedLogicalAnnotation;

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.OtherProxy;

        public override IList<RuleInstance> AllRuleInstances => mParentStorage.AllRuleInstances;

        public override IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
            //LogInstance.Log($"Context.EntityDictionary.GetName(key) = {Context.EntityDictionary.GetName(key)}");
#endif

            var indexedRuleInstance = mParentStorage.GetIndexedRuleInstanceByKey(mIndexedLogicalAnnotation.RuleInstanceKey);

            return indexedRuleInstance?.GetIndexedRulePartOfFactsByKeyOfRelation(key);
        }

        public override IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
            //throw new NotImplementedException();
#endif

            return mParentStorage.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(key);

            //var result = new List<IndexedRulePart>();

            //var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = Context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList;

            //foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityAndUseProductionsList)
            //{
            //    var indexedRulePartWithOneRelationsList = dataSourcesSettings.Storage.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(key);

            //    if (indexedRulePartWithOneRelationsList == null)
            //    {
            //        continue;
            //    }

            //    result.AddRange(indexedRulePartWithOneRelationsList);
            //}

            //return result;
        }

        public override RuleInstance GetRuleInstanceByKey(ulong key)
        {
            return mParentStorage.GetRuleInstanceByKey(key);
        }

        public override IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
#if DEBUG
            LogInstance.Log($"key = {key}");
#endif

            return mParentStorage.GetIndexedRuleInstanceByKey(key);
        }

        public override IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            return mParentStorage.GetIndexedAdditionalRuleInstanceByKey(key);
        }

        public override IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            return mParentStorage.GetAllRelations();
        }

        public override IList<ResolverForRelationExpressionNode> AllRelationsForProductions => mParentStorage.AllRelationsForProductions;
    }
}
