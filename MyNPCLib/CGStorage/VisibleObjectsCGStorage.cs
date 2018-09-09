using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.CGStorage
{
    public class VisibleObjectsCGStorage: BaseProxyStorage
    {
        public VisibleObjectsCGStorage(IEntityDictionary entityDictionary, StorageOfSpecialEntities storageOfSpecialEntities, IBusOfCGStorages busOfCGStorages)
            : base(entityDictionary)
        {
            mBusOfCGStorages = busOfCGStorages;

            mDataSourcesSettingsOrderedByPriorityList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseFactsList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = new List<SettingsOfStorageForSearchingInThisSession>();
            mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = new List<SettingsOfStorageForSearchingInThisSession>();

            storageOfSpecialEntities.OnUpdateVisibleEntitiesIdList += StorageOfSpecialEntities_OnUpdateVisibleEntitiesIdList;
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.OtherProxy;
        private readonly object mLockObj = new object();
        private IBusOfCGStorages mBusOfCGStorages;
        private Dictionary<ulong, SettingsOfStorageForSearchingInThisSession> mStoragesSettingsDict = new Dictionary<ulong, SettingsOfStorageForSearchingInThisSession>();

        private void StorageOfSpecialEntities_OnUpdateVisibleEntitiesIdList(IList<ulong> visibleEntitiesId, IList<ulong> oldVisibleEntitiesId)
        {
            lock(mLockObj)
            {
                foreach(var entityId in visibleEntitiesId)
                {
                    var storage = mBusOfCGStorages.GetStorageWithVisibleFacts(entityId);

                    if(storage == null)
                    {
                        continue;
                    }

                    var storageOptions = new SettingsOfStorageForSearchingInThisSession();
                    storageOptions.Storage = storage;
                    storageOptions.MaxDeph = null;
                    storageOptions.UseFacts = true;
                    storageOptions.UseAdditionalInstances = true;
                    storageOptions.UseProductions = false;
                    storageOptions.Priority = 1;

                    mStoragesSettingsDict[entityId] = storageOptions;
                }

                foreach(var entityId in oldVisibleEntitiesId)
                {
                    mStoragesSettingsDict.Remove(entityId);
                }

                mDataSourcesSettingsOrderedByPriorityList = mStoragesSettingsDict.Values.OrderByDescending(p => p.Priority).ToList();
                mDataSourcesSettingsOrderedByPriorityAndUseFactsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseFacts).ToList();
                mDataSourcesSettingsOrderedByPriorityAndUseProductionsList = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();
                mDataSourcesSettingsOrderedByPriorityAndUseAdditionalInstances = mDataSourcesSettingsOrderedByPriorityList.Where(p => p.UseAdditionalInstances).ToList();
            }
        }

        private void Storage_OnChanged()
        {
            Task.Run(() => {
                try
                {
                    EmitOnChanged();
                }
                catch (Exception e)
                {
                    LogInstance.Error($"e = {e}");
                }
            });
        }

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
