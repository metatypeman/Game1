using System;
using System.Collections.Generic;
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
        }
    }
}
