using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class ResolverOfUnaryOperatorOfQueryASTNode: BaseQueryResolverASTNode
    {
        public BaseQueryResolverASTNode Left { get; set; }
    }
}
