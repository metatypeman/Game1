using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.PersistLogicalDataStorage
{
    [Serializable]
    public class CommonPersistIndexedLogicalData
    {
        public CommonPersistIndexedLogicalData()
        {
            IndexedRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();
            AdditionalRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();
            IndexedRulePartsOfFactsDict = new Dictionary<ulong, IList<IndexedRulePart>>();
            IndexedRulePartsWithOneRelationWithVarsDict = new Dictionary<ulong, IList<IndexedRulePart>>();
            RelationsList = new List<ResolverForRelationExpressionNode>();
        }

        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IndexedRuleInstance> IndexedRuleInstancesDict { get; set; }
        public IDictionary<ulong, IndexedRuleInstance> AdditionalRuleInstancesDict { get; set; }

        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IList<IndexedRulePart>> IndexedRulePartsOfFactsDict { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IList<IndexedRulePart>> IndexedRulePartsWithOneRelationWithVarsDict { get; set; }
        public IList<ResolverForRelationExpressionNode> RelationsList { get; set; }

        //It is temporary public for construction time. It will be private after complete construction.
        public void NSetIndexedRuleInstanceToIndexData(IndexedRuleInstance indexedRuleInstance)
        {
#if DEBUG
            //LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");
#endif

            IndexedRuleInstancesDict[indexedRuleInstance.Key] = indexedRuleInstance;

            var kind = indexedRuleInstance.Kind;

            switch (kind)
            {
                case KindOfRuleInstance.Fact:
                case KindOfRuleInstance.Annotation:
                    {
                        if (indexedRuleInstance.IsPart_1_Active)
                        {
                            NAddIndexedRulePartToKeysOfRelationsIndex(IndexedRulePartsOfFactsDict, indexedRuleInstance.Part_1);
                        }
                        else
                        {
                            NAddIndexedRulePartToKeysOfRelationsIndex(IndexedRulePartsOfFactsDict, indexedRuleInstance.Part_2);
                        }
                    }
                    break;

                case KindOfRuleInstance.Rule:
                    {
                        var part_1 = indexedRuleInstance.Part_1;

                        if (part_1.HasVars && !part_1.HasQuestionVars && part_1.RelationsDict.Count == 1)
                        {
                            NAddIndexedRulePartToKeysOfRelationsIndex(IndexedRulePartsWithOneRelationWithVarsDict, part_1);
                        }

                        var part_2 = indexedRuleInstance.Part_2;

                        if (part_2.HasVars && !part_2.HasQuestionVars && part_2.RelationsDict.Count == 1)
                        {
                            NAddIndexedRulePartToKeysOfRelationsIndex(IndexedRulePartsWithOneRelationWithVarsDict, part_2);
                        }
                    }
                    break;

                case KindOfRuleInstance.EntityCondition:
                    break;

                case KindOfRuleInstance.QuestionVars:
                    break;

                    default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            switch (kind)
            {
                case KindOfRuleInstance.Annotation:
                case KindOfRuleInstance.EntityCondition:
                    AdditionalRuleInstancesDict[indexedRuleInstance.Key] = indexedRuleInstance;
                    break;
            }        
        }

        private void NAddIndexedRulePartToKeysOfRelationsIndex(IDictionary<ulong, IList<IndexedRulePart>> indexData, IndexedRulePart indexedRulePart)
        {
            var relationsList = indexedRulePart.RelationsDict.SelectMany(p => p.Value).Distinct().ToList();

            foreach (var relation in relationsList)
            {
                if (!RelationsList.Contains(relation))
                {
                    RelationsList.Add(relation);
                }
            }

            var keysOfRelationsList = indexedRulePart.RelationsDict.Keys.ToList();

            foreach (var keyOfRelation in keysOfRelationsList)
            {
                if (indexData.ContainsKey(keyOfRelation))
                {
                    var tmpList = indexData[keyOfRelation];
                    if (!tmpList.Contains(indexedRulePart))
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
#if DEBUG
            //LogInstance.Log($"key = {key}");
            //LogInstance.Log($"IndexedRulePartsOfFactsDict.Count = {IndexedRulePartsOfFactsDict.Count}");
#endif

            if (IndexedRulePartsOfFactsDict.ContainsKey(key))
            {
                return IndexedRulePartsOfFactsDict[key];
            }

            return null;
        }

        public IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            if (IndexedRulePartsWithOneRelationWithVarsDict.ContainsKey(key))
            {
                return IndexedRulePartsWithOneRelationWithVarsDict[key];
            }

            return null;
        }

        public IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            return RelationsList.ToList();
        }

        public IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
            if(IndexedRuleInstancesDict.ContainsKey(key))
            {
                return IndexedRuleInstancesDict[key];
            }

            return null;
        }

        public IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            if (AdditionalRuleInstancesDict.ContainsKey(key))
            {
                return AdditionalRuleInstancesDict[key];
            }

            return null;
        }

        public string GetContentAsDbgStr(IEntityDictionary entityDictionary)
        {
            var n = 0u;
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}IndexedRuleInstancesDict.Count = {IndexedRuleInstancesDict.Count}");
            foreach (var indexedRuleInstancesKVPItem in IndexedRuleInstancesDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRuleInstancesKVPItem.Key} ({entityDictionary.GetName(indexedRuleInstancesKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}IndexedRulePartsOfFactsDict.Count = {IndexedRulePartsOfFactsDict.Count}");
            foreach (var indexedRulePartsOfFactsKVPItem in IndexedRulePartsOfFactsDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRulePartsOfFactsKVPItem.Key} ({entityDictionary.GetName(indexedRulePartsOfFactsKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}IndexedRulePartsWithOneRelationWithVarsDict.Count = {IndexedRulePartsWithOneRelationWithVarsDict.Count}");
            foreach (var indexedRulePartsWithOneRelationWithVarsKVPItem in IndexedRulePartsWithOneRelationWithVarsDict)
            {
                sb.AppendLine($"{spaces}Key = {indexedRulePartsWithOneRelationWithVarsKVPItem.Key} ({entityDictionary.GetName(indexedRulePartsWithOneRelationWithVarsKVPItem.Key)})");
            }
            sb.AppendLine($"{spaces}RelationsList.Count = {RelationsList.Count}");
            foreach (var relation in RelationsList)
            {
                sb.AppendLine($"{spaces}Key = {relation.Key} ({entityDictionary.GetName(relation.Key)})");
            }

            return sb.ToString();
        }
    }
}
