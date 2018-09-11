using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.VariantsConverting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public static class ConvertorASTNodeOfLogicalQueryToRuleInstance
    {
        public static RuleInstancePackage Convert(ASTNodeOfLogicalQuery node, IEntityDictionary entityDictionary)
        {
            var context = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            context.EntityDictionary = entityDictionary;

            var result = new RuleInstancePackage();

            result.MainRuleInstance = NConvertFact(node, context);

            result.AllRuleInstances = context.ResultsList;

            return result;
        }

        private static RuleInstance NConvertFact(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var kindOfNode = node.Kind;

            switch(kindOfNode)
            {
                case KindOfASTNodeOfLogicalQuery.Fact:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }

            var entityDictionary = context.EntityDictionary;

            var result = new RuleInstance();
            result.Kind = KindOfRuleInstance.Fact;
            var nameOfFact = NamesHelper.CreateEntityName();
            result.Name = nameOfFact;
            result.Key = entityDictionary.GetKey(nameOfFact);

            var initAccessPoliciesList = node.AccessPolicy;

            if(initAccessPoliciesList == null)
            {
                var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                result.AccessPolicyToFactModality = accessPolicyToFactModalityList;
            }
            else
            {
                var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

                foreach (var initAccessPolicy in initAccessPoliciesList)
                {
                    var accessPolicyToFactModality = ConvertAccessPolicy(initAccessPolicy, result, context);
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
              
                result.AccessPolicyToFactModality = accessPolicyToFactModalityList;
            }

            if (node.Part_1 != null)
            {
                result.Part_1 = new RulePart();
            }
            
            if(node.Part_2 != null)
            {
                result.Part_2 = new RulePart();
            }

            if (result.Part_1 != null)
            {
                NConvertRulePart(node.Part_1, result, result.Part_1, result.Part_2, context);
            }

            if (result.Part_2 != null)
            {
                NConvertRulePart(node.Part_2, result, result.Part_2, result.Part_1, context);
            }

            if (!node.AnnotationsList.IsEmpty())
            {
                var completeAnnotationsList = new List<LogicalAnnotation>();

                foreach (var initAnnotation in node.AnnotationsList)
                {
                    var annotationsList = NConvertAnnotation(initAnnotation, context);
                    completeAnnotationsList.AddRange(annotationsList);
                }

                result.Annotations = completeAnnotationsList;
            }

            context.ResultsList.Add(result);

            return result;
        }

        private static EntitiesConditions GetEntitiesConditions(RuleInstance ruleInstance)
        {
            if(ruleInstance.EntitiesConditions != null)
            {
                return ruleInstance.EntitiesConditions;
            }

            var result = new EntitiesConditions();
            result.Items = new List<EntityConditionItem>();
            ruleInstance.EntitiesConditions = result;
            return result;
        }

        private static AccessPolicyToFactModality ConvertAccessPolicy(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var accessPolicyToFactModality = new AccessPolicyToFactModality();
            accessPolicyToFactModality.Kind = node.KindOfAccessPolicy;

            if(accessPolicyToFactModality.Kind == KindOfAccessPolicyToFact.IfCondition)
            {
                accessPolicyToFactModality.Expression = NConvertExpression(node.Expression, parent, context);
            }

            return accessPolicyToFactModality;
        }

        private static void NConvertRulePart(ASTNodeOfLogicalQuery node, RuleInstance parent, RulePart dest, RulePart nextPart, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var kindOfNode = node.Kind;

            switch (kindOfNode)
            {
                case KindOfASTNodeOfLogicalQuery.RulePart:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }

            dest.Parent = parent;
            dest.NextPart = nextPart;
            dest.IsActive = node.IsActivePart;

            var expressionNode = node.Expression;

            dest.Expression = NConvertExpression(expressionNode, parent, context);

            if (!node.AnnotationsList.IsEmpty())
            {
                var completeAnnotationsList = new List<LogicalAnnotation>();

                foreach (var initAnnotation in node.AnnotationsList)
                {
                    var annotationsList = NConvertAnnotation(initAnnotation, context);
                    completeAnnotationsList.AddRange(annotationsList);
                }

                dest.Annotations = completeAnnotationsList;
            }
        }

        private static BaseExpressionNode NConvertExpression(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            var secondKindOfExpressionNode = node.SecondaryKind;

            switch (secondKindOfExpressionNode)
            {
                case SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression:
                    return NConvertStandardExpression(node, parent, context);

                case SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition:
                    return NConvertEntityConditionExpression(node, parent, context);

                case SecondaryKindOfASTNodeOfLogicalQuery.SimpleConcept:
                    return NConvertSimpleConcept(node, parent, context);

                default:
                    throw new ArgumentOutOfRangeException(nameof(secondKindOfExpressionNode), secondKindOfExpressionNode, null);
            }
        }

        private static BaseExpressionNode NConvertStandardExpression(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var kindOfNode = node.Kind;

            switch (kindOfNode)
            {
                case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                    {
                        var kindOfOperator = node.KindOfOperator;

                        switch(kindOfOperator)
                        {
                            case KindOfOperatorOfASTNodeOfLogicalQuery.And:
                                return NConvertStandardExpressionAndNode(node, parent, context);

                            case KindOfOperatorOfASTNodeOfLogicalQuery.Or:
                                return NConvertStandardExpressionOrNode(node, parent, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfOperator), kindOfOperator, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var kindOfOperator = node.KindOfOperator;

                        switch (kindOfOperator)
                        {
                            case KindOfOperatorOfASTNodeOfLogicalQuery.Not:
                                return NConvertStandardExpressionNotNode(node, parent, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfOperator), kindOfOperator, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.Relation:
                    return NConvertStandardExpressionRelationNode(node, parent, context);

                case KindOfASTNodeOfLogicalQuery.Concept:
                    return NConvertStandardExpressionConceptNode(node, context);

                case KindOfASTNodeOfLogicalQuery.EntityRef:
                    return NConvertStandardExpressionEntityNode(node, context);

                case KindOfASTNodeOfLogicalQuery.Var:
                    return NConvertStandardExpressionVarNode(node, context);

                case KindOfASTNodeOfLogicalQuery.Fact:
                    {
                        var secondaryKind = node.SecondaryKind;

                        switch(secondaryKind)
                        {
                            case SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition:
                                return NConvertStandardExpressionEntityRefNode(node, parent, context);

                            case SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression:
                            case SecondaryKindOfASTNodeOfLogicalQuery.SimpleConcept:
                                return NConvertStandardExpressionFactNode(node, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(secondaryKind), secondaryKind, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.QuestionParam:
                    return NConvertStandardExpressionQuestionVarNode(node, parent, context);

                case KindOfASTNodeOfLogicalQuery.Value:
                    return NConvertStandardExpressionValueNode(node, context);

                case KindOfASTNodeOfLogicalQuery.LogicalValue:
                    return NConvertStandardExpressionLogicalValueNode(node, context);

                case KindOfASTNodeOfLogicalQuery.BindedParam:
                    return NConvertStandardExpressionBindedParamNode(node, context);

                case KindOfASTNodeOfLogicalQuery.StubParam:
                    return NConvertStandardExpressionStubNode(node, context);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }
        }

        private static BaseExpressionNode NConvertStandardExpressionAndNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorAndExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, parent, context);
            result.Right = NConvertStandardExpression(node.Right, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionOrNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorOrExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, parent, context);
            result.Right = NConvertStandardExpression(node.Right, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionNotNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorNotExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionRelationNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var result = new RelationExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            result.IsQuestion = node.IsQuestion;
            result.Params = new List<BaseExpressionNode>();

            foreach (var initParamInfo in node.ParamsList)
            {
                result.Params.Add(NConvertStandardExpression(initParamInfo, parent, context));
            }

            if(!node.VarsList.IsEmpty())
            {
                var linkedVarsList = new List<VarExpressionNode>();
                result.LinkedVars = linkedVarsList;

                foreach (var initLinkedVar in node.VarsList)
                {
                    var linkedVar = NConvertStandardExpression(initLinkedVar, parent, context);
                    linkedVarsList.Add(linkedVar.AsVar);
                }
            }

            FillAnnotationForExpression(result, node, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionConceptNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            var result = new ConceptExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionEntityNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            var result = new EntityRefExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionVarNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var result = new VarExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionFactNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = entityDictionary;

            var factNode = NConvertFact(node, newContext);

            var factName = factNode.Name;
            var factKey = entityDictionary.GetKey(factName);

#if DEBUG
            //LogInstance.Log($"factName = {factName}");
#endif

            var factExprNode = new FactExpressionNode();
            factExprNode.Name = factName;
            factExprNode.Key = factKey;
            FillAnnotationForExpression(factExprNode, node, context);

            return factExprNode;
        }

        private static BaseExpressionNode NConvertStandardExpressionEntityRefNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = entityDictionary;

            var factNode = NConvertFact(node, newContext);
            factNode.Kind = KindOfRuleInstance.EntityCondition;

            var factName = factNode.Name;
            var factKey = entityDictionary.GetKey(factName);

#if DEBUG
            //LogInstance.Log($"factName = {factName}");
#endif

            var entitiesConditions = GetEntitiesConditions(parent);
            var items = entitiesConditions.Items;

            var varIndex = items.Count;

            var varName = string.Empty;

            if (varIndex == 0)
            {
                varName = "#@x";
            }
            else
            {
                varName = $"#@x{varIndex}";
            }

#if DEBUG
            //LogInstance.Log($"varName = {varName}");
#endif

            var varKey = entityDictionary.GetKey(varName);

            var item = new EntityConditionItem();
            items.Add(item);

            item.Name = factName;
            item.Key = factKey;
            item.VariableName = varName;
            item.VariableKey = varKey;

            var result = new EntityConditionExpressionNode();
            result.Name = factName;
            result.Key = factKey;
            result.VariableName = varName;
            result.VariableKey = varKey;
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionQuestionVarNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            parent.Kind = KindOfRuleInstance.QuestionVars;

            var result = new QuestionVarExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var valueNode = new ValueExpressionNode();
            valueNode.Value = node.ObjValue;
            valueNode.KindOfValueType = node.KindOfValueType;
            FillAnnotationForExpression(valueNode, node, context);
            return valueNode;
        }

        private static BaseExpressionNode NConvertStandardExpressionLogicalValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var valueNode = new FuzzyLogicValueExpressionNode();
            valueNode.Value = (float)node.ObjValue;
            FillAnnotationForExpression(valueNode, node, context);
            return valueNode;
        }

        private static BaseExpressionNode NConvertStandardExpressionBindedParamNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var targetExpressionNode = VariantsConvertor.ConvertVariantToExpressionNode(node.BindedValue);

#if DEBUG
            //LogInstance.Log($"targetExpressionNode = {targetExpressionNode}");
#endif

            FillAnnotationForExpression(targetExpressionNode, node, context);
            return targetExpressionNode;
        }

        private static BaseExpressionNode NConvertStandardExpressionStubNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new ParamStubExpressionNode();
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static void FillAnnotationForExpression(BaseExpressionNode dest, ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            if (!node.AnnotationsList.IsEmpty())
            {
                var completeAnnotationsList = new List<LogicalAnnotation>();

                foreach (var initAnnotation in node.AnnotationsList)
                {
                    var annotationsList = NConvertAnnotation(initAnnotation, context);
                    completeAnnotationsList.AddRange(annotationsList);
                }

                dest.Annotations = completeAnnotationsList;
            }
        }

        private static BaseExpressionNode NConvertEntityConditionExpression(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var kindOfNode = node.Kind;

            switch (kindOfNode)
            {
                case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                    {
                        var kindOfOperator = node.KindOfOperator;

                        switch (kindOfOperator)
                        {
                            case KindOfOperatorOfASTNodeOfLogicalQuery.And:
                                return NConvertEntityConditionAndNode(node, parent, context);

                            case KindOfOperatorOfASTNodeOfLogicalQuery.Or:
                                return NConvertEntityConditionOrNode(node, parent, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfOperator), kindOfOperator, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var kindOfOperator = node.KindOfOperator;

                        switch (kindOfOperator)
                        {
                            case KindOfOperatorOfASTNodeOfLogicalQuery.Not:
                                return NConvertEntityConditionNotNode(node, parent, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfOperator), kindOfOperator, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.Condition:
                    return NConvertEntityConditionConditionNode(node, parent, context);

                case KindOfASTNodeOfLogicalQuery.Concept:
                    return NConvertEntityConditionConceptNode(node, context);

                case KindOfASTNodeOfLogicalQuery.EntityRef:
                    return NConvertEntityConditionEntityNode(node, context);

                case KindOfASTNodeOfLogicalQuery.Fact:
                    {
                        var secondaryKind = node.SecondaryKind;

                        switch (secondaryKind)
                        {
                            case SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition:
                                return NConvertEntityConditionEntityRefNode(node, parent, context);

                            case SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression:
                            case SecondaryKindOfASTNodeOfLogicalQuery.SimpleConcept:
                                return NConvertEntityConditionFactNode(node, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(secondaryKind), secondaryKind, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.Value:
                    return NConvertEntityConditionValueNode(node, context);

                case KindOfASTNodeOfLogicalQuery.LogicalValue:
                    return NConvertEntityConditionLogicalValueNode(node, context);

                case KindOfASTNodeOfLogicalQuery.BindedParam:
                    return NConvertEntityConditionBindedParamNode(node, context);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }
        }

        private static BaseExpressionNode NConvertEntityConditionAndNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorAndExpressionNode();
            result.Left = NConvertEntityConditionExpression(node.Left, parent, context);
            result.Right = NConvertEntityConditionExpression(node.Right, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionOrNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorOrExpressionNode();
            result.Left = NConvertEntityConditionExpression(node.Left, parent, context);
            result.Right = NConvertEntityConditionExpression(node.Right, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionNotNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorNotExpressionNode();
            result.Left = NConvertEntityConditionExpression(node.Left, parent, context);

            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionConditionNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var relationName = node.Name;

            if(relationName == "class")
            {
                return NConvertEntityConditionConditionNodeAsClass(node, context);
            }

            var entityDictionary = context.EntityDictionary;

            var propertyValue = node.PropertyValue;

#if DEBUG
            //LogInstance.Log($"relationName = {relationName}");
            //LogInstance.Log($"propertyValue = {propertyValue}");
#endif

            var varName = "@x";
            var varKey = entityDictionary.GetKey(varName);

            var varNode = new VarExpressionNode();
            varNode.Name = varName;
            varNode.Key = varKey;

            var relationKey = entityDictionary.GetKey(relationName);

            var secondParamOfRelation = NConvertEntityConditionExpression(propertyValue, parent, context);

            var relationNode = new RelationExpressionNode();
            relationNode.Name = relationName;
            relationNode.Key = relationKey;
            relationNode.Params = new List<BaseExpressionNode>() { varNode, secondParamOfRelation };

            FillAnnotationForExpression(relationNode, node, context);

            return relationNode;
        }

        private static BaseExpressionNode NConvertEntityConditionConditionNodeAsClass(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            var propertyValue = node.PropertyValue;

#if DEBUG
            //LogInstance.Log($"propertyValue = {propertyValue}");
#endif

            var relationName = propertyValue.Name;

#if DEBUG
            //LogInstance.Log($"relationName = {relationName}");
#endif

            var varName = "@x";
            var varKey = entityDictionary.GetKey(varName);

            var varNode = new VarExpressionNode();
            varNode.Name = varName;
            varNode.Key = varKey;

            var relationKey = entityDictionary.GetKey(relationName);

            var relationNode = new RelationExpressionNode();
            relationNode.Name = relationName;
            relationNode.Key = relationKey;
            relationNode.Params = new List<BaseExpressionNode>() { varNode };

            FillAnnotationForExpression(relationNode, node, context);

            return relationNode;
        }

        private static BaseExpressionNode NConvertEntityConditionConceptNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var result = new ConceptExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionEntityNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            var result = new EntityRefExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionFactNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif
            var entityDictionary = context.EntityDictionary;

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = entityDictionary;

            var factNode = NConvertFact(node, newContext);

            var factName = factNode.Name;
            var factKey = entityDictionary.GetKey(factName);

#if DEBUG
            //LogInstance.Log($"factName = {factName}");
#endif

            var factExprNode = new FactExpressionNode();
            factExprNode.Name = factName;
            factExprNode.Key = factKey;
            FillAnnotationForExpression(factExprNode, node, context);

            return factExprNode;
        }

        private static BaseExpressionNode NConvertEntityConditionEntityRefNode(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = entityDictionary;

            var factNode = NConvertFact(node, newContext);
            factNode.Kind = KindOfRuleInstance.EntityCondition;

            var factName = factNode.Name;
            var factKey = entityDictionary.GetKey(factName);

#if DEBUG
            //LogInstance.Log($"factName = {factName}");
#endif

            var entitiesConditions = GetEntitiesConditions(parent);
            var items = entitiesConditions.Items;

            var varIndex = items.Count;

            var varName = string.Empty;

            if(varIndex == 0)
            {
                varName = "#@x";
            }
            else
            {
                varName = $"#@x{varIndex}";
            }

#if DEBUG
            //LogInstance.Log($"varName = {varName}");
#endif

            var varKey = entityDictionary.GetKey(varName);

            var item = new EntityConditionItem();
            items.Add(item);

            item.Name = factName;
            item.Key = factKey;
            item.VariableName = varName;
            item.VariableKey = varKey;

            var result = new EntityConditionExpressionNode();
            result.Name = factName;
            result.Key = factKey;
            result.VariableName = varName;
            result.VariableKey = varKey;
            FillAnnotationForExpression(result, node, context);
            return result;
        }

        private static BaseExpressionNode NConvertEntityConditionValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var valueNode = new ValueExpressionNode();
            valueNode.Value = node.ObjValue;
            valueNode.KindOfValueType = node.KindOfValueType;
            FillAnnotationForExpression(valueNode, node, context);
            return valueNode;
        }

        private static BaseExpressionNode NConvertEntityConditionLogicalValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            var valueNode = new FuzzyLogicValueExpressionNode();
            valueNode.Value = (float)node.ObjValue;
            FillAnnotationForExpression(valueNode, node, context);
            return valueNode;
        }

        private static BaseExpressionNode NConvertEntityConditionBindedParamNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var targetExpressionNode = VariantsConvertor.ConvertVariantToExpressionNode(node.BindedValue);

#if DEBUG
            //LogInstance.Log($"targetExpressionNode = {targetExpressionNode}");
#endif

            FillAnnotationForExpression(targetExpressionNode, node, context);
            return targetExpressionNode;
        }

        private static BaseExpressionNode NConvertSimpleConcept(ASTNodeOfLogicalQuery node, RuleInstance parent, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var varName = "@x";
            var varKey = entityDictionary.GetKey(varName);

            var varNode = new VarExpressionNode();
            varNode.Name = varName;
            varNode.Key = varKey;

            var relationName = node.Name;
            var relationKey = entityDictionary.GetKey(relationName);

            var relationNode = new RelationExpressionNode();
            relationNode.Name = relationName;
            relationNode.Key = relationKey;
            relationNode.Params = new List<BaseExpressionNode>() { varNode };

            FillAnnotationForExpression(relationNode, node, context);

            return relationNode;
        }

        private static List<LogicalAnnotation> NConvertAnnotation(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            var resultsList = new List<LogicalAnnotation>();

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = context.EntityDictionary;

            var annotationInstance = NConvertFact(node, newContext);
            annotationInstance.Kind = KindOfRuleInstance.Annotation;

            var initFactsList = newContext.ResultsList;

            var resultItem = new LogicalAnnotation();
            resultItem.RuleInstanceKey = annotationInstance.Key;
            resultsList.Add(resultItem);

            context.ResultsList.AddRange(initFactsList);
            
            return resultsList;
        }
    }
}
