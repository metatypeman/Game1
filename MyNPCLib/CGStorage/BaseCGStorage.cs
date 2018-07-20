using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
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
        public abstract KindOfCGStorage Kind { get; }

        //It is temporary public for construction time. It will be private after complete construction.
        public string DictionaryName { get; set; }
        private readonly object mDataLockObj = new object();
        //It is temporary public for construction time. It will be private after complete construction.
        public IList<RuleInstance> mRuleInstancesList { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IndexedRuleInstance> mIndexedRuleInstancesDict { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IList<IndexedRulePart>> mIndexedRulePartsOfFactsDict { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IList<IndexedRulePart>> mIndexedRulePartsWithOneRelationWithVarsDict { get; set; }
        public IList<ResolverForRelationExpressionNode> mRelationsList { get; set; }

        public void Init()
        {
            lock(mDataLockObj)
            {
                mRuleInstancesList = new List<RuleInstance>();
                mIndexedRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();
                mIndexedRulePartsOfFactsDict = new Dictionary<ulong, IList<IndexedRulePart>>();
                mIndexedRulePartsWithOneRelationWithVarsDict = new Dictionary<ulong, IList<IndexedRulePart>>();
                mRelationsList = new List<ResolverForRelationExpressionNode>();
            }
        }

        //It is temporary public for construction time. It will be private after complete construction.
        public void NSetIndexedRuleInstanceToIndexData(IndexedRuleInstance indexedRuleInstance)
        {
            lock (mDataLockObj)
            {
#if DEBUG
                //LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");
#endif

                mIndexedRuleInstancesDict[indexedRuleInstance.Key] = indexedRuleInstance;

                var kind = indexedRuleInstance.Kind;

                switch(kind)
                {
                    case KindOfRuleInstance.Fact:
                        {
                            if(indexedRuleInstance.IsPart_1_Active)
                            {
                                NAddIndexedRulePartToKeysOfRelationsIndex(mIndexedRulePartsOfFactsDict, indexedRuleInstance.Part_1);
                            }
                            else
                            {
                                NAddIndexedRulePartToKeysOfRelationsIndex(mIndexedRulePartsOfFactsDict, indexedRuleInstance.Part_2);
                            }
                        }
                        break;

                    case KindOfRuleInstance.Rule:
                        {
                            var part_1 = indexedRuleInstance.Part_1;

                            if(part_1.HasVars && !part_1.HasQuestionVars && part_1.RelationsDict.Count == 1)
                            {
                                NAddIndexedRulePartToKeysOfRelationsIndex(mIndexedRulePartsWithOneRelationWithVarsDict, part_1);
                            }

                            var part_2 = indexedRuleInstance.Part_2;

                            if (part_2.HasVars && !part_2.HasQuestionVars && part_2.RelationsDict.Count == 1)
                            {
                                NAddIndexedRulePartToKeysOfRelationsIndex(mIndexedRulePartsWithOneRelationWithVarsDict, part_2);
                            }
                        }
                        break;

                    //default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        private void NAddIndexedRulePartToKeysOfRelationsIndex(IDictionary<ulong, IList<IndexedRulePart>> indexData, IndexedRulePart indexedRulePart)
        {
            var relationsList = indexedRulePart.RelationsDict.SelectMany(p => p.Value).Distinct().ToList();

            foreach(var relation in relationsList)
            {
                if (!mRelationsList.Contains(relation))
                {
                    mRelationsList.Add(relation);
                }
            }

            var keysOfRelationsList = indexedRulePart.RelationsDict.Keys.ToList();

            foreach(var keyOfRelation in keysOfRelationsList)
            {
                if(indexData.ContainsKey(keyOfRelation))
                {
                    var tmpList = indexData[keyOfRelation];
                    if(!tmpList.Contains(indexedRulePart))
                    {
                        tmpList.Add(indexedRulePart);
                    }
                }
                else
                {
                    var tmpList = new List<IndexedRulePart>() { indexedRulePart };
                    indexData[keyOfRelation] = tmpList;
                }
            }
        }

        public IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
#if DEBUG
                LogInstance.Log($"key = {key}");
                LogInstance.Log($"mIndexedRulePartsOfFactsDict.Count = {mIndexedRulePartsOfFactsDict.Count}");
#endif

                if (mIndexedRulePartsOfFactsDict.ContainsKey(key))
                {
                    return mIndexedRulePartsOfFactsDict[key];
                }

                return null;
            }
        }

        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
                if (mIndexedRulePartsWithOneRelationWithVarsDict.ContainsKey(key))
                {
                    return mIndexedRulePartsWithOneRelationWithVarsDict[key];
                }

                return null;
            }
        }

        public IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            lock (mDataLockObj)
            {
                return mRelationsList.ToList();
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
            sb.AppendLine($"{spaces}mIndexedRuleInstancesDict.Count = {mIndexedRuleInstancesDict.Count}");
            foreach(var indexedRuleInstancesKVPItem in mIndexedRuleInstancesDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRuleInstancesKVPItem.Key} ({entityDictionary.GetName(indexedRuleInstancesKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}mIndexedRulePartsOfFactsDict.Count = {mIndexedRulePartsOfFactsDict.Count}");
            foreach (var indexedRulePartsOfFactsKVPItem in mIndexedRulePartsOfFactsDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRulePartsOfFactsKVPItem.Key} ({entityDictionary.GetName(indexedRulePartsOfFactsKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}mIndexedRulePartsWithOneRelationWithVarsDict.Count = {mIndexedRulePartsWithOneRelationWithVarsDict.Count}");
            foreach (var indexedRulePartsWithOneRelationWithVarsKVPItem in mIndexedRulePartsWithOneRelationWithVarsDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRulePartsWithOneRelationWithVarsKVPItem.Key} ({entityDictionary.GetName(indexedRulePartsWithOneRelationWithVarsKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}mRelationsList.Count = {mRelationsList.Count}");
            foreach(var relation in mRelationsList)
            {
                sb.AppendLine($"{spaces}Key = {relation.Key} ({entityDictionary.GetName(relation.Key)})");
            }

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
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
