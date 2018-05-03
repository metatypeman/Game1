using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class ResolverOfBinaryOperatorOfQueryASTNode : BaseQueryResolverASTNode
    {
        public ResolverOfBinaryOperatorOfQueryASTNode(BinaryOperatorOfQueryASTNode queryNode)
        {
            mQueryNode = queryNode;
        }

        private BinaryOperatorOfQueryASTNode mQueryNode;

        public BaseQueryResolverASTNode Right { get; set; }
        public BaseQueryResolverASTNode Left { get; set; }

        public override void FillExecutingCard(QueryExecutingCard queryExecutingCard)
        {
#if DEBUG
            LogInstance.Log($"ResolverOfBinaryOperatorOfQueryASTNode GetEntitiesIdList mQueryNode = {mQueryNode}");
#endif
        }
    }
}
