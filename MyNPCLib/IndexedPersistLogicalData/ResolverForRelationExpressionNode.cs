using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class ResolverForRelationExpressionNode: ResolverForBaseExpressionNode
    {
        public RelationExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Relation;
        public ulong Key { get; set; }
        public int CountParams { get; set; }
        public IList<ResolverForBaseExpressionNode> Params { get; set; }
        public IList<QueryExecutingCardAboutVar> VarsInfoList { get; set; }
        public IList<QueryExecutingCardAboutKnownInfo> KnownInfoList { get; set; }
        
        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"Key = {Key}");
            LogInstance.Log($"Params.Count = {Params.Count}");
            foreach (var param in Params)
            {
                LogInstance.Log($"param = {param}");
            }
            LogInstance.Log($"VarsInfoList.Count = {VarsInfoList.Count}");
            foreach (var varInfo in VarsInfoList)
            {
                LogInstance.Log($"varInfo = {varInfo}");
            }
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}"); 
#endif

            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;
            
            var indexedRulePartsOfFactsList = GetIndexedRulePartOfFactsByKeyOfRelation(Key, context);

#if DEBUG
            LogInstance.Log($"indexedRulePartsOfFactsList?.Count = {indexedRulePartsOfFactsList?.Count}");
#endif

            var mergingResult = QueryExecutingCardAboutKnownInfoHelper.Merge(KnownInfoList, VarsInfoList, queryExecutingCard.KnownInfoList, false);

            if (!mergingResult.IsSuccess)
            {
                return;
            }

            var targetKnownInfoList = mergingResult.KnownInfoList;

#if DEBUG
            LogInstance.Log($"targetKnownInfoList.Count = {targetKnownInfoList.Count}");
            foreach (var tmpKnownInfo in targetKnownInfoList)
            {
                LogInstance.Log($"tmpKnownInfo = {tmpKnownInfo}");
            }
