using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.ConvertingPersistLogicalDataToIndexing
{
    public static class ConvertorToIndexed
    {
        public static IndexedRuleInstance ConvertRuleInstance(RuleInstance source)
        {
            var context = new ContextOfConvertingToIndexed();
            return ConvertRuleInstance(source, context);
        }

        private static IndexedRuleInstance ConvertRuleInstance(RuleInstance source, ContextOfConvertingToIndexed context)
        {
            if(context.RuleInstancesDict.ContainsKey(source))
            {
                return context.RuleInstancesDict[source];
            }

            var result = new IndexedRuleInstance();

            context.RuleInstancesDict[source] = result;

            result.Origin = source;
            result.Key = source.Key;
            result.ModuleKey = source.ModuleKey;

            if(source.BelongToEntity != null)
            {
                result.BelongToEntity = ConvertBelongToEntity(source.BelongToEntity, context);
            }

            if(source.EntitiesConditions != null)
            {
                result.EntitiesConditions = ConvertEntitiesConditions(source.EntitiesConditions, context);
            }

            if (source.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(source.VariablesQuantification, context);
            }

            if(source.Part_1 != null)
            {
                result.IsPart_1_Active = source.Part_1.IsActive;
                result.Part_1 = ConvertRulePart(source.Part_1, context);
            }

            if(source.Part_2 != null)
            {
                result.IsPart_2_Active = source.Part_2.IsActive;
                result.Part_2 = ConvertRulePart(source.Part_2, context);
            }

            if(source.IfConditions != null)
            {
                result.IfConditions = ConvertIfConditionsPart(source.IfConditions, context);
            }
            
            if (source.NotContradict != null)
            {
                result.NotContradict = ConvertNotContradictPart(source.NotContradict, context);
            }

            if(source.AccessPolicyToFactModality != null)
            {
                result.AccessPolicyToFactModality = ConvertAccessPolicyToFactModality(source.AccessPolicyToFactModality, context);
            }
            
            if (source.DesirableModality != null)
            {
                result.DesirableModality = ConvertDesirableFuzzyModality(source.DesirableModality, context);
            }

            if(source.NecessityModality != null)
            {
                result.NecessityModality = ConvertNecessityFuzzyModality(source.NecessityModality, context);
            }

            if(source.ImperativeModality != null)
            {
                result.ImperativeModality = ConvertImperativeFuzzyModality(source.ImperativeModality, context);
            }

            if (source.IntentionallyModality != null)
            {
                result.IntentionallyModality = ConvertIntentionallyFuzzyModality(source.IntentionallyModality, context);
            }

            if (source.PriorityModality != null)
            {
                result.PriorityModality = ConvertPriorityFuzzyModality(source.PriorityModality, context);
            }

            if (source.RealityModality != null)
            {
                result.RealityModality = ConvertRealityFuzzyModality(source.RealityModality, context);
            }

            if (source.ProbabilityModality != null)
            {
                result.ProbabilityModality = ConvertProbabilityFuzzyModality(source.ProbabilityModality, context);
            }

            if (source.CertaintyFactor != null)
            {
                result.CertaintyFactor = ConvertCertaintyFactorFuzzyModality(source.CertaintyFactor, context);
            }

            if (source.MoralQualityModality != null)
            {
                result.MoralQualityModality = ConvertMoralQualityFuzzyModality(source.MoralQualityModality, context);
            }

            if (source.QuantityQualityModality != null)
            {
                result.QuantityQualityModality = ConvertQuantityQualityFuzzyModality(source.QuantityQualityModality, context);
            }

            return result;
        }

        private static IndexedBelongToEntity ConvertBelongToEntity(BaseExpressionNode source, ContextOfConvertingToIndexed context)
        {
            if(context.BelongToEntityDict.ContainsKey(source))
            {
                return context.BelongToEntityDict[source];
            }

            var result = new IndexedBelongToEntity();
            context.BelongToEntityDict[source] = result;

            result.Origin = source;

            return result;
        }

        private static IndexedVariablesQuantificationPart ConvertVariablesQuantification(VariablesQuantificationPart source, ContextOfConvertingToIndexed context)
        {          
            if (context.VariablesQuantificationPartDict.ContainsKey(source))
            {
                return context.VariablesQuantificationPartDict[source];
            }

            var result = new IndexedVariablesQuantificationPart();
            context.VariablesQuantificationPartDict[source] = result;

            result.Origin = source;

            return result;
        }

        private static IndexedEntitiesConditions ConvertEntitiesConditions(EntitiesConditions source, ContextOfConvertingToIndexed context)
        {
            if(context.EntitiesConditionsDict.ContainsKey(source))
            {
                return context.EntitiesConditionsDict[source];
            }

            var result = new IndexedEntitiesConditions();
            context.EntitiesConditionsDict[source] = result;

            result.Origin = source;

            return result;
        }

        private static IndexedRulePart ConvertRulePart(RulePart source, ContextOfConvertingToIndexed context)
        {
            if(context.RulePartDict.ContainsKey(source))
            {
                return context.RulePartDict[source];
            }

            var result = new IndexedRulePart();
            context.RulePartDict[source] = result;

            result.Origin = source;

            result.IsActive = source.IsActive;

            result.Parent = context.RuleInstancesDict[source.Parent];

            if (source.NextPart != null)
            {
                result.NextPart = ConvertRulePart(source.NextPart, context);
            }

            if(source.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(source.VariablesQuantification, context);
            }

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, context, contextOfConvertingExpressionNode);

#if DEBUG
            LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif

            return result;
        }

        private static ResolverForBaseExpressionNode ConvertExpressionNode(BaseExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var kind = source.Kind;

            switch (kind)
            {
                case KindOfExpressionNode.And:
                    return ConvertAndNode(source.AsOperatorAnd, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Or:
                    return ConvertOrNode(source.AsOperatorOr, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Not:
                    return ConvertNotNode(source.AsOperatorNot, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Relation:
                    return ConvertRelationNode(source.AsRelation, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Concept:
                    return ConvertConceptNode(source.AsConcept, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.EntityRef:
                    return ConvertEntityRefNode(source.AsEntityRef, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.EntityCondition:
                    return ConvertEntityConditionNode(source.AsEntityCondition, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Var:
                    return ConvertVarNode(source.AsVar, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.QuestionVar:
                    return ConvertQuestionVarNode(source.AsQuestionVar, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Value:
                    return ConvertValueNode(source.AsValue, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.FuzzyLogicValue:
                    return ConvertFuzzyLogicValueNode(source.AsFuzzyLogicValue, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Fact:
                    return ConvertFactNode(source.AsFact, context, contextOfConvertingExpressionNode);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static ResolverForOperatorAndExpressionNode ConvertAndNode(OperatorAndExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorAndExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context, contextOfConvertingExpressionNode);
            result.Right = ConvertExpressionNode(source.Right, context, contextOfConvertingExpressionNode);
            return result;
        }

        private static ResolverForOperatorOrExpressionNode ConvertOrNode(OperatorOrExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorOrExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context, contextOfConvertingExpressionNode);
            result.Right = ConvertExpressionNode(source.Right, context, contextOfConvertingExpressionNode);
            return result;
        }

        private static ResolverForOperatorNotExpressionNode ConvertNotNode(OperatorNotExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorNotExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context, contextOfConvertingExpressionNode);
            return result;
        }

        private static ResolverForRelationExpressionNode ConvertRelationNode(RelationExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForRelationExpressionNode();
            result.Key = source.Key;
            var parametersList = new List<ResolverForBaseExpressionNode>();
            foreach (var param in source.Params)
            {
                var resultParam = ConvertExpressionNode(param, context, contextOfConvertingExpressionNode);
                parametersList.Add(resultParam);
            }
            result.Params = parametersList;
            contextOfConvertingExpressionNode.RelationsList.Add(source);
            return result;
        }

        private static ResolverForConceptExpressionNode ConvertConceptNode(ConceptExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForConceptExpressionNode();
            result.Key = source.Key;
            return result;
        }

        private static ResolverForEntityRefExpressionNode ConvertEntityRefNode(EntityRefExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForEntityRefExpressionNode();
            result.Key = source.Key;
            return result;
        }

        private static ResolverForEntityConditionExpressionNode ConvertEntityConditionNode(EntityConditionExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForEntityConditionExpressionNode();
            result.Key = source.Key;
            return result;
        }

        private static ResolverForVarExpressionNode ConvertVarNode(VarExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForVarExpressionNode();
            result.Key = source.Key;
            contextOfConvertingExpressionNode.VarsList.Add(source);
            return result;
        }

        private static ResolverForQuestionVarExpressionNode ConvertQuestionVarNode(QuestionVarExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForQuestionVarExpressionNode();
            result.Key = source.Key;
            contextOfConvertingExpressionNode.QuestionVarsList.Add(source);
            return result;
        }

        private static ResolverForValueExpressionNode ConvertValueNode(ValueExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForValueExpressionNode();
            result.Value = source.Value;
            return result;
        }

        private static ResolverForFuzzyLogicValueExpressionNode ConvertFuzzyLogicValueNode(FuzzyLogicValueExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForFuzzyLogicValueExpressionNode();
            return result;
        }

        private static ResolverForFactExpressionNode ConvertFactNode(FactExpressionNode source, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForFactExpressionNode();
            result.Key = source.Key;
            return result;
        }

        private static IndexedIfConditionsPart ConvertIfConditionsPart(IfConditionsPart source, ContextOfConvertingToIndexed context)
        {
            if(context.IfConditionsPartDict.ContainsKey(source))
            {
                return context.IfConditionsPartDict[source];
            }

            var result = new IndexedIfConditionsPart();
            context.IfConditionsPartDict[source] = result;

            result.Origin = source;

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, context, contextOfConvertingExpressionNode);

#if DEBUG
            LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif

            return result;
        }

        private static IndexedNotContradictPart ConvertNotContradictPart(NotContradictPart source, ContextOfConvertingToIndexed context)
        {
            if(context.NotContradictPartDict.ContainsKey(source))
            {
                return context.NotContradictPartDict[source];
            }

            var result = new IndexedNotContradictPart();
            context.NotContradictPartDict[source] = result;

            result.Origin = source;

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, context, contextOfConvertingExpressionNode);

#if DEBUG
            LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif

            return result;
        }

        private static IndexedAccessPolicyToFactModality ConvertAccessPolicyToFactModality(AccessPolicyToFactModality source, ContextOfConvertingToIndexed context)
        {
            if(context.AccessPolicyToFactModalityDict.ContainsKey(source))
            {
                return context.AccessPolicyToFactModalityDict[source];
            }

            var result = new IndexedAccessPolicyToFactModality();
            context.AccessPolicyToFactModalityDict[source] = result;
            result.Kind = source.Kind;
            if(source.Expression != null)
            {
                var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

                result.Expression = ConvertExpressionNode(source.Expression, context, contextOfConvertingExpressionNode);
            }
            return result;
        }
            
        private static IndexedDesirableFuzzyModality ConvertDesirableFuzzyModality(DesirableFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if(context.DesirableFuzzyModalityDict.ContainsKey(source))
            {
                return context.DesirableFuzzyModalityDict[source];
            }

            var result = new IndexedDesirableFuzzyModality();
            context.DesirableFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedNecessityFuzzyModality ConvertNecessityFuzzyModality(NecessityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if(context.NecessityFuzzyModalityDict.ContainsKey(source))
            {
                return context.NecessityFuzzyModalityDict[source];
            }

            var result = new IndexedNecessityFuzzyModality();
            context.NecessityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedImperativeFuzzyModality ConvertImperativeFuzzyModality(ImperativeFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if(context.ImperativeFuzzyModalityDict.ContainsKey(source))
            {
                return context.ImperativeFuzzyModalityDict[source];
            }
            var result = new IndexedImperativeFuzzyModality();
            context.ImperativeFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedIntentionallyFuzzyModality ConvertIntentionallyFuzzyModality(IntentionallyFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if(context.IntentionallyFuzzyModalityDict.ContainsKey(source))
            {
                return context.IntentionallyFuzzyModalityDict[source];
            }
            var result = new IndexedIntentionallyFuzzyModality();
            context.IntentionallyFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedPriorityFuzzyModality ConvertPriorityFuzzyModality(PriorityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if(context.PriorityFuzzyModalityDict.ContainsKey(source))
            {
                return context.PriorityFuzzyModalityDict[source];
            }
            var result = new IndexedPriorityFuzzyModality();
            context.PriorityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedRealityFuzzyModality ConvertRealityFuzzyModality(RealityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if (context.RealityFuzzyModalityDict.ContainsKey(source))
            {
                return context.RealityFuzzyModalityDict[source];
            }
            var result = new IndexedRealityFuzzyModality();
            context.RealityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedProbabilityFuzzyModality ConvertProbabilityFuzzyModality(ProbabilityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if (context.ProbabilityFuzzyModalityDict.ContainsKey(source))
            {
                return context.ProbabilityFuzzyModalityDict[source];
            }
            var result = new IndexedProbabilityFuzzyModality();
            context.ProbabilityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedCertaintyFactorFuzzyModality ConvertCertaintyFactorFuzzyModality(CertaintyFactorFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if (context.CertaintyFactorFuzzyModalityDict.ContainsKey(source))
            {
                return context.CertaintyFactorFuzzyModalityDict[source];
            }
            var result = new IndexedCertaintyFactorFuzzyModality();
            context.CertaintyFactorFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedMoralQualityFuzzyModality ConvertMoralQualityFuzzyModality(MoralQualityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if (context.MoralQualityFuzzyModalityDict.ContainsKey(source))
            {
                return context.MoralQualityFuzzyModalityDict[source];
            }
            var result = new IndexedMoralQualityFuzzyModality();
            context.MoralQualityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static IndexedQuantityQualityFuzzyModality ConvertQuantityQualityFuzzyModality(QuantityQualityFuzzyModality source, ContextOfConvertingToIndexed context)
        {
            if (context.QuantityQualityFuzzyModalityDict.ContainsKey(source))
            {
                return context.QuantityQualityFuzzyModalityDict[source];
            }
            var result = new IndexedQuantityQualityFuzzyModality();
            context.QuantityQualityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context);
            return result;
        }

        private static void FillIndexedFuzzyModality(FuzzyModality source, IndexedFuzzyModality dest, ContextOfConvertingToIndexed context)
        {
            dest.Parent = context.RuleInstancesDict[source.Parent];
            dest.Origin = source;
        }
    }
}
