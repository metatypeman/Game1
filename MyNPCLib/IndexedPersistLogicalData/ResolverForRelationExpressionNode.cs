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
        
        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
            if(IsQuestion)
            {
                FillExecutingCardForQuestion(queryExecutingCard, dataSource, options);
                return;
            }

#if DEBUG
            //LogInstance.Log($"Key = {Key}");
            //LogInstance.Log($"IsQuestion = {IsQuestion}");
            //LogInstance.Log($"Params.Count = {Params.Count}");
            //foreach (var param in Params)
            //{
            //    LogInstance.Log($"param = {param}");
            //}
            //LogInstance.Log($"VarsInfoList.Count = {VarsInfoList.Count}");
            //foreach (var varInfo in VarsInfoList)
            //{
            //    LogInstance.Log($"varInfo = {varInfo}");
            //}
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");
#endif

            //var strategyForGettingInfo = new StrategyForGettingInfoFromStoragesByLogicalSearchContext(context);

            NFillExecutingCard(queryExecutingCard, dataSource, options);

#if DEBUG
            //LogInstance.Log($"^^^^^^queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");

            //throw new NotImplementedException();

            //LogInstance.Log("End");
#endif
        }

        public override void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"Key = {Key}");
            //LogInstance.Log($"IsQuestion = {IsQuestion}");
            //LogInstance.Log($"Params.Count = {Params.Count}");
            //foreach (var param in Params)
            //{
            //    LogInstance.Log($"param = {param}");
            //}
            //LogInstance.Log($"VarsInfoList.Count = {VarsInfoList.Count}");
            //foreach (var varInfo in VarsInfoList)
            //{
            //    LogInstance.Log($"varInfo = {varInfo}");
            //}
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");
#endif

            NFillExecutingCard(queryExecutingCard, dataSource, options);

#if DEBUG
            //LogInstance.Log($"^^^^^^queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");

            //throw new NotImplementedException();

            //LogInstance.Log("End");
#endif
        }

        private void NFillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;

            //var dataSource = context.DataSource;

            var indexedRulePartsOfFactsList = dataSource.GetIndexedRulePartOfFactsByKeyOfRelation(Key);

#if DEBUG
            //LogInstance.Log($"indexedRulePartsOfFactsList?.Count = {indexedRulePartsOfFactsList?.Count}");
#endif

            var mergingResult = QueryExecutingCardAboutKnownInfoHelper.Merge(KnownInfoList, VarsInfoList, queryExecutingCard.KnownInfoList, false);

            if (!mergingResult.IsSuccess)
            {
                return;
            }

            var targetKnownInfoList = mergingResult.KnownInfoList;

#if DEBUG
            //LogInstance.Log($"targetKnownInfoList.Count = {targetKnownInfoList.Count}");
            //foreach (var tmpKnownInfo in targetKnownInfoList)
            //{
            //    LogInstance.Log($"tmpKnownInfo = {tmpKnownInfo}");
            //}
