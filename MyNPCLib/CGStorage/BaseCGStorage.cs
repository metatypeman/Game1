using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseCGStorage: ICGStorage
    {
        protected BaseCGStorage(ContextOfCGStorage context)
        {
            mContext = context;
            DictionaryName = mContext.EntityDictionary.Name;
        }

        private ContextOfCGStorage mContext;
        public abstract KindOfCGStorage KindOfStorage { get; }

        //It is temporary public for construction time. It will be private after complete construction.
        public string DictionaryName { get; set; }
        private readonly object mDataLockObj = new object();
        //It is temporary public for construction time. It will be private after complete construction.
        public IList<RuleInstance> mRuleInstancesList { get; set; }
        private CommonPersistIndexedLogicalData mCommonPersistIndexedLogicalData { get; set; }

        public void Init()
        {
            lock(mDataLockObj)
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

                mCommonPersistIndexedLogicalData.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
            }
        }

        public IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
#if DEBUG
                LogInstance.Log($"key = {key}");
#endif

                return mCommonPersistIndexedLogicalData.GetIndexedRulePartOfFactsByKeyOfRelation(key);
            }
        }

        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(key);
            }
        }

        public IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            lock (mDataLockObj)
            {
                return mCommonPersistIndexedLogicalData.GetAllRelations();
            }
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

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(KindOfStorage)} = {KindOfStorage}");
            return sb.ToString();
        }
    }
}
