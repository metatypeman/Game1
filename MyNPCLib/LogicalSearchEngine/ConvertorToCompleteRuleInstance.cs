﻿using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public static class ConvertorToCompleteRuleInstance
    {
        public static RuleInstance Convert(LogicalSearchResultItem source, IEntityDictionary entityDictionary)
        {
            var context = new ContextOfConvertorToCompleteRuleInstance();
            context.EntityDictionary = entityDictionary;
            context.ResultOfVarOfQueryToRelationDict = source.ResultOfVarOfQueryToRelationList.ToDictionary(p => p.KeyOfVar, p => p.FoundExpression);

            var queryExpression = source.QueryExpression.Origin;

            var result = new RuleInstance();
            result.DictionaryName = entityDictionary.Name;
            result.Kind = KindOfRuleInstance.Fact;
            var name = NamesHelper.CreateEntityName();
            result.Name = name;
            result.Key = entityDictionary.GetKey(name);
            result.ModuleName = queryExpression.ModuleName;
            result.ModuleKey = queryExpression.ModuleKey;

            if(queryExpression.BelongToEntity != null)
            {
                result.BelongToEntity = ConvertExpressionNode(queryExpression.BelongToEntity, context);
            }

            if(queryExpression.EntitiesConditions != null)
            {
                result.EntitiesConditions = ConvertEntitiesConditions(queryExpression.EntitiesConditions, context);
            }

            if (queryExpression.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(queryExpression.VariablesQuantification, context);
            }

            if (queryExpression.Part_1 != null)
            {
                result.Part_1 = ConvertRulePart(queryExpression.Part_1, result, context);
            }

            if (queryExpression.Part_2 != null)
            {
                result.Part_2 = ConvertRulePart(queryExpression.Part_2, result, context);
            }

            if (queryExpression.IfConditions != null)
            {
                result.IfConditions = ConvertIfConditionsPart(queryExpression.IfConditions, result, context);
            }

            if (queryExpression.NotContradict != null)
            {
                result.NotContradict = ConvertNotContradictPart(queryExpression.NotContradict, result, context);
            }

            if (queryExpression.AccessPolicyToFactModality != null)
            {
                result.AccessPolicyToFactModality = ConvertAccessPolicyToFactModality(queryExpression.AccessPolicyToFactModality, result, context);
            }

            if (queryExpression.DesirableModality != null)
            {
                result.DesirableModality = ConvertDesirableFuzzyModality(queryExpression.DesirableModality, result, context);
            }

            if (queryExpression.NecessityModality != null)
            {
                result.NecessityModality = ConvertNecessityFuzzyModality(queryExpression.NecessityModality, result, context);
            }

            if (queryExpression.ImperativeModality != null)
            {
                result.ImperativeModality = ConvertImperativeFuzzyModality(queryExpression.ImperativeModality, result, context);
            }

            if (queryExpression.IntentionallyModality != null)
            {
                result.IntentionallyModality = ConvertIntentionallyFuzzyModality(queryExpression.IntentionallyModality, result, context);
            }

            if (queryExpression.PriorityModality != null)
            {
                result.PriorityModality = ConvertPriorityFuzzyModality(queryExpression.PriorityModality, result, context);
            }

            if (queryExpression.RealityModality != null)
            {
                result.RealityModality = ConvertRealityFuzzyModality(queryExpression.RealityModality, result, context);
            }

            if (queryExpression.ProbabilityModality != null)
            {
                result.ProbabilityModality = ConvertProbabilityFuzzyModality(queryExpression.ProbabilityModality, result, context);
            }

            if (queryExpression.CertaintyFactor != null)
            {
                result.CertaintyFactor = ConvertCertaintyFactorFuzzyModality(queryExpression.CertaintyFactor, result, context);
            }

            if (queryExpression.MoralQualityModality != null)
            {
                result.MoralQualityModality = ConvertMoralQualityFuzzyModality(queryExpression.MoralQualityModality, result, context);
            }

            if (queryExpression.QuantityQualityModality != null)
            {
                result.QuantityQualityModality = ConvertQuantityQualityFuzzyModality(queryExpression.QuantityQualityModality, result, context);
            }

            result.Annotations = ConvertAnnotations(queryExpression.Annotations, context);
            return result;
        }

        private static VariablesQuantificationPart ConvertVariablesQuantification(VariablesQuantificationPart source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new VariablesQuantificationPart();
            var resultItemsList = new List<VarExpressionNode>();
            foreach(var varItem in source.Items)
            {
                var resultItem = ConvertVarNode(varItem, context);
                resultItemsList.Add(resultItem);
            }
            result.Items = resultItemsList;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static EntitiesConditions ConvertEntitiesConditions(EntitiesConditions source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new EntitiesConditions();
            var resultItemsList = new List<EntityConditionItem>();
            foreach(var item in source.Items)
            {
                var resultItem = new EntityConditionItem();
                resultItem.Name = item.Name;
                resultItem.Key = item.Key;
                resultItem.VariableName = item.VariableName;
                resultItem.VariableKey = item.VariableKey;
                if(item.Annotations != null)
                {
                    resultItem.Annotations = ConvertAnnotations(item.Annotations, context);
                }
                resultItemsList.Add(resultItem);
            }
            result.Items = resultItemsList;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static RulePart ConvertRulePart(RulePart source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            if(context.RulePartsDict.ContainsKey(source))
            {
                return context.RulePartsDict[source];
            }

            var result = new RulePart();
            context.RulePartsDict[source] = result;
            result.IsActive = source.IsActive;
            result.Parent = parent;

            if(source.NextPart != null)
            {
                result.NextPart = ConvertRulePart(source.NextPart, parent, context);
            }

            if (source.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(source.VariablesQuantification, context);
            }

            if (source.Expression != null)
            {
                result.Expression = ConvertExpressionNode(source.Expression, context);
            }

            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static BaseExpressionNode ConvertExpressionNode(BaseExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var kind = source.Kind;

            switch (kind)
            {
                case KindOfExpressionNode.And:
                    return ConvertAndNode(source.AsOperatorAnd, context);

                case KindOfExpressionNode.Or:
                    return ConvertOrNode(source.AsOperatorOr, context);

                case KindOfExpressionNode.Not:
                    return ConvertNotNode(source.AsOperatorNot, context);

                case KindOfExpressionNode.Relation:
                    return ConvertRelationNode(source.AsRelation, context);

                case KindOfExpressionNode.Concept:
                    return ConvertConceptNode(source.AsConcept, context);

                case KindOfExpressionNode.EntityRef:
                    return ConvertEntityRefNode(source.AsEntityRef, context);

                case KindOfExpressionNode.EntityCondition:
                    return ConvertEntityConditionNode(source.AsEntityCondition, context);

                case KindOfExpressionNode.Var:
                    return ConvertVarNode(source.AsVar, context);

                case KindOfExpressionNode.QuestionVar:
                    return ConvertQuestionVarNode(source.AsQuestionVar, context);

                case KindOfExpressionNode.Value:
                    return ConvertValueNode(source.AsValue, context);

                case KindOfExpressionNode.FuzzyLogicValue:
                    return ConvertFuzzyLogicValueNode(source.AsFuzzyLogicValue, context);

                case KindOfExpressionNode.Fact:
                    return ConvertFactNode(source.AsFact, context);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static OperatorAndExpressionNode ConvertAndNode(OperatorAndExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new OperatorAndExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context);
            result.Right = ConvertExpressionNode(source.Right, context);
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static OperatorOrExpressionNode ConvertOrNode(OperatorOrExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new OperatorOrExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context);
            result.Right = ConvertExpressionNode(source.Right, context);
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static OperatorNotExpressionNode ConvertNotNode(OperatorNotExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new OperatorNotExpressionNode();
            result.Left = ConvertExpressionNode(source.Left, context);
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static RelationExpressionNode ConvertRelationNode(RelationExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new RelationExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            var resultParamsList = new List<BaseExpressionNode>();
            foreach(var item in source.Params)
            {
                var resultParamItem = ConvertExpressionNode(item, context);
                resultParamsList.Add(resultParamItem);
            }
            result.Params = resultParamsList;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static ConceptExpressionNode ConvertConceptNode(ConceptExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new ConceptExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static EntityRefExpressionNode ConvertEntityRefNode(EntityRefExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new EntityRefExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static EntityConditionExpressionNode ConvertEntityConditionNode(EntityConditionExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new EntityConditionExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static VarExpressionNode ConvertVarNode(VarExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new VarExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            result.Quantifier = source.Quantifier;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static BaseExpressionNode ConvertQuestionVarNode(QuestionVarExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var key = source.Key;
            if (context.ResultOfVarOfQueryToRelationDict.ContainsKey(key))
            {
                var targetValue = context.ResultOfVarOfQueryToRelationDict[key];
                
                var foundResult = ConvertExpressionNode(targetValue, context);
                return foundResult;
            }
            var result = new QuestionVarExpressionNode();
            result.Name = source.Name;
            result.Key = key;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static ValueExpressionNode ConvertValueNode(ValueExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new ValueExpressionNode();
            result.Value = source.Value;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static FuzzyLogicValueExpressionNode ConvertFuzzyLogicValueNode(FuzzyLogicValueExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new FuzzyLogicValueExpressionNode();
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static FactExpressionNode ConvertFactNode(FactExpressionNode source, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new FactExpressionNode();
            result.Name = source.Name;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static IfConditionsPart ConvertIfConditionsPart(IfConditionsPart source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new IfConditionsPart();
            result.Parent = parent;
            if (source.Expression != null)
            {
                result.Expression = ConvertExpressionNode(source.Expression, context);
            }

            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static NotContradictPart ConvertNotContradictPart(NotContradictPart source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new NotContradictPart();
            result.Parent = parent;
            if (source.Expression != null)
            {
                result.Expression = ConvertExpressionNode(source.Expression, context);
            }

            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static AccessPolicyToFactModality ConvertAccessPolicyToFactModality(AccessPolicyToFactModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new AccessPolicyToFactModality();
            if (source.Expression != null)
            {
                result.Expression = ConvertExpressionNode(source.Expression, context);
            }

            result.Annotations = ConvertAnnotations(source.Annotations, context);
            return result;
        }

        private static DesirableFuzzyModality ConvertDesirableFuzzyModality(DesirableFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new DesirableFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static NecessityFuzzyModality ConvertNecessityFuzzyModality(NecessityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new NecessityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static ImperativeFuzzyModality ConvertImperativeFuzzyModality(ImperativeFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new ImperativeFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static IntentionallyFuzzyModality ConvertIntentionallyFuzzyModality(IntentionallyFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new IntentionallyFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static PriorityFuzzyModality ConvertPriorityFuzzyModality(PriorityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new PriorityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static RealityFuzzyModality ConvertRealityFuzzyModality(RealityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new RealityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static ProbabilityFuzzyModality ConvertProbabilityFuzzyModality(ProbabilityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new ProbabilityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static CertaintyFactorFuzzyModality ConvertCertaintyFactorFuzzyModality(CertaintyFactorFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new CertaintyFactorFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static MoralQualityFuzzyModality ConvertMoralQualityFuzzyModality(MoralQualityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new MoralQualityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }
        
        private static QuantityQualityFuzzyModality ConvertQuantityQualityFuzzyModality(QuantityQualityFuzzyModality source, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            var result = new QuantityQualityFuzzyModality();
            FillFuzzyModality(source, result, parent, context);
            return result;
        }

        private static void FillFuzzyModality(FuzzyModality source, FuzzyModality dest, RuleInstance parent, ContextOfConvertorToCompleteRuleInstance context)
        {
            dest.Parent = parent;
            dest.Expression = ConvertExpressionNode(source.Expression, context);
            dest.Annotations = ConvertAnnotations(source.Annotations, context);
        }

        private static IList<LogicalAnnotation> ConvertAnnotations(IList<LogicalAnnotation> source, ContextOfConvertorToCompleteRuleInstance context)
        {
            if(source.IsEmpty())
            {
                return new List<LogicalAnnotation>();
            }
            var result = new List<LogicalAnnotation>();
            foreach (var item in source)
            {
                var resultItem = new LogicalAnnotation();
                resultItem.Name = item.Name;
                resultItem.Key = item.Key;
                resultItem.Annotations = ConvertAnnotations(item.Annotations, context);
                result.Add(resultItem);
            }
            return result;
        }
    }
}