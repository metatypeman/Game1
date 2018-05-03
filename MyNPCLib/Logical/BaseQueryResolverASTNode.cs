using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseQueryResolverASTNode
    {
        public IList<ulong> GetEntitiesIdList(ILogicalStorage source)
        {
#if DEBUG
            LogInstance.Log("BaseQueryResolverASTNode GetEntitiesIdList");
#endif

            var queryExecutingCard = new QueryExecutingCard();

            FillExecutingCard(queryExecutingCard, source);

#if DEBUG
            LogInstance.Log($"BaseQueryResolverASTNode GetEntitiesIdList queryExecutingCard = {queryExecutingCard}");
#endif

            if (queryExecutingCard.EntitiesIdList == null)
            {
                return new List<ulong>();
            }

            return queryExecutingCard.EntitiesIdList;
        }

        public abstract void FillExecutingCard(QueryExecutingCard queryExecutingCard, ILogicalStorage source);
    }
}
