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

        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard)
        {
#if DEBUG
            LogInstance.Log($"ResolverOfConditionOfQueryASTNode GetEntitiesIdList mQueryNode = {mQueryNode}");
#endif
        }
    }
}
