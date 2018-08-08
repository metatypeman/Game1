using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class StrategyForGettingInfoFromStoragesByLogicalSearchContext: IStrategyForGettingInfoFromStorages
    {
        public StrategyForGettingInfoFromStoragesByLogicalSearchContext(LogicalSearchContext context)
        {
            Context = context;
        }

        public LogicalSearchContext Context { get; private set; }

        public IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
#endif

            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityList = Context.DataSourcesSettingsOrderedByPriorityAndUseFactsList;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
            {
                var indexedRulePartsOfFactsList = dataSourcesSettings.Storage.GetIndexedRulePartOfFactsByKeyOfRelation(key);

                if (indexedRulePartsOfFactsList == null)
                {
                    continue;
                }

                result.AddRange(indexedRulePartsOfFactsList);
            }

            return result;
        }

        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
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
