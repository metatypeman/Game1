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
    public class ResolverForOperatorAndExpressionNode: ResolverForBinaryOperatorExpressionNode
    {
        public OperatorAndExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.And;

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            var leftQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            Left.FillExecutingCard(leftQueryExecutingCard, context);

#if DEBUG
            LogInstance.Log($"leftQueryExecutingCard = {leftQueryExecutingCard}");
#endif

            var leftQueryExecutingCardResultsOfQueryToRelationList = leftQueryExecutingCard.ResultsOfQueryToRelationList;

            if (leftQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
            {
                return;
            }

            var resultsOfQueryToRelationList = new List<ResultOfQueryToRelation>();

            foreach (var leftResultOfQueryToRelation in leftQueryExecutingCardResultsOfQueryToRelationList)
            {
                var rightQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
                Right.FillExecutingCard(rightQueryExecutingCard, context);

#if DEBUG
                LogInstance.Log($"rightQueryExecutingCard = {rightQueryExecutingCard}");
                LogInstance.Log($"leftQueryExecutingCard.ToBriefString() = {leftQueryExecutingCard.ToBriefString()}");
                LogInstance.Log($"rightQueryExecutingCard.ToBriefString() = {rightQueryExecutingCard.ToBriefString()}");
#endif

                var rightQueryExecutingCardResultsOfQueryToRelationList = rightQueryExecutingCard.ResultsOfQueryToRelationList;

                if (rightQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
                {
                    continue;
                }

                var leftVarsList = leftResultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                var leftVarsKeysList = leftVarsList.Select(p => p.KeyOfVar).Distinct().ToList();

                foreach (var rightResultOfQueryToRelation in rightQueryExecutingCardResultsOfQueryToRelationList)
                {
#if DEBUG
                    LogInstance.Log($"rightResultOfQueryToRelation.ToBriefString() = {rightResultOfQueryToRelation.ToBriefString()}");
#endif

                    var rightVarsList = rightResultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                    var rightVarsKeysList = rightVarsList.Select(p => p.KeyOfVar).Distinct().ToList();

#if DEBUG
                    LogInstance.Log($"leftVarsKeysList.Count = {leftVarsKeysList.Count}");
                    foreach(var leftVarKey in leftVarsKeysList)
                    {
                        LogInstance.Log($"leftVarKey = {leftVarKey}");
                    }
                    
                    LogInstance.Log($"rightVarsKeysList.Count = {rightVarsKeysList.Count}");
                    foreach(var rightVarKey in rightVarsKeysList)
                    {
                        LogInstance.Log($"rightVarKey = {rightVarKey}");
                    }
#endif

                    var intersectOfVarsKeysList = leftVarsKeysList.Intersect(rightVarsKeysList).ToList();

#if DEBUG
                    LogInstance.Log($"intersectOfVarsKeysList.Count = {intersectOfVarsKeysList.Count}");
                    foreach (var intersectOfVarKey in intersectOfVarsKeysList)
                    {
                        LogInstance.Log($"intersectOfVarKey = {intersectOfVarKey}");
                    }
#endif

                    if(intersectOfVarsKeysList.Count == 0)
                    {
                        var resultItem = new ResultOfQueryToRelation();
                        foreach(var varItem in leftVarsList)
                        {
                            resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                        }

                        foreach(var varItem in rightVarsList)
                        {
                            resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                        }

                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultItem);
                    }
                    else
                    {
                        var resultItem = new ResultOfQueryToRelation();
                        foreach (var varItem in leftVarsList)
                        {
                            if(!intersectOfVarsKeysList.Contains(varItem.KeyOfVar))
                            {
                                resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                                continue;
                            }
                        }

                        foreach (var varItem in rightVarsList)
                        {
                            if (!intersectOfVarsKeysList.Contains(varItem.KeyOfVar))
                            {
                                resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                                continue;
                            }
                        }

#if DEBUG
                        LogInstance.Log($"resultItem = {resultItem}");
#endif
                    }
                }
            }

#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            throw new NotImplementedException();
#if DEBUG
            LogInstance.Log("End");
#endif
        }

        [Obsolete]
        public override void FillExecutingCardForFact(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            throw new NotImplementedException();
        }

        [Obsolete]
        public override void FillExecutingCardForProduction(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var leftQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            Left.FillExecutingCardForProduction(leftQueryExecutingCard, context);

#if DEBUG
            LogInstance.Log($"leftQueryExecutingCard = {leftQueryExecutingCard}");
#endif

            var rightQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            Right.FillExecutingCardForProduction(rightQueryExecutingCard, context);

#if DEBUG
            LogInstance.Log($"rightQueryExecutingCard = {rightQueryExecutingCard}");
#endif

            throw new NotImplementedException();
        }
    }
}
