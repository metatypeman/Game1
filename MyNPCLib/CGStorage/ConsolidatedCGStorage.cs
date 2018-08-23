using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ConsolidatedCGStorage: BaseProxyStorage
    {
        public ConsolidatedCGStorage(ContextOfCGStorage context, IList<SettingsOfStorageForSearchingInThisSession> settings)
            : base(context)
        {
            mDataSourcesSettingsOrderedByPriorityList = settings.OrderBy(p => p.Priority).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseFactsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseFacts).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseAdditionalInstances).ToList();
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Consolidated;

        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityList;
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseFactsList { get; set; }
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseProductionsList { get; set; }
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances { get; set; }

        public override IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
#if DEBUG
            //LogInstance.Log($"key = {key}");
#endif

            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityAndUseFactsList;

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

        public override IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
            var dataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityAndUseFactsList;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
            {
                var indexedRuleInstance = dataSourcesSettings.Storage.GetIndexedRuleInstanceByKey(key);

                if (indexedRuleInstance == null)
                {
                    continue;
                }

                return indexedRuleInstance;
            }

            return null;
        }

        public override IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            var dataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
            {
                var indexedRuleInstance = dataSourcesSettings.Storage.GetIndexedAdditionalRuleInstanceByKey(key);

                if (indexedRuleInstance == null)
                {
                    continue;
                }

                return indexedRuleInstance;
            }

            return null;
        }

        public override IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityAndUseProductionsList;

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

        public override IList<ResolverForRelationExpressionNode> AllRelationsForProductions
        {
            get
            {
                var result = new List<ResolverForRelationExpressionNode>();

                var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityAndUseProductionsList;

                foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityAndUseProductionsList)
                {
                    var targetRelationsList = dataSourcesSettings.Storage.GetAllRelations();

                    if (targetRelationsList == null)
                    {
                        continue;
                    }

                    result.AddRange(targetRelationsList);
                }

                return result;
            }
        }
    }
}
