using System;
using System.Collections.Generic;
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
        }
    }
}
