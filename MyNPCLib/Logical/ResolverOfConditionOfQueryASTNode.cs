using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfConditionOfQueryASTNode: BaseQueryResolverASTNode
    {
        public ResolverOfConditionOfQueryASTNode(ConditionOfQueryASTNode queryNode)
        {
            mQueryNode = queryNode;
        }

        private ConditionOfQueryASTNode mQueryNode;

        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard, IOldLogicalStorage source)
        {
#if DEBUG
            LogInstance.Log($"mQueryNode = {mQueryNode}");
#endif

            var entitiesIdsList = source.GetEntitiesIdsList(mQueryNode.PropertyId, mQueryNode.Value);

            queryExecutingCard.EntitiesIdList = entitiesIdsList;
        }
    }
}
