using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseRealStorage: BaseCGStorage
    {
        protected BaseRealStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
            mRuleInstancesList = new List<RuleInstance>();
            mRuleInstancesDict = new Dictionary<ulong, RuleInstance>();
            mCommonPersistIndexedLogicalData = new CommonPersistIndexedLogicalData();
        }

        protected BaseRealStorage(IEntityDictionary entityDictionary, RuleInstancePackage ruleInstancePackage)
            : this(entityDictionary)
        {
            Append(ruleInstancePackage);
        }

        public override IList<RuleInstance> AllRuleInstances => mRuleInstancesList;
        private readonly object mDataLockObj = new object();

        private RuleInstance mMainRuleInstance;
        private List<RuleInstance> mRuleInstancesList;
        private Dictionary<ulong, RuleInstance> mRuleInstancesDict;
        private IndexedRuleInstance mMainIndexedRuleInstance;

        private CommonPersistIndexedLogicalData mCommonPersistIndexedLogicalData { get; set; }

        public override void Append(RuleInstance ruleInstance)
        {
#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif
            lock (mDataLockObj)
            {
                NAppend(ruleInstance);
            }
        }

        public override void Append(ICGStorage storage)
        {
            lock (mDataLockObj)
            {
                var ruleInstancesList = storage.AllRuleInstances;
                var mainRuleInstance = storage.MainRuleInstance;

                if (mainRuleInstance != null)
                {
                    NAppend(mainRuleInstance, true);
                }

                foreach (var ruleInstance in ruleInstancesList)
                {
                    NAppend(ruleInstance);
                }
            }            
        }

        public override void Append(RuleInstancePackage ruleInstancePackage)
        {
            lock (mDataLockObj)
            {
                var ruleInstancesList = ruleInstancePackage.AllRuleInstances;
                var mainRuleInstance = ruleInstancePackage.MainRuleInstance;

                if (mainRuleInstance != null)
                {
                    NAppend(mainRuleInstance, true);
                }

                foreach (var ruleInstance in ruleInstancesList)
                {
                    NAppend(ruleInstance);
                }
            }
        }

        private void NAppend(RuleInstance ruleInstance, bool setAsMain = false)
        {
#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

            var ruleInstanceKey = ruleInstance.Key;

            if (ruleInstanceKey == 0)
            {
                throw new NotSupportedException();
            }

            if (mRuleInstancesDict.ContainsKey(ruleInstanceKey))
            {
                return;
            }

            if (mRuleInstancesList.Contains(ruleInstance))
            {
                return;
            }

            ruleInstance = ruleInstance.Clone();

#if DEBUG
            //LogInstance.Log($"after ruleInstance = {ruleInstance}");
#endif

            var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            ruleInstance.DataSource = this;

            mRuleInstancesList.Add(ruleInstance);
            mRuleInstancesDict[ruleInstanceKey] = ruleInstance;
            NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);

            if(setAsMain)
            {
                mMainRuleInstance = ruleInstance;
                mMainIndexedRuleInstance = indexedRuleInstance;
            }

            EmitOnChanged();
        }

        private void NSetIndexedRuleInstanceToIndexData(IndexedRuleInstance indexedRuleInstance)
        {
#if DEBUG
            //LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");
#endif

            mCommonPersistIndexedLogicalData.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
        }

        public override IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
#if DEBUG
                //LogInstance.Log($"key = {key}");
#endif

                return mCommonPersistIndexedLogicalData.GetIndexedRulePartOfFactsByKeyOfRelation(key);
            }
        }

        public override IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(key);
            }
        }

        public override IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetAllRelations();
            }
        }

        public override IList<ResolverForRelationExpressionNode> AllRelationsForProductions
        {
            get
            {
                lock (mDataLockObj)
                {
                    return mCommonPersistIndexedLogicalData.GetAllRelations();
                }
            }
        }

        public override RuleInstance GetRuleInstanceByKey(ulong key)
        {
            lock (mDataLockObj)
            {
                if (mRuleInstancesDict.ContainsKey(key))
                {
                    return mRuleInstancesDict[key];
                }

                return null;
            }
        }

        public override IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetIndexedRuleInstanceByKey(key);
            }
        }

        public override IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetIndexedAdditionalRuleInstanceByKey(key);
            }
        }

        public override RuleInstance MainRuleInstance
        {
            get
            {
                lock (mDataLockObj)
                {
                    return mMainRuleInstance;
                }
            }
        }

        public override IndexedRuleInstance MainIndexedRuleInstance
        {
            get
            {
                lock (mDataLockObj)
                {
                    return mMainIndexedRuleInstance;
                }
            }
        }

        public string GetContentAsDbgStr()
        {
            var n = 0u;
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}mRuleInstancesList.Count = {mRuleInstancesList.Count}");
            sb.Append(mCommonPersistIndexedLogicalData.GetContentAsDbgStr(EntityDictionary));
            return sb.ToString();
        }
    }
}
