﻿using System;
using System.Collections.Generic;
using System.Text;
using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class ResolverForOperatorNotExpressionNode: ResolverForUnaryOperatorExpressionNode
    {
        public OperatorNotExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Not;

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log("Begin");
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

            throw new NotImplementedException();
        }
    }
}
