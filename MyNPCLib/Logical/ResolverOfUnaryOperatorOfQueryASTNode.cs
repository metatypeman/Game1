using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfUnaryOperatorOfQueryASTNode: BaseQueryResolverASTNode
    {
        public ResolverOfUnaryOperatorOfQueryASTNode(UnaryOperatorOfQueryASTNode queryNode)
        {
            mQueryNode = queryNode;
        }

        private UnaryOperatorOfQueryASTNode mQueryNode;

        public BaseQueryResolverASTNode Left { get; set; }

        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard)
        {
#if DEBUG
            LogInstance.Log($"ResolverOfUnaryOperatorOfQueryASTNode GetEntitiesIdList mQueryNode = {mQueryNode}");
#endif
        }
    }
}
