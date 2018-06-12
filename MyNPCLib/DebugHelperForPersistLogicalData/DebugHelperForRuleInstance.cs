using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.DebugHelperForPersistLogicalData
{
    public class ContextForDebugHelperForRuleInstance
    {
        public string MainView { get; set; }
        public List<string> AnnotationsViews { get; set; } = new List<string>();
    }

    public static class DebugHelperForRuleInstance
    {
        private static string ToString(ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            if(context.AnnotationsViews.Count == 0)
            {
                return context.MainView;
            }

            return sb.ToString();
        }

        public static string ToString(RuleInstance source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(RuleInstance source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append($"{{:{source.Name}");
            if(source.Part_1 != null || source.Part_2 != null)
            {
                var markBetweenParts = GetMarkBetweenParts(source);

                if(source.Part_1 != null)
                {
                    sb.Append(ToString(source.Part_1, context));
                }
                
                sb.Append(markBetweenParts);

                if (source.Part_2 != null)
                {
                    sb.Append(ToString(source.Part_2, context));
                }
            }
            sb.Append(":}}");
            return sb.ToString();
        }

        private static string GetMarkBetweenParts(RuleInstance source)
        {
            if(source.Part_1 == null || source.Part_2 == null)
            {
                return string.Empty;
            }

            if(source.IsPart_1_Active && source.IsPart_2_Active)
            {
                return "<->";
            }

            if(source.IsPart_1_Active)
            {
                return "->";
            }

            return "<-";
        }

        public static string ToString(RulePart source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(RulePart source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append("{");

            if(source.Expression != null)
            {
                sb.Append(ToString(source.Expression, context));
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ToString(BaseExpressionNode source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(BaseExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var kind = source.Kind;

            switch(kind)
            {
                case KindOfExpressionNode.And:
                    return AndToString(source.AsOperatorAnd, context);

                case KindOfExpressionNode.Or:
                    return OrToString(source.AsOperatorOr, context);

                case KindOfExpressionNode.Not:
                    return NotToString(source.AsOperatorNot, context);

                case KindOfExpressionNode.Relation:
                    return RelationToString(source.AsRelation, context);

                case KindOfExpressionNode.Concept:
                    return ConceptToString(source.AsConcept, context);

                case KindOfExpressionNode.EntityRef:
                    return EntityRefToString(source.AsEntityRef, context);

                case KindOfExpressionNode.EntityCondition:
                    return EntityConditionToString(source.AsEntityCondition, context);

                case KindOfExpressionNode.Var:
                    return VarToString(source.AsVar, context);

                case KindOfExpressionNode.Value:
                    return ValueToString(source.AsValue, context);

                case KindOfExpressionNode.FuzzyLogicValue:
                    return FuzzyLogicValueToString(source.AsFuzzyLogicValue, context);

                case KindOfExpressionNode.Fact:
                    return FactToString(source.AsFact, context);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static string AndToString(OperatorAndExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return $"{ToString(source.Left, context)} & {ToString(source.Right, context)}";
        }

        private static string OrToString(OperatorOrExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return $"{ToString(source.Left, context)} | {ToString(source.Right, context)}";
        }

        private static string NotToString(OperatorNotExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return $"!{ToString(source.Left, context)}";
        }

        private static string RelationToString(RelationExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append($"{source.Name}(");
            if(!ListHelper.IsEmpty(source.Params))
            {
                var paramsViewsList = new List<string>();

                foreach(var param in source.Params)
                {
                    paramsViewsList.Add(ToString(param, context));
                }

                var paramsStr = string.Join(",", paramsViewsList);
                sb.Append(paramsStr);
            }
            sb.Append(")");
            return sb.ToString();
        }

        private static string ConceptToString(ConceptExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return source.Name;
        }

        private static string EntityRefToString(EntityRefExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return source.Name;
        }

        private static string EntityConditionToString(EntityConditionExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string VarToString(VarExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ValueToString(ValueExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string FuzzyLogicValueToString(FuzzyLogicValueExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string FactToString(FactExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }
    }
}