#endif

            if (indexedRulePartsOfFactsList.Count > 0)
            {
                foreach (var indexedRulePartsOfFacts in indexedRulePartsOfFactsList)
                {
#if DEBUG
                    //LogInstance.Log($"this = {this}");
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

                    indexedRulePartsOfFacts.FillExecutingCardForCallingFromRelationForFact(queryExecutingCardForTargetFact, dataSource, options);

#if DEBUG
                    //LogInstance.Log($"++++++queryExecutingCardForTargetFact = {queryExecutingCardForTargetFact}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetFact.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            //LogInstance.Log($"~~~~~~~~~~~~~~~~~queryExecutingCard = {queryExecutingCard}");
#endif

            var indexedRulePartWithOneRelationsList = dataSource.GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(Key);

#if DEBUG
            //LogInstance.Log($"indexedRulePartWithOneRelationsList?.Count = {indexedRulePartWithOneRelationsList?.Count}");
#endif

            if (!indexedRulePartWithOneRelationsList.IsEmpty())
            {
                foreach (var indexedRulePartsOfRule in indexedRulePartWithOneRelationsList)
                {
#if DEBUG
                    //LogInstance.Log($"this = {this}");
                    //LogInstance.Log($"indexedRulePartsOfRule = {indexedRulePartsOfRule}");
#endif
                    var queryExecutingCardForTargetRule = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetRule.TargetRelation = Key;
                    queryExecutingCardForTargetRule.CountParams = CountParams;
                    queryExecutingCardForTargetRule.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetRule.KnownInfoList = targetKnownInfoList;
                    queryExecutingCardForTargetRule.SenderIndexedRuleInstance = senderIndexedRuleInstance;
                    queryExecutingCardForTargetRule.SenderIndexedRulePart = senderIndexedRulePart;
                    queryExecutingCardForTargetRule.SenderExpressionNode = Origin;

                    indexedRulePartsOfRule.FillExecutingCardForCallingFromRelationForProduction(queryExecutingCardForTargetRule, dataSource, options);

#if DEBUG
                    //LogInstance.Log($"&&&&&&&&&&&&&&&&&queryExecutingCardForTargetRule = {queryExecutingCardForTargetRule}");
#endif
#if DEBUG
                    //LogInstance.Log($"!!!!!!!!!!!!!!!!!!queryExecutingCard = {queryExecutingCard}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetRule.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }

        private void FillExecutingCardForQuestion(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"Key = {Key}");
            //LogInstance.Log($"IsQuestion = {IsQuestion}");
            //LogInstance.Log($"Params.Count = {Params.Count}");
            //foreach (var param in Params)
            //{
            //    LogInstance.Log($"param = {param}");
            //}
            //LogInstance.Log($"VarsInfoList.Count = {VarsInfoList.Count}");
            //foreach (var varInfo in VarsInfoList)
            //{
            //    LogInstance.Log($"varInfo = {varInfo}");
            //}
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");
#endif

            var hasAnnotations = !Annotations.IsEmpty();

#if DEBUG
            //LogInstance.Log($"hasAnnotations = {hasAnnotations}");
#endif

            var targetRelationsList = dataSource.AllRelationsForProductions;// GetAllRelations(context);

#if DEBUG
            //LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
            //foreach (var targetRelation in targetRelationsList)
            //{
            //    LogInstance.Log($"targetRelation.GetHumanizeDbgString() = {targetRelation.GetHumanizeDbgString()}");
            //}
#endif

            foreach (var targetRelation in targetRelationsList)
            {
                if(targetRelation.CountParams != CountParams)
                {
                    continue;
                }
#if DEBUG
                //LogInstance.Log($"targetRelation.GetHumanizeDbgString() = {targetRelation.GetHumanizeDbgString()}");
                //LogInstance.Log($"targetRelation = {targetRelation}");
                //LogInstance.Log($"hasAnnotations = {hasAnnotations}");
#endif
                var isCheckAnnotation = false;

                if (hasAnnotations)
                {
                    if(targetRelation.Annotations.IsEmpty())
                    {
                        continue;
                    }

                    foreach(var annotation in Annotations)
                    {
                        foreach (var annotationOfTargetRealtion in targetRelation.Annotations)
                        {
                            var annotationStorage = new AnnotationCGStorage(dataSource, annotationOfTargetRealtion);

                            var tmpCheckAnnotation = CheckAnnotation(annotation, annotationStorage, options);

                            if(tmpCheckAnnotation)
                            {
                                isCheckAnnotation = true;
                            }
                        }
                    }

#if DEBUG
                    //LogInstance.Log($"isCheckAnnotation = {isCheckAnnotation}");
#endif

                }

                if (hasAnnotations && !isCheckAnnotation)
                {
                    continue;
                }

#if DEBUG
                //LogInstance.Log($"NEXT targetRelation.GetHumanizeDbgString() = {targetRelation.GetHumanizeDbgString()}");
                //LogInstance.Log($"NEXT targetRelation = {targetRelation}");
#endif

                var resultOfQueryToRelation = new ResultOfQueryToRelation();
                queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);

                {                
                    var resultOfVarOfQueryToRelation = new ResultOfVarOfQueryToRelation();
                    resultOfVarOfQueryToRelation.KeyOfVar = Key;
                    resultOfVarOfQueryToRelation.FoundExpression = targetRelation.Origin;
                    resultOfQueryToRelation.ResultOfVarOfQueryToRelationList.Add(resultOfVarOfQueryToRelation);

                    var originInfo = new OriginOfVarOfQueryToRelation();
                    var targetRulePart = targetRelation.RulePart;
                    originInfo.IndexedRuleInstance = targetRelation.RuleInstance;
                    originInfo.IndexedRulePart = targetRulePart;

                    var keyOfRuleInstance = targetRelation.RuleInstance.Key;

                    originInfo.KeyOfRuleInstance = keyOfRuleInstance;

                    resultOfVarOfQueryToRelation.OriginDict[keyOfRuleInstance] = originInfo;
                }

                var n = 0;

                foreach (var param in Params)
                {
#if DEBUG
                    //LogInstance.Log($"n = {n} param = {param}");
#endif

                    n++;

                    if(param.Kind != KindOfExpressionNode.QuestionVar)
                    {
                        continue;
                    }

                    var foundExpression = targetRelation.Params[n - 1].Origin;

                    var questionVarParam = param.Origin.AsQuestionVar;

                    var resultOfVarOfQueryToRelation = new ResultOfVarOfQueryToRelation();
                    resultOfVarOfQueryToRelation.KeyOfVar = questionVarParam.Key;
                    resultOfVarOfQueryToRelation.FoundExpression = foundExpression;
                    resultOfQueryToRelation.ResultOfVarOfQueryToRelationList.Add(resultOfVarOfQueryToRelation);

                    var originInfo = new OriginOfVarOfQueryToRelation();
                    var targetRulePart = targetRelation.RulePart;
                    originInfo.IndexedRuleInstance = targetRelation.RuleInstance;
                    originInfo.IndexedRulePart = targetRulePart;

                    var keyOfRuleInstance = targetRelation.RuleInstance.Key;

                    originInfo.KeyOfRuleInstance = keyOfRuleInstance;

                    resultOfVarOfQueryToRelation.OriginDict[keyOfRuleInstance] = originInfo;

#if DEBUG
                    //LogInstance.Log($"resultOfVarOfQueryToRelation = {resultOfVarOfQueryToRelation}");
                    //throw new NotImplementedException();
#endif
                }

#if DEBUG
                //LogInstance.Log($"resultOfQueryToRelation = {resultOfQueryToRelation}");
                //throw new NotImplementedException();
#endif
            }
#if DEBUG
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"GetHumanizeDbgString() = {GetHumanizeDbgString()}");
            //LogInstance.Log($"this = {this}");
#endif

            //var resultOfVarOfQueryToRelation = new ResultOfVarOfQueryToRelation();
            //resultOfVarOfQueryToRelation.KeyOfVar = Key;
            //resultOfVarOfQueryToRelation.FoundExpression = paramOfTargetRelation;
            //resultOfQueryToRelation.ResultOfVarOfQueryToRelationList.Add(resultOfVarOfQueryToRelation);

            //var originInfo = new OriginOfVarOfQueryToRelation();
            //originInfo.IndexedRuleInstance = Parent;
            //originInfo.IndexedRulePart = this;

            //var keyOfRuleInstance = Parent.Key;

            //originInfo.KeyOfRuleInstance = keyOfRuleInstance;

            //resultOfVarOfQueryToRelation.OriginDict[keyOfRuleInstance] = originInfo;


#if DEBUG
            //throw new NotImplementedException();
            //LogInstance.Log("End");
#endif
        }

        private bool CheckAnnotation(IndexedLogicalAnnotation annotationOfQuery, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"annotationOfQuery = {annotationOfQuery.GetHumanizeDbgString()}");
            //LogInstance.Log($"annotationOfStored = {annotationOfStored.GetHumanizeDbgString()}");
            //LogInstance.Log($"annotationOfQuery = {annotationOfQuery}");
            //throw new NotImplementedException();
#endif

            //var strategyForGettingInfo = new StrategyForGettingInfoFromStoragesByAnnotation(context, annotationOfStored);

            var executingCardForAnnotation = new QueryExecutingCardForIndexedPersistLogicalData();

            var indexedRuleInstanceOfAnnotation = dataSource.GetIndexedAdditionalRuleInstanceByKey(annotationOfQuery.RuleInstanceKey);

            //annotationOfQuery.RuleInstance.FillExecutingCardForAnnotation(executingCardForAnnotation, dataSource);
            indexedRuleInstanceOfAnnotation.FillExecutingCardForAnnotation(executingCardForAnnotation, dataSource, options);
            //NFillExecutingCard(executingCardForAnnotation, strategyForGettingInfo);

#if DEBUG
            //LogInstance.Log($"executingCardForAnnotation = {executingCardForAnnotation}");
            //throw new NotImplementedException();
            //return false;//tmp
#endif

            if (executingCardForAnnotation.ResultsOfQueryToRelationList.IsEmpty())
            {
                return false;
            }

            return true;
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
