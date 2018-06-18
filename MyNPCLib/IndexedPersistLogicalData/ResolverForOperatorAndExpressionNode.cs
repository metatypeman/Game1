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
            LogInstance.Log("Begin");
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
#endif

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
