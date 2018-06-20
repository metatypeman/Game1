using MyNPCLib.CGStorage;
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
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = senderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            Expression.FillExecutingCard(queryExecutingCardForExpression, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        [Obsolete]
        public void FillExecutingCardForFacts(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();

            Expression.FillExecutingCardForFact(queryExecutingCardForExpression, source, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

            foreach (var resultOfQueryToRelation in queryExecutingCardForExpression.ResultsOfQueryToRelationList)
            {
                queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
            }
        }

        [Obsolete]
        public void FillExecutingCardForCallingFromRelationForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

#if DEBUG
            LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            foreach(var targetRelation in targetRelationsList)
            {
                if(targetRelation.CountParams != queryExecutingCard.CountParams)
                {
                    continue;
                }
#if DEBUG
                LogInstance.Log($"targetRelation = {targetRelation}");
#endif

                var paramsListOfTargetRelation = targetRelation.ConcreteOrigin.Params;

                var isFit = true;

                foreach (var knownInfo in queryExecutingCard.KnownInfoList)
                {
#if DEBUG
                    LogInstance.Log($"knownInfo = {knownInfo}");
#endif

                    var paramOfTargetRelation = paramsListOfTargetRelation[knownInfo.Position];

#if DEBUG
                    LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
#endif

                    var resultOfComparison = CompareKnownInfoAndExpressionNode(knownInfo, paramOfTargetRelation);

#if DEBUG
                    LogInstance.Log($"resultOfComparison = {resultOfComparison}");
#endif

                    if(!resultOfComparison)
                    {
                        isFit = false;
                        break;
                    }
                }

#if DEBUG
                LogInstance.Log($"isFit = {isFit}");
#endif

                if(isFit)
                {
                    var resultOfQueryToRelation = new ResultOfQueryToRelation();

                    foreach (var varItem in queryExecutingCard.VarsInfoList)
                    {
#if DEBUG
                        LogInstance.Log($"varItem = {varItem}");
#endif

                        var paramOfTargetRelation = paramsListOfTargetRelation[varItem.Position];

#if DEBUG
                        LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
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

                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }
        }

        public void FillExecutingCardForCallingFromRelationForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

#if DEBUG
            LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            foreach (var targetRelation in targetRelationsList)
            {
                if (targetRelation.CountParams != queryExecutingCard.CountParams)
                {
                    continue;
                }
#if DEBUG
                LogInstance.Log($"targetRelation = {targetRelation}");
#endif

                var paramsListOfTargetRelation = targetRelation.ConcreteOrigin.Params;

                var isFit = true;

                foreach (var knownInfo in queryExecutingCard.KnownInfoList)
                {
#if DEBUG
                    LogInstance.Log($"knownInfo = {knownInfo}");
#endif

                    var paramOfTargetRelation = paramsListOfTargetRelation[knownInfo.Position];

#if DEBUG
                    LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
#endif

                    var resultOfComparison = CompareKnownInfoAndExpressionNode(knownInfo, paramOfTargetRelation);

#if DEBUG
                    LogInstance.Log($"resultOfComparison = {resultOfComparison}");
#endif

                    if (!resultOfComparison)
                    {
                        isFit = false;
                        break;
                    }
                }

#if DEBUG
                LogInstance.Log($"isFit = {isFit}");
#endif

                if (isFit)
                {
                    var resultOfQueryToRelation = new ResultOfQueryToRelation();

                    foreach (var varItem in queryExecutingCard.VarsInfoList)
                    {
#if DEBUG
                        LogInstance.Log($"varItem = {varItem}");
#endif

                        var paramOfTargetRelation = paramsListOfTargetRelation[varItem.Position];

#if DEBUG
                        LogInstance.Log($"paramOfTargetRelation = {paramOfTargetRelation}");
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
#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;
            
            var targetRelationsList = RelationsDict[queryExecutingCard.TargetRelation];

#if DEBUG
            LogInstance.Log($"targetRelationsList.Count = {targetRelationsList.Count}");
#endif

            if(targetRelationsList.Count != 1)
            {
                return;
            }

            var targetRelation = targetRelationsList.First();

#if DEBUG
            LogInstance.Log($"targetRelation = {targetRelation}");
#endif

            if(targetRelation.Params.Count != queryExecutingCard.CountParams)
            {
                return;
            }

#if DEBUG
            LogInstance.Log($"targetRelation.VarsInfoList.Count = {targetRelation.VarsInfoList.Count}");
            foreach(var varInfo in targetRelation.VarsInfoList)
            {
                LogInstance.Log($"varInfo = {varInfo}");
            }
#endif

            var queryExecutingCardForNextPart = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForNextPart.VarsInfoList = targetRelation.VarsInfoList;
            queryExecutingCardForNextPart.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForNextPart.SenderIndexedRulePart = this;
            NextPart.FillExecutingCardForCallingFromOtherPart(queryExecutingCardForNextPart, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCardForNextPart = {queryExecutingCardForNextPart}");
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
            LogInstance.Log($"queryExecutingCardForNextPart.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderExpressionNodeHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCardForNextPart.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderIndexedRulePartHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCardForNextPart.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCardForNextPart.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString() = {queryExecutingCard.GetSenderExpressionNodeHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRulePartHumanizeDbgString()}");
            LogInstance.Log($"queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString() = {queryExecutingCard.GetSenderIndexedRuleInstanceHumanizeDbgString()}");
#endif

            throw new NotImplementedException();

#if DEBUG
            LogInstance.Log("End");
#endif           
        }

        public void FillExecutingCardForCallingFromOtherPart(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExecutingCardForExpression.SenderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            queryExecutingCardForExpression.SenderIndexedRulePart = this;
            Expression.FillExecutingCard(queryExecutingCardForExpression, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif
            queryExecutingCard.ResultsOfQueryToRelationList = queryExecutingCardForExpression.ResultsOfQueryToRelationList;

#if DEBUG
            LogInstance.Log("End");
#endif         
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
