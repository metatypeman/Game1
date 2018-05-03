using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public static class QueryResolverASTNodeFactory
    {
        public static BaseQueryResolverASTNode CreatePlan(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryResolverASTNodeFactory CreatePlan queryNode = {queryNode}");
#endif

            return CreateNode(queryNode);
        }

        private static BaseQueryResolverASTNode CreateNode(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryResolverASTNodeFactory CreateNode queryNode = {queryNode}");
#endif

            var kind = queryNode.Kind;

            switch (kind)
            {
                case QueryASTNodeKind.Condition:
                    return CreateResolverOfConditionOfQueryASTNode(queryNode.AsCondition);

                case QueryASTNodeKind.BinaryOperator:
                    return CreateResolverOfBinaryOperatorOfQueryASTNode(queryNode.AsBinaryOperator);

                case QueryASTNodeKind.UnaryOperator:
                    return CreateResolverOfUnaryOperatorOfQueryASTNode(queryNode.AsUnaryOperator);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            throw new NotImplementedException();
        }

        private static BaseQueryResolverASTNode CreateResolverOfConditionOfQueryASTNode(ConditionOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryResolverASTNodeFactory CreateResolverOfConditionOfQueryASTNode queryNode = {queryNode}");
#endif

            var result = new ResolverOfConditionOfQueryASTNode(queryNode);

            return result;
        }

        private static BaseQueryResolverASTNode CreateResolverOfBinaryOperatorOfQueryASTNode(BinaryOperatorOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryResolverASTNodeFactory CreateResolverOfBinaryOperatorOfQueryASTNode queryNode = {queryNode}");
#endif

            var result = new ResolverOfBinaryOperatorOfQueryASTNode(queryNode);

            return result;
        }

        private static BaseQueryResolverASTNode CreateResolverOfUnaryOperatorOfQueryASTNode(UnaryOperatorOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryResolverASTNodeFactory CreateResolverOfUnaryOperatorOfQueryASTNode queryNode = {queryNode}");
#endif

            var result = new ResolverOfUnaryOperatorOfQueryASTNode(queryNode);

            return result;
        }
    }
}
