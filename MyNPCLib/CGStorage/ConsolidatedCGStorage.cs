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
            var dataSourcesSettingsOrderedByPriorityList = settings.OrderBy(p => p.Priority).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseFactsList = dataSourcesSettingsOrderedByPriorityList.Where(p => p.UseFacts).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = dataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Consolidated;

        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseFactsList { get; set; }
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseProductionsList { get; set; }

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
