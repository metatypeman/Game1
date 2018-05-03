using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class ResolverOfBinaryOperatorOfQueryASTNode : BaseQueryResolverASTNode
    {
        public BaseQueryResolverASTNode Right { get; set; }
        public BaseQueryResolverASTNode Left { get; set; }
    }
}
