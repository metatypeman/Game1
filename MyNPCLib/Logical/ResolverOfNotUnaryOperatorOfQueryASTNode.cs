using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfNotUnaryOperatorOfQueryASTNode: ResolverOfUnaryOperatorOfQueryASTNode
    {
        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard, ILogicalStorage source)
        {
#if DEBUG
            LogInstance.Log("ResolverOfNotUnaryOperatorOfQueryASTNode FillExecutingCard");
#endif

            var leftExecutionCard = new QueryExecutingCard();

            Left.FillExecutingCard(leftExecutionCard, source);

#if DEBUG
            LogInstance.Log($"ResolverOfNotUnaryOperatorOfQueryASTNode FillExecutingCard leftExecutionCard = {leftExecutionCard}");
#endif

            var sourceEntitiesIdList = leftExecutionCard.EntitiesIdList;

            var allEntitiesIdList = source.GetAllEntitiesIdsList();

            var result = allEntitiesIdList.Where(p => !sourceEntitiesIdList.Contains(p)).ToList();

            queryExecutingCard.EntitiesIdList = result;

#if DEBUG
            LogInstance.Log("ResolverOfNotUnaryOperatorOfQueryASTNode FillExecutingCard NEXT");
#endif
        }
    }
}
