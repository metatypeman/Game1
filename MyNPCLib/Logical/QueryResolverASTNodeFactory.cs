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
            LogInstance.Log($"queryNode = {queryNode}");
#endif

            return CreateNode(queryNode);
        }

        private static BaseQueryResolverASTNode CreateNode(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"queryNode = {queryNode}");
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
        }

        private static BaseQueryResolverASTNode CreateResolverOfConditionOfQueryASTNode(ConditionOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"queryNode = {queryNode}");
#endif

            var result = new ResolverOfConditionOfQueryASTNode(queryNode);

            return result;
        }

        private static BaseQueryResolverASTNode CreateResolverOfBinaryOperatorOfQueryASTNode(BinaryOperatorOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"queryNode = {queryNode}");
#endif

            var operatorKind = queryNode.OperatorId;

            ResolverOfBinaryOperatorOfQueryASTNode result = null;

            switch (operatorKind)
            {
                case KindOfBinaryOperators.And:
                    result = new ResolverOfAndBinaryOperatorOfQueryASTNode();
                    break;

                case KindOfBinaryOperators.Or:
                    result = new ResolverOfOrBinaryOperatorOfQueryASTNode();
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(operatorKind), operatorKind, null);
            }

            result.Right = CreateNode(queryNode.Right);
            result.Left = CreateNode(queryNode.Left);

            return result;
        }

        private static BaseQueryResolverASTNode CreateResolverOfUnaryOperatorOfQueryASTNode(UnaryOperatorOfQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"queryNode = {queryNode}");
#endif

            var operatorKind = queryNode.OperatorId;

            ResolverOfUnaryOperatorOfQueryASTNode result = null;

            switch (operatorKind)
            {
                case KindOfUnaryOperators.Not:
                    result = new ResolverOfNotUnaryOperatorOfQueryASTNode();
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(operatorKind), operatorKind, null);
            }

            result.Left = CreateNode(queryNode.Left);

            return result;
        }
    }
}