#endif

            if (indexedRulePartsOfFactsList.Count > 0)
            {
                foreach (var indexedRulePartsOfFacts in indexedRulePartsOfFactsList)
                {
#if DEBUG
                    LogInstance.Log($"this = {this}");
                    //LogInstance.Log($"indexedRulePartsOfFacts = {indexedRulePartsOfFacts}");
#endif
                    var queryExecutingCardForTargetFact = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetFact.TargetRelation = Key;
                    queryExecutingCardForTargetFact.CountParams = CountParams;
                    queryExecutingCardForTargetFact.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetFact.KnownInfoList = targetKnownInfoList;
                    queryExecutingCardForTargetFact.SenderIndexedRuleInstance = senderIndexedRuleInstance;
                    queryExecutingCardForTargetFact.SenderIndexedRulePart = senderIndexedRulePart;
                    queryExecutingCardForTargetFact.SenderExpressionNode = Origin;

                    indexedRulePartsOfFacts.FillExecutingCardForCallingFromRelationForFact(queryExecutingCardForTargetFact, context);

#if DEBUG
                    LogInstance.Log($"++++++queryExecutingCardForTargetFact = {queryExecutingCardForTargetFact}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetFact.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            LogInstance.Log($"~~~~~~~~~~~~~~~~~queryExecutingCard = {queryExecutingCard}");
#endif

            var indexedRulePartWithOneRelationsList = GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(Key, context);

#if DEBUG
            LogInstance.Log($"indexedRulePartWithOneRelationsList?.Count = {indexedRulePartWithOneRelationsList?.Count}");
#endif

            if(indexedRulePartWithOneRelationsList.Count > 0)
            {
                foreach (var indexedRulePartsOfRule in indexedRulePartWithOneRelationsList)
                {
#if DEBUG
                    LogInstance.Log($"this = {this}");
                    LogInstance.Log($"indexedRulePartsOfRule = {indexedRulePartsOfRule}");
#endif

                    var queryExecutingCardForTargetRule = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetRule.TargetRelation = Key;
                    queryExecutingCardForTargetRule.CountParams = CountParams;
                    queryExecutingCardForTargetRule.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetRule.KnownInfoList = targetKnownInfoList;
                    queryExecutingCardForTargetRule.SenderIndexedRuleInstance = senderIndexedRuleInstance;
                    queryExecutingCardForTargetRule.SenderIndexedRulePart = senderIndexedRulePart;
                    queryExecutingCardForTargetRule.SenderExpressionNode = Origin;

                    indexedRulePartsOfRule.FillExecutingCardForCallingFromRelationForProduction(queryExecutingCardForTargetRule, context);

#if DEBUG
                    LogInstance.Log($"&&&&&&&&&&&&&&&&&queryExecutingCardForTargetRule = {queryExecutingCardForTargetRule}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetRule.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            LogInstance.Log($"^^^^^^queryExecutingCard = {queryExecutingCard}");
            LogInstance.Log("End");
#endif
        }

        [Obsolete]
        public override void FillExecutingCardForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"Key = {Key}");
            LogInstance.Log($"Params.Count = {Params.Count}");
            foreach (var param in Params)
            {
                LogInstance.Log($"param = {param}");
            }
#endif

            var indexedRulePartsOfFactsList = source.GetIndexedRulePartOfFactsByKeyOfRelation(Key);

#if DEBUG
            LogInstance.Log($"indexedRulePartsOfFactsList?.Count = {indexedRulePartsOfFactsList?.Count}");
#endif

            if (indexedRulePartsOfFactsList != null)
            {
                foreach (var indexedRulePartsOfFacts in indexedRulePartsOfFactsList)
                {
#if DEBUG
                    LogInstance.Log($"this = {this}");
                    //LogInstance.Log($"indexedRulePartsOfFacts = {indexedRulePartsOfFacts}");
#endif
                    var queryExecutingCardForTargetFact = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetFact.TargetRelation = Key;
                    queryExecutingCardForTargetFact.CountParams = CountParams;
                    queryExecutingCardForTargetFact.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetFact.KnownInfoList = KnownInfoList;

                    indexedRulePartsOfFacts.FillExecutingCardForCallingFromRelationForFact(queryExecutingCardForTargetFact, source, context);

#if DEBUG
                    LogInstance.Log($"queryExecutingCardForTargetFact = {queryExecutingCardForTargetFact}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetFact.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

        }

        [Obsolete]
        public override void FillExecutingCardForProduction(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"Key = {Key}");
            LogInstance.Log($"Params.Count = {Params.Count}");
            foreach (var param in Params)
            {
                LogInstance.Log($"param = {param}");
            }
#endif

            var indexedRulePartWithOneRelationsList = GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(Key, context);

#if DEBUG
            LogInstance.Log($"indexedRulePartWithOneRelationsList?.Count = {indexedRulePartWithOneRelationsList?.Count}");
#endif
            if(indexedRulePartWithOneRelationsList.Count > 0)
            {
                foreach (var indexedRulePartsOfRule in indexedRulePartWithOneRelationsList)
                {
#if DEBUG
                    LogInstance.Log($"this = {this}");
                    LogInstance.Log($"indexedRulePartsOfRule = {indexedRulePartsOfRule}");
#endif

                    var queryExecutingCardForTargetRule = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetRule.TargetRelation = Key;
                    queryExecutingCardForTargetRule.CountParams = CountParams;
                    queryExecutingCardForTargetRule.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetRule.KnownInfoList = KnownInfoList;

                    indexedRulePartsOfRule.FillExecutingCardForCallingFromRelationForProduction(queryExecutingCardForTargetRule, context);

#if DEBUG
                    LogInstance.Log($"queryExecutingCardForTargetRule = {queryExecutingCardForTargetRule}");
#endif
                }
            }

            throw new NotImplementedException();
        }

        private IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key, LogicalSearchContext context)
        {
            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityList = context.DataSourcesSettingsOrderedByPriorityList;

            foreach(var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityList)
            {
                var indexedRulePartsOfFactsList = dataSourcesSettings.Storage.GetIndexedRulePartOfFactsByKeyOfRelation(key);

                if(indexedRulePartsOfFactsList == null)
                {
                    continue;
                }

                result.AddRange(indexedRulePartsOfFactsList);
            }

            return result;
        }

        private IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key, LogicalSearchContext context)
        {
            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityAndUseProductionsList)
            {
                var indexedRulePartWithOneRelationsList = dataSourcesSettings.Storage.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(Key);

                if(indexedRulePartWithOneRelationsList == null)
                {
                    continue;
                }

                result.AddRange(indexedRulePartWithOneRelationsList);
            }

            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(CountParams)} = {CountParams}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            if (VarsInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsInfoList)}");
                foreach (var varItem in VarsInfoList)
                {
                    sb.Append(varItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsInfoList)}");
            }
            if (KnownInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(KnownInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(KnownInfoList)}");
                foreach (var knownInfo in KnownInfoList)
                {
                    sb.Append(knownInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(KnownInfoList)}");
            }
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(CountParams)} = {CountParams}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            if (VarsInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsInfoList)}");
                foreach (var varItem in VarsInfoList)
                {
                    sb.Append(varItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsInfoList)}");
            }
            if (KnownInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(KnownInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(KnownInfoList)}");
                foreach (var knownInfo in KnownInfoList)
                {
                    sb.Append(knownInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(KnownInfoList)}");
            }
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
