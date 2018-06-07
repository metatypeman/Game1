using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfAndBinaryOperatorOfQueryASTNode: ResolverOfBinaryOperatorOfQueryASTNode
    {
        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard, ILogicalStorage source)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var leftExecutionCard = new QueryExecutingCard();

            Left.FillExecutingCard(leftExecutionCard, source);

#if DEBUG
            LogInstance.Log($"leftExecutionCard = {leftExecutionCard}");
#endif

            var rightExecutionCard = new QueryExecutingCard();

            Right.FillExecutingCard(rightExecutionCard, source);

#if DEBUG
            LogInstance.Log($"rightExecutionCard = {rightExecutionCard}");
#endif

            var result = leftExecutionCard.EntitiesIdList.Intersect(rightExecutionCard.EntitiesIdList).ToList();

            queryExecutingCard.EntitiesIdList = result;

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
