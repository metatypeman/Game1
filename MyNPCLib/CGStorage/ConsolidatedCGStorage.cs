using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.CGStorage
{
    public class ConsolidatedCGStorage: BaseProxyStorage
    {
        public ConsolidatedCGStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
            mDataSourcesSettingsOrderedByPriorityList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseFactsList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = new List<SettingsOfStorageForSearchingInThisSession>();
        }

        public ConsolidatedCGStorage(IEntityDictionary entityDictionary, IList<SettingsOfStorageForSearchingInThisSession> settings)
            : base(entityDictionary)
        {
            mDataSourcesSettingsOrderedByPriorityList = settings.OrderByDescending(p => p.Priority).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseFactsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseFacts).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();
            mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseAdditionalInstances).ToList();

            foreach(var settingsOfStorage in mDataSourcesSettingsOrderedByPriorityList)
            {
                var storage = settingsOfStorage.Storage;
                storage.OnChanged += Storage_OnChanged;
            }
        }

        private void Storage_OnChanged()
        {
            Task.Run(() => {
                try
                {
                    EmitOnChanged();
                }catch(Exception e)
                {
                    LogInstance.Error($"e = {e}");
                }
            });
        }

        private readonly object mLockObj = new object();

        public void AddStorage(SettingsOfStorageForSearchingInThisSession settings)
        {
            lock(mLockObj)
            {
                mDataSourcesSettingsOrderedByPriorityList.Add(settings);
                mDataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityList.OrderByDescending(p => p.Priority).ToList();

                if (settings.UseFacts)
                {
                    mDataSourcesSettingsOrderedByPriorityAndUseFactsList.Add(settings);
                    mDataSourcesSettingsOrderedByPriorityAndUseFactsList = mDataSourcesSettingsOrderedByPriorityAndUseFactsList.OrderByDescending(p => p.Priority).ToList();
                }

                if (settings.UseProductions)
                {
                    mDataSourcesSettingsOrderedByPriorityAndUseProductionsList.Add(settings);
                    mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityAndUseProductionsList.OrderByDescending(p => p.Priority).ToList();
                }

                if (settings.UseAdditionalInstances)
                {
                    mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances.Add(settings);
                    mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances.OrderByDescending(p => p.Priority).ToList();
                }

#if DEBUG
                //LogInstance.Log($"mDataSourcesSettingsOrderedByPriorityList.Count = {mDataSourcesSettingsOrderedByPriorityList.Count}");
                //LogInstance.Log($"mDataSourcesSettingsOrderedByPriorityAndUseFactsList.Count = {mDataSourcesSettingsOrderedByPriorityAndUseFactsList.Count}");
#endif

                var storage = settings.Storage;
                storage.OnChanged += Storage_OnChanged;
            }
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Consolidated;

        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityList;
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseFactsList;
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseProductionsList;
        private IList<SettingsOfStorageForSearchingInThisSession> mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances;

        public override IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            lock (mLockObj)
            {
                var result = new List<ResolverForRelationExpressionNode>();

                var dataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityAndUseFactsList;

                foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
                {
                    var allRelationsOfStorage = dataSourcesSettings.Storage.GetAllRelations();

                    if (allRelationsOfStorage == null)
                    {
                        continue;
                    }

                    result.AddRange(allRelationsOfStorage);
                }

                return result;
            }
        }

        public override RuleInstance GetRuleInstanceByKey(ulong key)
        {
            lock (mLockObj)
            {
                var dataSourcesSettingsOrderedByPriorityList = mDataSourcesSettingsOrderedByPriorityAndUseFactsList;

                foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
                {
                    var ruleInstanceOfStorage = dataSourcesSettings.Storage.GetRuleInstanceByKey(key);

                    if (ruleInstanceOfStorage != null)
                    {
                        return ruleInstanceOfStorage;
                    }
                }

                return null;
            }
        }

        public override IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
            lock (mLockObj)
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
        }

        public override IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
            lock (mLockObj)
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
        }

        public override IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            lock (mLockObj)
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
        }

        public override IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            lock (mLockObj)
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
        }

        public override IList<ResolverForRelationExpressionNode> AllRelationsForProductions
        {
            get
            {
                lock (mLockObj)
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
}
