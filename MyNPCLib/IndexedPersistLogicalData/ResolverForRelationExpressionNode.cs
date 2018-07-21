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
        public bool IsQuestion { get; set; }
        public IList<ResolverForBaseExpressionNode> Params { get; set; }
        public IList<QueryExecutingCardAboutVar> VarsInfoList { get; set; }
        public IList<QueryExecutingCardAboutKnownInfo> KnownInfoList { get; set; }
        
        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            if(IsQuestion)
            {
                FillExecutingCardForQuestion(queryExecutingCard, context);
                return;
            }

#if DEBUG
            LogInstance.Log($"Key = {Key}");
            LogInstance.Log($"IsQuestion = {IsQuestion}");
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

            if (indexedRulePartWithOneRelationsList.Count > 0)
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
#if DEBUG
                    LogInstance.Log($"!!!!!!!!!!!!!!!!!!queryExecutingCard = {queryExecutingCard}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetRule.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            LogInstance.Log($"^^^^^^queryExecutingCard = {queryExecutingCard}");
            LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");

            //throw new NotImplementedException();

            LogInstance.Log("End");
#endif
        }

        private void FillExecutingCardForQuestion(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"Key = {Key}");
            LogInstance.Log($"IsQuestion = {IsQuestion}");
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

            var hasAnnotations = !Annotations.IsEmpty();

#if DEBUG
            LogInstance.Log($"hasAnnotations = {hasAnnotations}");
#endif

            var targetRelationsList = GetAllRelations(context);

#if DEBUG
            LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            foreach(var targetRelation in targetRelationsList)
            {
                if(targetRelation.CountParams != CountParams)
                {
                    continue;
                }
#if DEBUG
                LogInstance.Log($"targetRelation.GetHumanizeDbgString() = {targetRelation.GetHumanizeDbgString()}");
                //LogInstance.Log($"targetRelation = {targetRelation}");
                LogInstance.Log($"hasAnnotations = {hasAnnotations}");
#endif

                if (hasAnnotations)
                {
                    if(targetRelation.Annotations.IsEmpty())
                    {
                        continue;
                    }

                    var isCheckAnnotation = false;

                    foreach(var annotation in Annotations)
                    {
                        foreach (var annotationOfTargetRealtion in targetRelation.Annotations)
                        {
                            var tmpCheckAnnotation = CheckAnnotation(annotation, annotationOfTargetRealtion);

                            if(tmpCheckAnnotation)
                            {
                                isCheckAnnotation = true;
                            }
                        }
                    }

#if DEBUG
                    LogInstance.Log($"isCheckAnnotation = {isCheckAnnotation}");
#endif

                    if(!isCheckAnnotation)
                    {
                        continue;
                    }

#if DEBUG
                    throw new NotImplementedException();
#endif
                }

#if DEBUG
                LogInstance.Log($"NEXT targetRelation.GetHumanizeDbgString() = {targetRelation.GetHumanizeDbgString()}");
#endif
            }

#if DEBUG
            throw new NotImplementedException();
            LogInstance.Log("End");
#endif
        }

        private bool CheckAnnotation(IndexedLogicalAnnotation annotationOfQuery, IndexedLogicalAnnotation annotationOfStored)
        {
#if DEBUG
            LogInstance.Log($"annotationOfQuery = {annotationOfQuery.GetHumanizeDbgString()}");
            LogInstance.Log($"annotationOfStored = {annotationOfStored.GetHumanizeDbgString()}");
#endif

            return false;//tmp
        }

        private IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"key = {key}");
#endif

            var result = new List<IndexedRulePart>();

            var dataSourcesSettingsOrderedByPriorityList = context.DataSourcesSettingsOrderedByPriorityAndUseFactsList;

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

        private IList<IndexedRulePart> GetEntityConditions(ulong key, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"key = {key}");
#endif

            throw new NotImplementedException();
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

        private IList<ResolverForRelationExpressionNode> GetAllRelations(LogicalSearchContext context)
        {
            var result = new List<ResolverForRelationExpressionNode>();

            var dataSourcesSettingsOrderedByPriorityAndUseProductionsList = context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList;

            foreach (var dataSourcesSettings in dataSourcesSettingsOrderedByPriorityAndUseProductionsList)
            {
                var targetRelationsList = dataSourcesSettings.Storage.GetAllRelations();

                if(targetRelationsList == null)
                {
                    continue;
                }

                result.AddRange(targetRelationsList);
            }

            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
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
