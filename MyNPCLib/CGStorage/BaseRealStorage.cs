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
        }

        public override IList<RuleInstance> AllRuleInstances => mRuleInstancesList;
        private readonly object mDataLockObj = new object();
        //It is temporary public for construction time. It will be private after complete construction.
        public IList<RuleInstance> mRuleInstancesList { get; set; }
        private CommonPersistIndexedLogicalData mCommonPersistIndexedLogicalData { get; set; }

        public override void Init()
        {
            lock (mDataLockObj)
            {
                mRuleInstancesList = new List<RuleInstance>();
                mCommonPersistIndexedLogicalData = new CommonPersistIndexedLogicalData();
                mCommonPersistIndexedLogicalData.Init();
            }
        }

        public void NSetIndexedRuleInstanceToIndexData(IndexedRuleInstance indexedRuleInstance)
        {
            lock (mDataLockObj)
            {
#if DEBUG
                //LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");
#endif

                if (!mRuleInstancesList.Contains(indexedRuleInstance.Origin))
                {
                    mRuleInstancesList.Add(indexedRuleInstance.Origin);
                }

                mCommonPersistIndexedLogicalData.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
            }
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

        public override RuleInstance GetRuleInstanceByKey(ulong key)
        {
            return mRuleInstancesList.FirstOrDefault(p => p.Key == key);
        }

        public string GetContentAsDbgStr()
        {
            var n = 0u;
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = StringHelper.Spaces(nextN);
            var entityDictionary = mContext.EntityDictionary;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}mRuleInstancesList.Count = {mRuleInstancesList.Count}");
            sb.Append(mCommonPersistIndexedLogicalData.GetContentAsDbgStr(entityDictionary));
            return sb.ToString();
        }
    }
}
