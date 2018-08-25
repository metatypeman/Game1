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
        protected BaseRealStorage(ContextOfCGStorage context)
            : base(context)
        {
            mRuleInstancesList = new List<RuleInstance>();
            mRuleInstancesDict = new Dictionary<ulong, RuleInstance>();
            mCommonPersistIndexedLogicalData = new CommonPersistIndexedLogicalData();
        }

        protected BaseRealStorage(ContextOfCGStorage context, RuleInstancePackage ruleInstancePackage)
            : this(context)
        {
            var ruleInstancesList = ruleInstancePackage.AllRuleInstances;
            var mainRuleInstance = ruleInstancePackage.MainRuleInstance;

            foreach(var ruleInstance in ruleInstancesList)
            {
                Append(ruleInstance);
            }

            if(mainRuleInstance != null)
            {
                mMainRuleInstance = mainRuleInstance;
                mMainIndexedRuleInstance = mCommonPersistIndexedLogicalData.GetIndexedRuleInstanceByKey(mainRuleInstance.Key);
            }
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

                var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

                ruleInstance.DataSource = this;

                mRuleInstancesList.Add(ruleInstance);
                mRuleInstancesDict[ruleInstanceKey] = ruleInstance;
                NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
            }
        }

        public override void Append(ICGStorage storage)
        {
            throw new NotImplementedException();
        }

        public override void Append(RuleInstancePackage ruleInstancePackage)
        {
            throw new NotImplementedException();
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
            var entityDictionary = Context.EntityDictionary;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}mRuleInstancesList.Count = {mRuleInstancesList.Count}");
            sb.Append(mCommonPersistIndexedLogicalData.GetContentAsDbgStr(entityDictionary));
            return sb.ToString();
        }
    }
}
