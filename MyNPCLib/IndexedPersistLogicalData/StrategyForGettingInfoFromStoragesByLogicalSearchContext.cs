using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Obsolete]
    public class StrategyForGettingInfoFromStoragesByLogicalSearchContext: IStrategyForGettingInfoFromStorages
    {
        public StrategyForGettingInfoFromStoragesByLogicalSearchContext(LogicalSearchContext context)
        {
            Context = context;
        }

        [Obsolete]
        public LogicalSearchContext Context { get; private set; }

        [Obsolete]
        public IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
#endif

            //var result = new List<IndexedRulePart>();

            //var dataSourcesSettingsOrderedByPriorityList = Context.DataSourcesSettingsOrderedByPriorityAndUseFactsList;

            //foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
            //{
            //    var indexedRulePartsOfFactsList = dataSourcesSettings.Storage.GetIndexedRulePartOfFactsByKeyOfRelation(key);

            //    if (indexedRulePartsOfFactsList == null)
            //    {
            //        continue;
            //    }

            //    result.AddRange(indexedRulePartsOfFactsList);
            //}

            //return result;

            throw new NotImplementedException();
        }

        [Obsolete]
        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            throw new NotImplementedException();
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
    }
}
