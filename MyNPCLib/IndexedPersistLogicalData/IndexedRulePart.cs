using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
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
                    resultOfQueryToRelation.IndexedRulePart = this;
                    resultOfQueryToRelation.IndexedRuleInstance = Parent;


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
                    }

                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }
        }

        private bool CompareKnownInfoAndExpressionNode(QueryExecutingCardAboutKnownInfo knownInfo, BaseExpressionNode expressionNode)
        {
            var knownInfoExpression = knownInfo.Expression;

            if (knownInfoExpression.IsBaseRef == expressionNode.IsBaseRef)
            {
                if(knownInfo.Key == expressionNode.AsBaseRef.Key)
                {
                    return true;
                }

                return false;
            }

            if(knownInfo.Kind == KindOfExpressionNode.Value && expressionNode.Kind == KindOfExpressionNode.Value)
            {
                if(knownInfo.Value == expressionNode.AsValue.Value)
                {
                    return true;
                }

                return true;
            }

            return true;
        }

        public void FillExecutingCardForCallingFromOtherPart(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            throw new NotImplementedException();
        }

        public void FillExecutingCardForProduction(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var queryExecutingCardForExpression = new QueryExecutingCardForIndexedPersistLogicalData();

            Expression.FillExecutingCardForProduction(queryExecutingCardForExpression, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCardForExpression = {queryExecutingCardForExpression}");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
            //throw new NotImplementedException();
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
