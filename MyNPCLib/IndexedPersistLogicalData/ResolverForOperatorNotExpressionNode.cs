using System;
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

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        public override void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            throw new NotImplementedException();
            //LogInstance.Log("End");
#endif
        }
    }
}
