using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedRulePart : IIndexedLogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public RulePart Origin { get; set; }
        public bool IsActive { get; set; }
        public bool HasVars { get; set; }
        public bool HasQuestionVars { get; set; }
        public IndexedRuleInstance Parent { get; set; }
        public IndexedRulePart NextPart { get; set; }
        public IndexedVariablesQuantificationPart VariablesQuantification { get; set; }
        public ResolverForBaseExpressionNode Expression { get; set; }
        public IDictionary<ulong, IList<ResolverForRelationExpressionNode>> RelationsDict { get; set; }
        public IList<IndexedLogicalAnnotation> Annotations { get; set; }

        public void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"Begin~~~~~~ GetHumanizeDbgString() = {GetHumanizeDbgString()}");
#endif
            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = senderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            Expression.FillExecutingCard(queryExecutingCardForExpression, dataSource, options);

#if DEBUG
            //LogInstance.Log($"#$%^$%^^ queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

            foreach (var resultOfQueryToRelation in queryExecutingCardForExpression.ResultsOfQueryToRelationList)
            {
                queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }

        public void FillExecutingCardForCallingFromRelationForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

#if DEBUG
            //LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            foreach (var targetRelation in targetRelationsList)
            {
                if (targetRelation.CountParams != queryExecutingCard.CountParams)
                {
                    continue;
                }

#if DEBUG
                //LogInstance.Log($"targetRelation = {targetRelation}");
#endif

                var paramsListOfTargetRelation = targetRelation.ConcreteOrigin.Params;

                var isFit = true;

                foreach (var knownInfo in queryExecutingCard.KnownInfoList)
                {
#if DEBUG
                    //LogInstance.Log($"knownInfo = {knownInfo}");
#endif

                    var position = knownInfo.Position;

                    if(position.HasValue)
                    {
                        var paramOfTargetRelation = paramsListOfTargetRelation[position.Value];

#if DEBUG
                        //LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
#endif

                        var resultOfComparison = CompareKnownInfoAndExpressionNode(knownInfo, paramOfTargetRelation);

#if DEBUG
                        //LogInstance.Log($"resultOfComparison = {resultOfComparison}");
#endif

                        if (!resultOfComparison)
                        {
                            isFit = false;
                            break;
                        }
                    }
                    else
                    {
                        isFit = false;
                        break;
                    }
                }

#if DEBUG
                //LogInstance.Log($"isFit = {isFit}");
#endif

                if (isFit)
                {
                    var resultOfQueryToRelation = new ResultOfQueryToRelation();

                    var isEntityIdOnly = options.EntityIdOnly;
                    var useAccessPolicy = options.UseAccessPolicy;

                    if (useAccessPolicy)
                    {
                        foreach (var accessPolicy in options.AccessPolicyToFactModalityList)
                        {
#if DEBUG
                            //LogInstance.Log($"accessPolicy = {accessPolicy}");
#endif

                            if (!Parent.AccessPolicyToFactModality.Any(p => p.Kind == accessPolicy.Kind))
                            {
                                return;
                            }
                        }
                    }

                    foreach (var varItem in queryExecutingCard.VarsInfoList)
                    {
#if DEBUG
                        //LogInstance.Log($"varItem = {varItem}");
#endif

                        var paramOfTargetRelation = paramsListOfTargetRelation[varItem.Position];

#if DEBUG
                        //LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
#endif

                        if(isEntityIdOnly && !paramOfTargetRelation.IsEntityRef)
                        {
                            continue;
                        }

#if DEBUG
                        //LogInstance.Log($"NEXT paramOfTargetRelation = {paramOfTargetRelation}");
#endif

                        var resultOfVarOfQueryToRelation = new ResultOfVarOfQueryToRelation();
                        resultOfVarOfQueryToRelation.KeyOfVar = varItem.KeyOfVar;
                        resultOfVarOfQueryToRelation.FoundExpression = paramOfTargetRelation;
                        resultOfQueryToRelation.ResultOfVarOfQueryToRelationList.Add(resultOfVarOfQueryToRelation);

                        var originInfo = new OriginOfVarOfQueryToRelation();
                        originInfo.IndexedRuleInstance = Parent;
                        originInfo.IndexedRulePart = this;

                        var keyOfRuleInstance = Parent.Key;

                        originInfo.KeyOfRuleInstance = keyOfRuleInstance;

                        resultOfVarOfQueryToRelation.OriginDict[keyOfRuleInstance] = originInfo;
                    }

                    if(resultOfQueryToRelation.ResultOfVarOfQueryToRelationList.Count == 0)
                    {
                        continue;
                    }

                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }
        }

        private bool CompareKnownInfoAndExpressionNode(QueryExecutingCardAboutKnownInfo knownInfo, BaseExpressionNode expressionNode)
        {
            var knownInfoExpression = knownInfo.Expression;

            return ExpressionNodeHelper.Compare(knownInfoExpression, expressionNode);
        }

        public void FillExecutingCardForCallingFromRelationForProduction(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;
            
            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

#if DEBUG
            //LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            if (targetRelationsList.Count != 1)
            {
                return;
            }

            var targetRelation = targetRelationsList.First();

#if DEBUG
            //LogInstance.Log($"targetRelation = {targetRelation}");
#endif

            if (targetRelation.Params.Count != queryExecutingCard.CountParams)
            {
                return;
            }

            var targetRelationVarsInfoList = targetRelation.VarsInfoList;

#if DEBUG
            //LogInstance.Log($"targetRelationVarsInfoList.Count = {targetRelationVarsInfoList.Count}");
            //foreach (var varInfo in targetRelationVarsInfoList)
            //{
            //    LogInstance.Log($"varInfo = {varInfo}");
            //}
#endif

            var targetRelationVarsInfoDictByPosition = targetRelationVarsInfoList.ToDictionary(p => p.Position, p => p.KeyOfVar);

            var mergingResult = QueryExecutingCardAboutKnownInfoHelper.Merge(targetRelation.KnownInfoList, targetRelationVarsInfoList, queryExecutingCard.KnownInfoList, true);
            if(!mergingResult.IsSuccess)
            {
                return;
            }

            var targetKnownInfoList = mergingResult.KnownInfoList;

#if DEBUG
            //LogInstance.Log($"########################targetKnownInfoList.Count = {targetKnownInfoList.Count}");
            //foreach (var tmpKnownInfo in targetKnownInfoList)
            //{
            //    LogInstance.Log($"tmpKnownInfo = {tmpKnownInfo}");
            //}
#endif

            var queryExecutingCardForNextPart = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForNextPart.VarsInfoList = targetRelation.VarsInfoList;
            queryExecutingCardForNextPart.KnownInfoList = targetKnownInfoList;
            queryExecutingCardForNextPart.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForNextPart.SenderIndexedRulePart = this;
            NextPart.FillExecutingCardForCallingFromOtherPart(queryExecutingCardForNextPart, dataSource, options);

#if DEBUG
            //LogInstance.Log($"queryExecutingCardForNextPart = {queryExecutingCardForNextPart}");
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            //LogInstance.Log($"queryExecutingCardForNextPart.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCardForNextPart.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCardForNextPart.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            //LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
#endif

            var resultsOfQueryToRelationList = queryExecutingCardForNextPart.ResultsOfQueryToRelationList;

            if (resultsOfQueryToRelationList.Count > 0)
            {
                var varsInfoList = queryExecutingCard.VarsInfoList;

                var backKeysDict = new Dictionary<ulong, ulong>();

                foreach (var varInfo in varsInfoList)
                {
#if DEBUG
                    //LogInstance.Log($"varInfo = {varInfo}");
#endif

                    var targetInternalKeyOfVar = targetRelationVarsInfoDictByPosition[varInfo.Position];

#if DEBUG
                    //LogInstance.Log($"targetInternalKeyOfVar = {targetInternalKeyOfVar}");
#endif

                    backKeysDict[targetInternalKeyOfVar] = varInfo.KeyOfVar;
                }

                foreach (var resultOfQueryToRelation in resultsOfQueryToRelationList)
                {
                    var newResultOfQueryToRelation = new ResultOfQueryToRelation();
                    var newResultOfVarOfQueryToRelationList = new List<ResultOfVarOfQueryToRelation>();

                    foreach (var resultOfVarOfQueryToRelation in resultOfQueryToRelation.ResultOfVarOfQueryToRelationList)
                    {
#if DEBUG
                        //LogInstance.Log($"resultOfQueryToRelation = {resultOfQueryToRelation}");
#endif

                        var internalKeyOfVar = resultOfVarOfQueryToRelation.KeyOfVar;

#if DEBUG
                        //LogInstance.Log($"internalKeyOfVar = {internalKeyOfVar}/'{context.EntityDictionary.GetName(internalKeyOfVar)}'");

#endif

                        if (backKeysDict.ContainsKey(internalKeyOfVar))
                        {
                            var externalKeyOfVar = backKeysDict[internalKeyOfVar];

#if DEBUG
                            //LogInstance.Log($"externalKeyOfVar = {externalKeyOfVar}/'{context.EntityDictionary.GetName(externalKeyOfVar)}'");
                            //LogInstance.Log($"resultOfVarOfQueryToRelation before = {resultOfVarOfQueryToRelation}");
#endif

                            resultOfVarOfQueryToRelation.KeyOfVar = externalKeyOfVar;

#if DEBUG
                            //LogInstance.Log($"resultOfVarOfQueryToRelation after = {resultOfVarOfQueryToRelation}");
#endif

                            newResultOfVarOfQueryToRelationList.Add(resultOfVarOfQueryToRelation);
                        }
                    }

                    if(newResultOfVarOfQueryToRelationList.Count == 0)
                    {
                        continue;
                    }

                    newResultOfQueryToRelation.ResultOfVarOfQueryToRelationList = newResultOfVarOfQueryToRelationList;
                    queryExecutingCard.ResultsOfQueryToRelationList.Add(newResultOfQueryToRelation);
                }
            }

#if DEBUG
            //LogInstance.Log($"+++++++++queryExecutingCard = {queryExecutingCard}");
#endif
#if DEBUG
            //LogInstance.Log("End");
#endif 
        }

        public void FillExecutingCardForCallingFromOtherPart(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log("Begin ^&*^&*");
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif
            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            queryExecutingCardForExpression.KnownInfoList = queryExecutingCard.KnownInfoList;
            Expression.FillExecutingCard(queryExecutingCardForExpression, dataSource, options);

#if DEBUG
            //LogInstance.Log($"%%%%%%%% queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

            queryExecutingCard.ResultsOfQueryToRelationList = queryExecutingCardForExpression.ResultsOfQueryToRelationList;

#if DEBUG
            //LogInstance.Log("End");
#endif
        }

        public void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = senderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            Expression.FillExecutingCardForAnnotation(queryExecutingCardForExpression, dataSource, options);

