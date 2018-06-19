using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
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

            var rightQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            Right.FillExecutingCard(rightQueryExecutingCard, context);

#if DEBUG
            LogInstance.Log($"rightQueryExecutingCard = {rightQueryExecutingCard}");
            LogInstance.Log($"leftQueryExecutingCard.ToBriefString() = {leftQueryExecutingCard.ToBriefString()}");
            LogInstance.Log($"rightQueryExecutingCard.ToBriefString() = {rightQueryExecutingCard.ToBriefString()}");
#endif

            var resultsOfQueryToRelationList = new List<ResultOfQueryToRelation>();
            var leftQueryExecutingCardResultsOfQueryToRelationList = leftQueryExecutingCard.ResultsOfQueryToRelationList;

            if (leftQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
            {
                return;
            }

            var rightQueryExecutingCardResultsOfQueryToRelationList = rightQueryExecutingCard.ResultsOfQueryToRelationList;

            if(rightQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
            {
                return;
            }

            foreach (var leftResultOfQueryToRelation in leftQueryExecutingCardResultsOfQueryToRelationList)
            {
#if DEBUG
                LogInstance.Log($"leftResultOfQueryToRelation.ToBriefString() = {leftResultOfQueryToRelation.ToBriefString()}");
#endif

                foreach (var rightResultOfQueryToRelation in rightQueryExecutingCard.ResultsOfQueryToRelationList)
                {
#if DEBUG
                    LogInstance.Log($"rightResultOfQueryToRelation.ToBriefString() = {rightResultOfQueryToRelation.ToBriefString()}");
#endif
                }
            }

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
