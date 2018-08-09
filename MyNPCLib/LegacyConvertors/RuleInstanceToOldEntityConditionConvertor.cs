using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyConvertors
{
    public static class RuleInstanceToOldEntityConditionConvertor
    {
        public static string ConvertToOldQueryString(RuleInstance ruleInstance)
        {
#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

            if (ruleInstance.Part_1 != null)
            {
                return ConvertRulePart(ruleInstance.Part_1);
            }

            if (ruleInstance.Part_2 != null)
            {
                return ConvertRulePart(ruleInstance.Part_2);
            }

            return string.Empty;
        }

        private static string ConvertRulePart(RulePart rulepart)
        {
#if DEBUG
            //LogInstance.Log($"rulepart = {rulepart?.ToShortString()}");
#endif

            return ConvertBaseExpressionNode(rulepart.Expression);
        }

        private static string ConvertBaseExpressionNode(BaseExpressionNode expressionNode)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode?.ToShortString()}");
#endif

            var kind = expressionNode.Kind;

            switch(kind)
            {
                case KindOfExpressionNode.And:
                    return ConvertOperatorAnd(expressionNode.AsOperatorAnd);

                case KindOfExpressionNode.Relation:
                    return ConvertRelation(expressionNode.AsRelation);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static string ConvertOperatorAnd(OperatorAndExpressionNode operatorAndNode)
        {
#if DEBUG
            //LogInstance.Log($"operatorAndNode = {operatorAndNode?.ToShortString()}");
#endif

            return $"{ConvertBaseExpressionNode(operatorAndNode.Left)}&{ConvertBaseExpressionNode(operatorAndNode.Right)}";
        }

        private static string ConvertRelation(RelationExpressionNode relationNode)
        {
#if DEBUG
            //LogInstance.Log($"relationNode = {relationNode?.ToShortString()}");
#endif

            if(relationNode.Params.Count == 1)
            {
                return ConvertRelationWithOneArg(relationNode);
            }

            if (relationNode.Params.Count == 2)
            {
                return ConvertRelationWithTwoArgs(relationNode);
            }

            throw new NotImplementedException();
        }

        private static string ConvertRelationWithOneArg(RelationExpressionNode relationNode)
        {
#if DEBUG
            //LogInstance.Log($"relationNode = {relationNode?.ToShortString()}");
#endif

            var arg = relationNode.Params[0];

            if (!arg.IsVar)
            {
                throw new NotSupportedException();
            }

            return $"class={relationNode.Name}";
        }

        private static string ConvertRelationWithTwoArgs(RelationExpressionNode relationNode)
        {
#if DEBUG
            //LogInstance.Log($"relationNode = {relationNode?.ToShortString()}");
#endif

            var arg1 = relationNode.Params[0];
            var arg2 = relationNode.Params[1];

            if(!arg1.IsVar)
            {
                throw new NotSupportedException();
            }

            if(!arg2.IsConcept)
            {
                throw new NotSupportedException();
            }

            return $"{relationNode.Name}={arg2.AsConcept.Name}";
        }
    }
}