#if DEBUG
            //LogInstance.Log($"queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

            foreach (var resultOfQueryToRelation in queryExecutingCardForExpression.ResultsOfQueryToRelationList)
            {
                queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
            }
#if DEBUG
            //throw new NotImplementedException();
            //LogInstance.Log("End");
#endif
        }

        public string GetHumanizeDbgString()
        {
            if (Origin == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(Origin);
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpace = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsActive)} = {IsActive}");
            sb.AppendLine($"{spaces}{nameof(HasVars)} = {HasVars}");
            sb.AppendLine($"{spaces}{nameof(HasQuestionVars)} = {HasQuestionVars}");

            if (Parent == null)
            {
                sb.AppendLine($"{spaces}{nameof(Parent)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Parent)}");
                sb.Append(Parent.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
            }

            if (NextPart == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextPart)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextPart)}");
                sb.Append(NextPart.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NextPart)}");
            }

            if (VariablesQuantification == null)
            {
                sb.AppendLine($"{spaces}{nameof(VariablesQuantification)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VariablesQuantification)}");
                sb.Append(VariablesQuantification.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VariablesQuantification)}");
            }

            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
            }
            if (RelationsDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(RelationsDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RelationsDict)}");
                var nextNextN = nextN + 4;
                foreach (var relationsKVPItem in RelationsDict)
                {
                    sb.AppendLine($"{nextNSpace}key of relation = {relationsKVPItem.Key}");
                    var tmpRelationsList = relationsKVPItem.Value;
                    sb.AppendLine($"{nextNSpace}count of relations = {tmpRelationsList.Count}");
                    foreach(var relation in tmpRelationsList)
                    {
                        sb.Append(relation.ToShortString(nextNextN));
                    }
                }
                sb.AppendLine($"{spaces}End {nameof(RelationsDict)}");
            }

            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpace = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }

            sb.AppendLine($"{spaces}{nameof(IsActive)} = {IsActive}");
            sb.AppendLine($"{spaces}{nameof(HasVars)} = {HasVars}");
            sb.AppendLine($"{spaces}{nameof(HasQuestionVars)} = {HasQuestionVars}");

            if (VariablesQuantification == null)
            {
                sb.AppendLine($"{spaces}{nameof(VariablesQuantification)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VariablesQuantification)}");
                sb.Append(VariablesQuantification.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VariablesQuantification)}");
            }

            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
            }

            if (RelationsDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(RelationsDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RelationsDict)}");
                var nextNextN = nextN + 4;
                foreach (var relationsKVPItem in RelationsDict)
                {
                    sb.AppendLine($"{nextNSpace}key of relation = {relationsKVPItem.Key}");
                    var tmpRelationsList = relationsKVPItem.Value;
                    sb.AppendLine($"{nextNSpace}count of relations = {tmpRelationsList.Count}");
                    foreach (var relation in tmpRelationsList)
                    {
                        sb.Append(relation.ToShortString(nextNextN));
                    }
                }
                sb.AppendLine($"{spaces}End {nameof(RelationsDict)}");
            }

            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }
    }
}
