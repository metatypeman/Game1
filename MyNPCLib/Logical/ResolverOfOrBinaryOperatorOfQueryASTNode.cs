using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfOrBinaryOperatorOfQueryASTNode: ResolverOfBinaryOperatorOfQueryASTNode
    {
        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard, ILogicalStorage source)
        {
#if DEBUG
            LogInstance.Log("ResolverOfOrBinaryOperatorOfQueryASTNode FillExecutingCard");
#endif

            var leftExecutionCard = new QueryExecutingCard();

            Left.FillExecutingCard(leftExecutionCard, source);

#if DEBUG
            LogInstance.Log($"ResolverOfOrBinaryOperatorOfQueryASTNode FillExecutingCard leftExecutionCard = {leftExecutionCard}");
#endif

            var rightExecutionCard = new QueryExecutingCard();

            Right.FillExecutingCard(rightExecutionCard, source);

#if DEBUG
            LogInstance.Log($"ResolverOfOrBinaryOperatorOfQueryASTNode FillExecutingCard rightExecutionCard = {rightExecutionCard}");
#endif

            var result = leftExecutionCard.EntitiesIdList.Concat(rightExecutionCard.EntitiesIdList).Distinct().ToList();

            queryExecutingCard.EntitiesIdList = result;

#if DEBUG
            LogInstance.Log("ResolverOfOrBinaryOperatorOfQueryASTNode FillExecutingCard NEXT");
#endif
        }
    }
}
