using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MyNPCLib.DebugHelperForPersistLogicalData
{
    public class ContextForDebugHelperForRuleInstance
    {
        public string MainView { get; set; }
        public List<string> AnnotationsViews { get; set; } = new List<string>();
        public ICGStorage DataSource { get; set; }
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
            context.DataSource = source.DataSource;
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(RuleInstance source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append($"{{: {source.Name}");

            if (!string.IsNullOrWhiteSpace(source.ModuleName))
            {
                sb.Append($" $:{{{source.ModuleName}}}");
                sb.Append(ToString(source.Annotations, context));
            }

            if(source.BelongToEntity != null)
            {
                sb.Append($" $$:{{{ToString(source.BelongToEntity, context)}}}");
                sb.Append(ToString(source.Annotations, context));
            }

            if (source.AccessPolicyToFactModality != null)
            {
                sb.Append(ToString(source.AccessPolicyToFactModality, context));
            }

            if (source.EntitiesConditions != null)
            {
                sb.Append(ToString(source.EntitiesConditions, context));
            }

            if(source.VariablesQuantification != null)
            {
                sb.Append(ToString(source.VariablesQuantification, context));
            }
            
            if (source.Part_1 != null || source.Part_2 != null)
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

            if (source.IfConditions != null)
            {
                sb.Append(ToString(source.IfConditions, context));
            }

            if (source.NotContradict != null)
            {
                sb.Append(ToString(source.NotContradict, context));
            }

            if (source.DesirableModality != null)
            {
                sb.Append(ToString(source.DesirableModality, context));
            }

            if (source.NecessityModality != null)
            {
                sb.Append(ToString(source.NecessityModality, context));
            }

            if (source.ImperativeModality != null)
            {
                sb.Append(ToString(source.ImperativeModality, context));
            }

            if (source.IntentionallyModality != null)
            {
                sb.Append(ToString(source.IntentionallyModality, context));
            }

            if (source.PriorityModality != null)
            {
                sb.Append(ToString(source.PriorityModality, context));
            }

            if (source.RealityModality != null)
            {
                sb.Append(ToString(source.RealityModality, context));
            }

            if (source.ProbabilityModality != null)
            {
                sb.Append(ToString(source.ProbabilityModality, context));
            }

            if (source.CertaintyFactor != null)
            {
                sb.Append(ToString(source.CertaintyFactor, context));
            }

            if (source.MoralQualityModality != null)
            {
                sb.Append(ToString(source.MoralQualityModality, context));
            }

            if (source.QuantityQualityModality != null)
            {
                sb.Append(ToString(source.QuantityQualityModality, context));
            }

            sb.Append(":}");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ToString(EntitiesConditions source, ContextForDebugHelperForRuleInstance context)
        {
            var items = source.Items;

            if (ListHelper.IsEmpty(items))
            {
                return string.Empty;
            }

            var paramsViewsList = new List<string>();

            foreach (var item in items)
            {
                paramsViewsList.Add($"{item.VariableName}:{item.Name}");
            }

            var sb = new StringBuilder();
            sb.Append($"(:{string.Join(",", paramsViewsList)}:)");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ToString(KindOfQuantifier quantifier)
        {
            switch(quantifier)
            {
                case KindOfQuantifier.Undefined:
                    return string.Empty;

                case KindOfQuantifier.Universal:
                    return "∀";

                case KindOfQuantifier.Existential:
                    return "∃";

                default: throw new ArgumentOutOfRangeException(nameof(quantifier), quantifier, null);
            }
        }

        private static string ToString(VariablesQuantificationPart source, ContextForDebugHelperForRuleInstance context)
        {
            var items = source.Items;

            if(ListHelper.IsEmpty(items))
            {
                return string.Empty;
            }

            var paramsViewsList = new List<string>();

            foreach(var item in items)
            {
                paramsViewsList.Add($"{ToString(item.Quantifier)}{item.Name}");
            }

            var sb = new StringBuilder();
            sb.Append($"({string.Join(",", paramsViewsList)})");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string GetMarkBetweenParts(RuleInstance source)
        {
            if(source.Part_1 == null || source.Part_2 == null)
            {
                return string.Empty;
            }

            if(source.Part_1.IsActive && source.Part_2.IsActive)
            {
                return "<->";
            }

            if(source.Part_1.IsActive)
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
            sb.Append(" {");
            if (source.VariablesQuantification != null)
            {
                sb.Append(ToString(source.VariablesQuantification, context));
                sb.Append("(");
            }
            if (source.Expression != null)
            {
                sb.Append(ToString(source.Expression, context));
            }
            if (source.VariablesQuantification != null)
            {
                sb.Append(")");
            }
            sb.Append("} ");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        public static string ToString(BaseExpressionNode source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            if(source != null)
            {
                context.MainView = ToString(source, context);
            }   
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

                case KindOfExpressionNode.QuestionVar:
                    return QuestionVarToString(source.AsQuestionVar, context);

                case KindOfExpressionNode.Value:
                    return ValueToString(source.AsValue, context);

                case KindOfExpressionNode.FuzzyLogicValue:
                    return FuzzyLogicValueToString(source.AsFuzzyLogicValue, context);

                case KindOfExpressionNode.Fact:
                    return FactToString(source.AsFact, context);

                case KindOfExpressionNode.ParamStub:
                    return ParamStubToSting(source.AsParamStub, context);

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

            if(source.LinkedVars != null)
            {
                foreach (var linkedVar in source.LinkedVars)
                {
                    sb.Append($"{linkedVar.Name} = ");
                }
            }

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
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ConceptToString(ConceptExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.Name);
            sb.Append(ToString(source.Annotations, context));
            return source.Name.ToString();
        }

        private static string EntityRefToString(EntityRefExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.Name);
            sb.Append(ToString(source.Annotations, context));
            return source.Name.ToString();
        }

        private static string EntityConditionToString(EntityConditionExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.VariableName);
            sb.Append(ToString(source.Annotations, context));
            return source.VariableName.ToString();
        }

        private static string VarToString(VarExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.Name);
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string QuestionVarToString(QuestionVarExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.Name);
            sb.Append(ToString(source.Annotations, context));
            return source.Name.ToString();
        }

        private static string ValueToString(ValueExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(NValueToString(source, context));
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static Type mStringType = typeof(string);
        private static Type mFloatType = typeof(float);
        private static CultureInfo mCultureInfo = new CultureInfo("en-us");
        private static string NValueToString(ValueExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var value = source.Value;

            if (value == null)
            {
                return "NULL";
            }

            var typeOfValue = value.GetType();

            if (typeOfValue == mStringType)
            {
                return $"'{value}'";
            }

            if (typeOfValue == mFloatType)
            {
                return ((float)value).ToString(mCultureInfo);
            }

            return value.ToString();
        }

        private static string FuzzyLogicValueToString(FuzzyLogicValueExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var value = source.Value;

            return $"{value}l";
        }

        private static string FactToString(FactExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append(source.Name);
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ParamStubToSting(ParamStubExpressionNode source, ContextForDebugHelperForRuleInstance context)
        {
            return "*";
        }

        private static string ToString(IfConditionsPart source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append("^^:{");
            if (source.Expression != null)
            {
                sb.Append(ToString(source.Expression, context));
            }
            sb.Append("}");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ToString(NotContradictPart source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append("^:{");
            if(source.Expression != null)
            {
                sb.Append(ToString(source.Expression, context));
            }
            sb.Append("}");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ToString(IList<AccessPolicyToFactModality> source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder(" !:");

            if(source.Count == 1)
            {
                sb.Append(ToString(source.Single(), context));
                return sb.ToString();
            }

            var n = 0;
            sb.Append("{");
            foreach (var item in source)
            {
                sb.Append(ToString(item, context));
                n++;

                if (n == source.Count)
                {
                    continue;
                }
               
                sb.Append(",");
            }
            sb.Append("}");
            return sb.ToString();
        }

        private static string ToString(AccessPolicyToFactModality source, ContextForDebugHelperForRuleInstance context)
        {
            var kind = source.Kind;

            var sb = new StringBuilder();
            switch (kind)
            {
                case KindOfAccessPolicyToFact.Public:
                    sb.Append("public");
                    break;

                case KindOfAccessPolicyToFact.ForVisible:
                    sb.Append("visible");
                    break;

                case KindOfAccessPolicyToFact.Private:
                    sb.Append("private");
                    break;

                case KindOfAccessPolicyToFact.IfCondition:
                    sb.Append("{");
                    sb.Append(ToString(source.Expression, context));
                    sb.Append("}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();;
        }

        private static string ToString(DesirableFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(NecessityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(ImperativeFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(IntentionallyFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(PriorityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(RealityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(ProbabilityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(CertaintyFactorFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append("!:{");
            if (source.Expression != null)
            {
                sb.Append(ToString(source.Expression, context));
            }
            sb.Append("}");
            sb.Append(ToString(source.Annotations, context));
            return sb.ToString();
        }

        private static string ToString(MoralQualityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(QuantityQualityFuzzyModality source, ContextForDebugHelperForRuleInstance context)
        {
            throw new NotImplementedException();
        }

        private static string ToString(IList<LogicalAnnotation> annotations, ContextForDebugHelperForRuleInstance context)
        {
            if (ListHelper.IsEmpty(annotations))
            {
                return string.Empty;
            }

            var dataSource = context.DataSource;

            if(dataSource == null)
            {
                return string.Empty;
            }

            var annotationsViewsList = new List<string>();

            foreach (var annotation in annotations)
            {
                var ruleInstanceKey = annotation.RuleInstanceKey;

                var annotationRuleInstance = dataSource.GetRuleInstanceByKey(ruleInstanceKey);

                annotationsViewsList.Add($"{ToString(annotationRuleInstance)}{ToString(annotation.Annotations, context)}");
            }

            return $"[:{string.Join(",", annotationsViewsList)}:]";
        }
    }
}
