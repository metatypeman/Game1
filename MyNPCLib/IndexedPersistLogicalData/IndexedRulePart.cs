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
    public class IndexedRulePart : IObjectToString, IShortObjectToString
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

        public void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = senderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            Expression.FillExecutingCard(queryExecutingCardForExpression, context);

            foreach (var resultOfQueryToRelation in queryExecutingCardForExpression.ResultsOfQueryToRelationList)
            {
                queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
            }
        }

        public void FillExecutingCardForCallingFromRelationForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

            foreach (var targetRelation in targetRelationsList)
            {
                if (targetRelation.CountParams != queryExecutingCard.CountParams)
                {
                    continue;
                }

                var paramsListOfTargetRelation = targetRelation.ConcreteOrigin.Params;

                var isFit = true;

                foreach (var knownInfo in queryExecutingCard.KnownInfoList)
                {
                    var position = knownInfo.Position;

                    if(position.HasValue)
                    {
                        var paramOfTargetRelation = paramsListOfTargetRelation[position.Value];
                        var resultOfComparison = CompareKnownInfoAndExpressionNode(knownInfo, paramOfTargetRelation);

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

                if (isFit)
                {
                    var resultOfQueryToRelation = new ResultOfQueryToRelation();

                    foreach (var varItem in queryExecutingCard.VarsInfoList)
                    {
                        var paramOfTargetRelation = paramsListOfTargetRelation[varItem.Position];

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

                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }
        }

        private bool CompareKnownInfoAndExpressionNode(QueryExecutingCardAboutKnownInfo knownInfo, BaseExpressionNode expressionNode)
        {
            var knownInfoExpression = knownInfo.Expression;

            return ExpressionNodeHelper.Compare(knownInfoExpression, expressionNode);
        }

        public void FillExecutingCardForCallingFromRelationForProduction(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;
            
            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

            if(targetRelationsList.Count != 1)
            {
                return;
            }

            var targetRelation = targetRelationsList.First();

            if(targetRelation.Params.Count != queryExecutingCard.CountParams)
            {
                return;
            }

            var targetRelationVarsInfoList = targetRelation.VarsInfoList;

            var targetRelationVarsInfoDictByPosition = targetRelationVarsInfoList.ToDictionary(p => p.Position, p => p.KeyOfVar);

            var mergingResult = QueryExecutingCardAboutKnownInfoHelper.Merge(targetRelation.KnownInfoList, targetRelationVarsInfoList, queryExecutingCard.KnownInfoList, true);
            if(!mergingResult.IsSuccess)
            {
                return;
            }

            var targetKnownInfoList = mergingResult.KnownInfoList;

            var queryExecutingCardForNextPart = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForNextPart.VarsInfoList = targetRelation.VarsInfoList;
            queryExecutingCardForNextPart.KnownInfoList = targetKnownInfoList;
            queryExecutingCardForNextPart.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForNextPart.SenderIndexedRulePart = this;
            NextPart.FillExecutingCardForCallingFromOtherPart(queryExecutingCardForNextPart, context);

            var resultsOfQueryToRelationList = queryExecutingCardForNextPart.ResultsOfQueryToRelationList;

            if (resultsOfQueryToRelationList.Count > 0)
            {
                var varsInfoList = queryExecutingCard.VarsInfoList;

                var backKeysDict = new Dictionary<ulong, ulong>();

                foreach (var varInfo in varsInfoList)
                {
                    var targetInternalKeyOfVar = targetRelationVarsInfoDictByPosition[varInfo.Position];
                    backKeysDict[targetInternalKeyOfVar] = varInfo.KeyOfVar;
                }

                foreach (var resultOfQueryToRelation in resultsOfQueryToRelationList)
                {
                    var newResultOfQueryToRelation = new ResultOfQueryToRelation();
                    var newResultOfVarOfQueryToRelationList = new List<ResultOfVarOfQueryToRelation>();

                    foreach (var resultOfVarOfQueryToRelation in resultOfQueryToRelation.ResultOfVarOfQueryToRelationList)
                    {
                        var internalKeyOfVar = resultOfVarOfQueryToRelation.KeyOfVar;
                        if (backKeysDict.ContainsKey(internalKeyOfVar))
                        {
                            var externalKeyOfVar = backKeysDict[internalKeyOfVar];
                            resultOfVarOfQueryToRelation.KeyOfVar = externalKeyOfVar;
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
        }

        public void FillExecutingCardForCallingFromOtherPart(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            queryExecutingCardForExpression.KnownInfoList = queryExecutingCard.KnownInfoList;
            Expression.FillExecutingCard(queryExecutingCardForExpression, context);

            queryExecutingCard.ResultsOfQueryToRelationList = queryExecutingCardForExpression.ResultsOfQueryToRelationList;         
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
            return sb.ToString();
        }
    }
}
