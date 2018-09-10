using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.ConvertingPersistLogicalDataToIndexing
{
    public static class ConvertorToIndexed
    {
        public static IndexedRuleInstance ConvertRuleInstance(RuleInstance source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingToIndexed();
            return ConvertRuleInstance(source, context);
        }

        private static IndexedRuleInstance ConvertRuleInstance(RuleInstance source, ContextOfConvertingToIndexed context)
        {
#if DEBUG
            //LogInstance.Log($"(2) source = {source}");
#endif

            if (context.RuleInstancesDict.ContainsKey(source))
            {
                return context.RuleInstancesDict[source];
            }

            var result = new IndexedRuleInstance();

            context.RuleInstancesDict[source] = result;

            result.DataSource = source.DataSource;

            result.Origin = source;
            result.Kind = source.Kind;
            result.Key = source.Key;
            result.ModuleKey = source.ModuleKey;

            if(source.BelongToEntity != null)
            {
                result.BelongToEntity = ConvertBelongToEntity(source.BelongToEntity, context, result);
            }

            if(source.EntitiesConditions != null)
            {
                result.EntitiesConditions = ConvertEntitiesConditions(source.EntitiesConditions, context, result);
            }

            if (source.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(source.VariablesQuantification, context, result);
            }

            if (source.Part_1 != null)
            {
                result.IsPart_1_Active = source.Part_1.IsActive;
                result.Part_1 = ConvertRulePart(source.Part_1, result, context);
            }

            if(source.Part_2 != null)
            {
                result.IsPart_2_Active = source.Part_2.IsActive;
                result.Part_2 = ConvertRulePart(source.Part_2, result, context);
            }

            if(source.IfConditions != null)
            {
                result.IfConditions = ConvertIfConditionsPart(source.IfConditions, result, context);
            }
            
            if (source.NotContradict != null)
            {
                result.NotContradict = ConvertNotContradictPart(source.NotContradict, result, context);
            }

            if(source.AccessPolicyToFactModality != null)
            {
                var accessPolicyToFactModality = new List<IndexedAccessPolicyToFactModality>();

                foreach (var initAccessPolicyToFactModality in source.AccessPolicyToFactModality)
                {
                    var indexedAccessPolicyToFactModality = ConvertAccessPolicyToFactModality(initAccessPolicyToFactModality, result, context);
                    accessPolicyToFactModality.Add(indexedAccessPolicyToFactModality);
                }

                result.AccessPolicyToFactModality = accessPolicyToFactModality;
            }
            
            if (source.DesirableModality != null)
            {
                result.DesirableModality = ConvertDesirableFuzzyModality(source.DesirableModality, context, result);
            }

            if(source.NecessityModality != null)
            {
                result.NecessityModality = ConvertNecessityFuzzyModality(source.NecessityModality, context, result);
            }

            if(source.ImperativeModality != null)
            {
                result.ImperativeModality = ConvertImperativeFuzzyModality(source.ImperativeModality, context, result);
            }

            if (source.IntentionallyModality != null)
            {
                result.IntentionallyModality = ConvertIntentionallyFuzzyModality(source.IntentionallyModality, context, result);
            }

            if (source.PriorityModality != null)
            {
                result.PriorityModality = ConvertPriorityFuzzyModality(source.PriorityModality, context, result);
            }

            if (source.RealityModality != null)
            {
                result.RealityModality = ConvertRealityFuzzyModality(source.RealityModality, context, result);
            }

            if (source.ProbabilityModality != null)
            {
                result.ProbabilityModality = ConvertProbabilityFuzzyModality(source.ProbabilityModality, context, result);
            }

            if (source.CertaintyFactor != null)
            {
                result.CertaintyFactor = ConvertCertaintyFactorFuzzyModality(source.CertaintyFactor, context, result);
            }

            if (source.MoralQualityModality != null)
            {
                result.MoralQualityModality = ConvertMoralQualityFuzzyModality(source.MoralQualityModality, context, result);
            }

            if (source.QuantityQualityModality != null)
            {
                result.QuantityQualityModality = ConvertQuantityQualityFuzzyModality(source.QuantityQualityModality, context, result);
            }
            result.Annotations = ConvertAnnotations(source.Annotations, result);
            result.FillIndexedDataAsStorage();
            return result;
        }

        private static IndexedBelongToEntity ConvertBelongToEntity(BaseExpressionNode source, ContextOfConvertingToIndexed context, IndexedRuleInstance parentIndexedRuleInstance)
        {
            if(context.BelongToEntityDict.ContainsKey(source))
            {
                return context.BelongToEntityDict[source];
            }

            var result = new IndexedBelongToEntity();
            context.BelongToEntityDict[source] = result;

            result.Origin = source;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static IndexedVariablesQuantificationPart ConvertVariablesQuantification(VariablesQuantificationPart source, ContextOfConvertingToIndexed context, IndexedRuleInstance parentIndexedRuleInstance)
        {          
            if (context.VariablesQuantificationPartDict.ContainsKey(source))
            {
                return context.VariablesQuantificationPartDict[source];
            }

            var result = new IndexedVariablesQuantificationPart();
            context.VariablesQuantificationPartDict[source] = result;

            result.Origin = source;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static IndexedEntitiesConditions ConvertEntitiesConditions(EntitiesConditions source, ContextOfConvertingToIndexed context, IndexedRuleInstance parentIndexedRuleInstance)
        {
            if(context.EntitiesConditionsDict.ContainsKey(source))
            {
                return context.EntitiesConditionsDict[source];
            }

            var result = new IndexedEntitiesConditions();
            context.EntitiesConditionsDict[source] = result;

            result.Origin = source;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static IndexedRulePart ConvertRulePart(RulePart source, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            if (context.RulePartDict.ContainsKey(source))
            {
                return context.RulePartDict[source];
            }

            var result = new IndexedRulePart();
            context.RulePartDict[source] = result;

            result.Origin = source;

            result.IsActive = source.IsActive;

            result.Parent = parentIndexedRuleInstance;

            if (source.NextPart != null)
            {
                result.NextPart = ConvertRulePart(source.NextPart, parentIndexedRuleInstance, context);
            }

            if(source.VariablesQuantification != null)
            {
                result.VariablesQuantification = ConvertVariablesQuantification(source.VariablesQuantification, context, parentIndexedRuleInstance);
            }

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, result, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

#if DEBUG
            //LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif

            result.RelationsDict = contextOfConvertingExpressionNode.RelationsList.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => (IList<ResolverForRelationExpressionNode>)p.ToList());

            if(contextOfConvertingExpressionNode.QuestionVarsList.Any())
            {
                result.HasQuestionVars = true;
            }

            if(contextOfConvertingExpressionNode.VarsList.Any())
            {
                result.HasVars = true;
            }
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForBaseExpressionNode ConvertExpressionNode(BaseExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var kind = source.Kind;

            switch (kind)
            {
                case KindOfExpressionNode.And:
                    return ConvertAndNode(source.AsOperatorAnd, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Or:
                    return ConvertOrNode(source.AsOperatorOr, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Not:
                    return ConvertNotNode(source.AsOperatorNot, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Relation:
                    return ConvertRelationNode(source.AsRelation, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Concept:
                    return ConvertConceptNode(source.AsConcept, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.EntityRef:
                    return ConvertEntityRefNode(source.AsEntityRef, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.EntityCondition:
                    return ConvertEntityConditionNode(source.AsEntityCondition, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Var:
                    return ConvertVarNode(source.AsVar, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.QuestionVar:
                    return ConvertQuestionVarNode(source.AsQuestionVar, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Value:
                    return ConvertValueNode(source.AsValue, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.FuzzyLogicValue:
                    return ConvertFuzzyLogicValueNode(source.AsFuzzyLogicValue, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                case KindOfExpressionNode.Fact:
                    return ConvertFactNode(source.AsFact, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static ResolverForOperatorAndExpressionNode ConvertAndNode(OperatorAndExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorAndExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Left = ConvertExpressionNode(source.Left, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            result.Right = ConvertExpressionNode(source.Right, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForOperatorOrExpressionNode ConvertOrNode(OperatorOrExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorOrExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Left = ConvertExpressionNode(source.Left, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            result.Right = ConvertExpressionNode(source.Right, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForOperatorNotExpressionNode ConvertNotNode(OperatorNotExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForOperatorNotExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Left = ConvertExpressionNode(source.Left, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForRelationExpressionNode ConvertRelationNode(RelationExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForRelationExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.CountParams = source.Params.Count;
            result.IsQuestion = source.IsQuestion;
            var parametersList = new List<ResolverForBaseExpressionNode>();
            var varsInfoList = new List<QueryExecutingCardAboutVar>();
            var knownInfoList = new List<QueryExecutingCardAboutKnownInfo>();
            var i = 0;
            foreach (var param in source.Params)
            {
                var resultParam = ConvertExpressionNode(param, targetPart, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
                parametersList.Add(resultParam);
                var kindOfParam = param.Kind;
                switch(kindOfParam)
                {
                    case KindOfExpressionNode.Concept:
                        {
                            var originParam = param.AsConcept;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Key = originParam.Key;
                            knownInfoList.Add(knownInfo);
                        }
                        break;

                    case KindOfExpressionNode.EntityRef:
                        {
                            var originParam = param.AsEntityRef;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Key = originParam.Key;
                            knownInfoList.Add(knownInfo);
                        }
                        break;

                    case KindOfExpressionNode.EntityCondition:
                        {
                            var originParam = param.AsEntityCondition;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Key = originParam.Key;
                            knownInfoList.Add(knownInfo);
                        }
                        break;

                    case KindOfExpressionNode.Var:
                        {
                            var originParam = param.AsVar;
                            var varInfo = new QueryExecutingCardAboutVar();
                            varInfo.KeyOfVar = originParam.Key;
                            varInfo.Position = i;
                            varsInfoList.Add(varInfo);
                        }
                        break;

                    case KindOfExpressionNode.QuestionVar:
                        {
                            var originParam = param.AsQuestionVar;
                            var varInfo = new QueryExecutingCardAboutVar();
                            varInfo.KeyOfVar = originParam.Key;
                            varInfo.Position = i;
                            varsInfoList.Add(varInfo);
                        }
                        break;
                    case KindOfExpressionNode.Value:
                        {
                            var originParam = param.AsValue;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Value = originParam.Value;
                            knownInfoList.Add(knownInfo);
                        }
                        break;

                    case KindOfExpressionNode.FuzzyLogicValue:
                        {
                            var originParam = param.AsFuzzyLogicValue;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Value = originParam.Value;
                            knownInfoList.Add(knownInfo);
                        }
                        break;

                    case KindOfExpressionNode.Fact:
                        {
                            var originParam = param.AsFact;
                            var knownInfo = new QueryExecutingCardAboutKnownInfo();
                            knownInfo.Kind = kindOfParam;
                            knownInfo.Expression = param;
                            knownInfo.Position = i;
                            knownInfo.Key = originParam.Key;
                            knownInfoList.Add(knownInfo);
                        }
                        break;
                }
                i++;
            }
            result.Params = parametersList;
            result.VarsInfoList = varsInfoList;
            result.KnownInfoList = knownInfoList;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            contextOfConvertingExpressionNode.RelationsList.Add(result);
            return result;
        }

        private static ResolverForConceptExpressionNode ConvertConceptNode(ConceptExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForConceptExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForEntityRefExpressionNode ConvertEntityRefNode(EntityRefExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForEntityRefExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForEntityConditionExpressionNode ConvertEntityConditionNode(EntityConditionExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForEntityConditionExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.VariableKey = source.VariableKey;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForVarExpressionNode ConvertVarNode(VarExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForVarExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            contextOfConvertingExpressionNode.VarsList.Add(source);
            return result;
        }

        private static ResolverForQuestionVarExpressionNode ConvertQuestionVarNode(QuestionVarExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForQuestionVarExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            contextOfConvertingExpressionNode.QuestionVarsList.Add(source);
            return result;
        }

        private static ResolverForValueExpressionNode ConvertValueNode(ValueExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForValueExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Value = source.Value;
            result.KindOfValueType = source.KindOfValueType;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForFuzzyLogicValueExpressionNode ConvertFuzzyLogicValueNode(FuzzyLogicValueExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForFuzzyLogicValueExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Value = source.Value;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static ResolverForFactExpressionNode ConvertFactNode(FactExpressionNode source, IndexedRulePart targetPart, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context, ContextOfConvertingExpressionNode contextOfConvertingExpressionNode)
        {
            var result = new ResolverForFactExpressionNode();
            result.ConcreteOrigin = source;
            result.RulePart = targetPart;
            result.RuleInstance = parentIndexedRuleInstance;
            result.Key = source.Key;
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);
            return result;
        }

        private static IndexedIfConditionsPart ConvertIfConditionsPart(IfConditionsPart source, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context)
        {
            if(context.IfConditionsPartDict.ContainsKey(source))
            {
                return context.IfConditionsPartDict[source];
            }

            var result = new IndexedIfConditionsPart();
            context.IfConditionsPartDict[source] = result;

            result.Origin = source;

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, null, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

#if DEBUG
            //LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif
            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);

            return result;
        }

        private static IndexedNotContradictPart ConvertNotContradictPart(NotContradictPart source, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context)
        {
            if(context.NotContradictPartDict.ContainsKey(source))
            {
                return context.NotContradictPartDict[source];
            }

            var result = new IndexedNotContradictPart();
            context.NotContradictPartDict[source] = result;

            result.Origin = source;

            var contextOfConvertingExpressionNode = new ContextOfConvertingExpressionNode();

            result.Expression = ConvertExpressionNode(source.Expression, null, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);

#if DEBUG
            //LogInstance.Log($"contextOfConvertingExpressionNode = {contextOfConvertingExpressionNode}");
#endif

            result.Annotations = ConvertAnnotations(source.Annotations, parentIndexedRuleInstance);

            return result;
        }

        private static IndexedAccessPolicyToFactModality ConvertAccessPolicyToFactModality(AccessPolicyToFactModality source, IndexedRuleInstance parentIndexedRuleInstance, ContextOfConvertingToIndexed context)
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

                result.Expression = ConvertExpressionNode(source.Expression, null, parentIndexedRuleInstance, context, contextOfConvertingExpressionNode);
            }
            return result;
        }
            
        private static IndexedDesirableFuzzyModality ConvertDesirableFuzzyModality(DesirableFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if(context.DesirableFuzzyModalityDict.ContainsKey(source))
            {
                return context.DesirableFuzzyModalityDict[source];
            }

            var result = new IndexedDesirableFuzzyModality();
            context.DesirableFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedNecessityFuzzyModality ConvertNecessityFuzzyModality(NecessityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if(context.NecessityFuzzyModalityDict.ContainsKey(source))
            {
                return context.NecessityFuzzyModalityDict[source];
            }

            var result = new IndexedNecessityFuzzyModality();
            context.NecessityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedImperativeFuzzyModality ConvertImperativeFuzzyModality(ImperativeFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if(context.ImperativeFuzzyModalityDict.ContainsKey(source))
            {
                return context.ImperativeFuzzyModalityDict[source];
            }
            var result = new IndexedImperativeFuzzyModality();
            context.ImperativeFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedIntentionallyFuzzyModality ConvertIntentionallyFuzzyModality(IntentionallyFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if(context.IntentionallyFuzzyModalityDict.ContainsKey(source))
            {
                return context.IntentionallyFuzzyModalityDict[source];
            }
            var result = new IndexedIntentionallyFuzzyModality();
            context.IntentionallyFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedPriorityFuzzyModality ConvertPriorityFuzzyModality(PriorityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if(context.PriorityFuzzyModalityDict.ContainsKey(source))
            {
                return context.PriorityFuzzyModalityDict[source];
            }
            var result = new IndexedPriorityFuzzyModality();
            context.PriorityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedRealityFuzzyModality ConvertRealityFuzzyModality(RealityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if (context.RealityFuzzyModalityDict.ContainsKey(source))
            {
                return context.RealityFuzzyModalityDict[source];
            }
            var result = new IndexedRealityFuzzyModality();
            context.RealityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedProbabilityFuzzyModality ConvertProbabilityFuzzyModality(ProbabilityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if (context.ProbabilityFuzzyModalityDict.ContainsKey(source))
            {
                return context.ProbabilityFuzzyModalityDict[source];
            }
            var result = new IndexedProbabilityFuzzyModality();
            context.ProbabilityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedCertaintyFactorFuzzyModality ConvertCertaintyFactorFuzzyModality(CertaintyFactorFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if (context.CertaintyFactorFuzzyModalityDict.ContainsKey(source))
            {
                return context.CertaintyFactorFuzzyModalityDict[source];
            }
            var result = new IndexedCertaintyFactorFuzzyModality();
            context.CertaintyFactorFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedMoralQualityFuzzyModality ConvertMoralQualityFuzzyModality(MoralQualityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if (context.MoralQualityFuzzyModalityDict.ContainsKey(source))
            {
                return context.MoralQualityFuzzyModalityDict[source];
            }
            var result = new IndexedMoralQualityFuzzyModality();
            context.MoralQualityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static IndexedQuantityQualityFuzzyModality ConvertQuantityQualityFuzzyModality(QuantityQualityFuzzyModality source, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            if (context.QuantityQualityFuzzyModalityDict.ContainsKey(source))
            {
                return context.QuantityQualityFuzzyModalityDict[source];
            }
            var result = new IndexedQuantityQualityFuzzyModality();
            context.QuantityQualityFuzzyModalityDict[source] = result;

            FillIndexedFuzzyModality(source, result, context, parent);
            return result;
        }

        private static void FillIndexedFuzzyModality(FuzzyModality source, IndexedFuzzyModality dest, ContextOfConvertingToIndexed context, IndexedRuleInstance parent)
        {
            dest.Parent = context.RuleInstancesDict[source.Parent];
            dest.Origin = source;
            dest.Annotations = ConvertAnnotations(source.Annotations, parent);
        }

        private static IList<IndexedLogicalAnnotation> ConvertAnnotations(IList<LogicalAnnotation> source, IndexedRuleInstance parent)
        {
            var result = new List<IndexedLogicalAnnotation>();

            if(source.IsEmpty())
            {
                return result;
            }

            var dataSource = parent.DataSource;

            foreach(var sourceItem in source)
            {
                var resultItem = new IndexedLogicalAnnotation();
                resultItem.Origin = sourceItem;

                resultItem.RuleInstanceKey = sourceItem.RuleInstanceKey;
                resultItem.Annotations = ConvertAnnotations(sourceItem.Annotations, parent);
                result.Add(resultItem);
            }

            return result;
        }
    }
}
