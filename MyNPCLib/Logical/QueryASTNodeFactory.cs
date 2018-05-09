using MyNPCLib.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public static class QueryASTNodeFactory
    {
        public static BaseQueryASTNode CreateASTNode(string queryString, IEntityDictionary entityDictionary)
        {
#if DEBUG
            //LogInstance.Log($"QueryASTNodeFactory CreateASTNode queryString = {queryString}");
#endif

            var context = new ParserContext(queryString, entityDictionary);

            var node = LogicalExpressionParserHelper.CreateNode(context);

#if DEBUG
            //LogInstance.Log($"QueryASTNodeFactory CreateASTNode node = {node}");
#endif

            return node;
            //return CreateFakeASTNodeCase_2(entityDictionary);
        }

        public static BaseQueryASTNode CreateFakeASTNodeCase_1(IEntityDictionary entityDictionary)
        {
#if DEBUG
            LogInstance.Log("QueryASTNodeFactory CreateFakeASTNodeCase_1");
#endif
            var namePropertyId = entityDictionary.GetKey("name");
            var classPropertyId = entityDictionary.GetKey("class");

            var conditionNode = new ConditionOfQueryASTNode();
            conditionNode.PropertyId = namePropertyId;
            conditionNode.Value = "helen";

            var conditionNode_2 = new ConditionOfQueryASTNode();
            conditionNode_2.PropertyId = namePropertyId;
            conditionNode_2.Value = "ann";

            var orNode = new BinaryOperatorOfQueryASTNode();
            orNode.OperatorId = KindOfBinaryOperators.Or;
            orNode.Left = conditionNode;
            orNode.Right = conditionNode_2;

            var conditionNode_1 = new ConditionOfQueryASTNode();
            conditionNode_1.PropertyId = classPropertyId;
            conditionNode_1.Value = "girl";

            var andNode = new BinaryOperatorOfQueryASTNode();
            andNode.OperatorId = KindOfBinaryOperators.And;
            andNode.Left = orNode;
            andNode.Right = conditionNode_1;

            var notNode = new UnaryOperatorOfQueryASTNode();
            notNode.OperatorId = KindOfUnaryOperators.Not;
            notNode.Left = andNode;

#if DEBUG
            LogInstance.Log($"QueryASTNodeFactory CreateFakeASTNodeCase_1 notNode = {notNode}");
#endif

            return notNode;
        }

        public static BaseQueryASTNode CreateFakeASTNodeCase_2(IEntityDictionary entityDictionary)
        {
#if DEBUG
            LogInstance.Log("QueryASTNodeFactory CreateFakeASTNodeCase_2");
#endif
            var namePropertyId = entityDictionary.GetKey("name");
            var classPropertyId = entityDictionary.GetKey("class");

            var conditionNode = new ConditionOfQueryASTNode();
            conditionNode.PropertyId = namePropertyId;
            conditionNode.Value = "helen";

            var conditionNode_2 = new ConditionOfQueryASTNode();
            conditionNode_2.PropertyId = namePropertyId;
            conditionNode_2.Value = "ann";

            var orNode = new BinaryOperatorOfQueryASTNode();
            orNode.OperatorId = KindOfBinaryOperators.Or;
            orNode.Left = conditionNode;
            orNode.Right = conditionNode_2;

            var conditionNode_1 = new ConditionOfQueryASTNode();
            conditionNode_1.PropertyId = classPropertyId;
            conditionNode_1.Value = "girl";

            var andNode = new BinaryOperatorOfQueryASTNode();
            andNode.OperatorId = KindOfBinaryOperators.And;
            andNode.Left = orNode;
            andNode.Right = conditionNode_1;

#if DEBUG
            LogInstance.Log($"QueryASTNodeFactory CreateFakeASTNodeCase_2 andNode = {andNode}");
#endif

            return andNode;
        }
    }
}
