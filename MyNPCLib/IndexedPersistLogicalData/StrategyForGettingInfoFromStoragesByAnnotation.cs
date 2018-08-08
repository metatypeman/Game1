using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class StrategyForGettingInfoFromStoragesByAnnotation : IStrategyForGettingInfoFromStorages
    {
        public StrategyForGettingInfoFromStoragesByAnnotation(LogicalSearchContext context, IndexedLogicalAnnotation annotationOfStored)
        {
            Context = context;
            mIndexedLogicalAnnotation = annotationOfStored;
        }

        public LogicalSearchContext Context { get; private set; }
        private IndexedLogicalAnnotation mIndexedLogicalAnnotation;

        public IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
            //LogInstance.Log($"Context.EntityDictionary.GetName(key) = {Context.EntityDictionary.GetName(key)}");
#endif

            return mIndexedLogicalAnnotation.RuleInstance.GetIndexedRulePartOfFactsByKeyOfRelation(key);
        }

        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
            //throw new NotImplementedException();
#endif

            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = Context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityAndUseProductionsList)
            {
                var indexedRulePartWithOneRelationsList = dataSourcesSettings.Storage.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(key);

                if (indexedRulePartWithOneRelationsList == null)
                {
                    continue;
                }

                result.AddRange(indexedRulePartWithOneRelationsList);
            }

            return result;
        }
    }
}
