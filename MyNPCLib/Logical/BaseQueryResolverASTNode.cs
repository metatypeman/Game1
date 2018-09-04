using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseQueryResolverASTNode
    {
        public IList<ulong> GetEntitiesIdList(IOldLogicalStorage source)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var queryExecutingCard = new QueryExecutingCard();

            FillExecutingCard(queryExecutingCard, source);

#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            if (queryExecutingCard.EntitiesIdList == null)
            {
                return new List<ulong>();
            }

            return queryExecutingCard.EntitiesIdList;
        }

        public abstract void FillExecutingCard(QueryExecutingCard queryExecutingCard, IOldLogicalStorage source);
    }
}
