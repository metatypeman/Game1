using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public static class ConvertorASTNodeOfLogicalQueryToRuleInstance
    {
        public static ICGStorage Convert(ASTNodeOfLogicalQuery node, ContextOfCGStorage context)
        {
            var localContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            localContext.EntityDictionary = context.EntityDictionary;

            NConvertFact(node, localContext);

            var resultFactsList = localContext.ResultsList;

            var result = new PassiveListGCStorage(context, resultFactsList);

            foreach (var resultFact in resultFactsList)
            {
                resultFact.DataSource = result;
            }

            return result;
        }

        private static void NConvertFact(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
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
            var nameOfFact = NamesHelper.CreateEntityName();
            result.Name = nameOfFact;
            result.Key = entityDictionary.GetKey(nameOfFact);

            if(node.Part_1 != null)
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
        }

        private static void NConvertRulePart(ASTNodeOfLogicalQuery node, RuleInstance parent, RulePart dest, RulePart nextPart, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
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

            var secondKindOfExpressionNode = expressionNode.SecondaryKind;

            switch(secondKindOfExpressionNode)
            {
                case SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression:
                    dest.Expression = NConvertStandardExpression(expressionNode, context);
                    break;

                case SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition:
                    dest.Expression = NConvertEntityConditionExpression(expressionNode, context);
                    break;

                case SecondaryKindOfASTNodeOfLogicalQuery.SimpleConcept:
                    dest.Expression = NConvertSimpleConcept(expressionNode, context);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(secondKindOfExpressionNode), secondKindOfExpressionNode, null);
            }

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

        private static BaseExpressionNode NConvertStandardExpression(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
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
                                return NConvertStandardExpressionAndNode(node, context);

                            case KindOfOperatorOfASTNodeOfLogicalQuery.Or:
                                return NConvertStandardExpressionOrNode(node, context);

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
                                return NConvertStandardExpressionNotNode(node, context);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfOperator), kindOfOperator, null);
                        }
                    }

                case KindOfASTNodeOfLogicalQuery.Relation:
                    return NConvertStandardExpressionRelationNode(node, context);

                case KindOfASTNodeOfLogicalQuery.Concept:
                    return NConvertStandardExpressionConceptNode(node, context);

                case KindOfASTNodeOfLogicalQuery.Var:
                    return NConvertStandardExpressionVarNode(node, context);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }

            /*
            NConvertStandardExpressionFactNode
        NConvertStandardExpressionEntityRefNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        NConvertStandardExpressionQuestionVarNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        NConvertStandardExpressionValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
             */
        }

        private static BaseExpressionNode NConvertStandardExpressionAndNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorAndExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, context);
            result.Right = NConvertStandardExpression(node.Right, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionOrNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorOrExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, context);
            result.Right = NConvertStandardExpression(node.Right, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionNotNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            var result = new OperatorNotExpressionNode();
            result.Left = NConvertStandardExpression(node.Left, context);

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionRelationNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            var entityDictionary = context.EntityDictionary;

            var result = new RelationExpressionNode();
            result.Name = node.Name;
            result.Key = entityDictionary.GetKey(node.Name);
            result.Params = new List<BaseExpressionNode>();

            foreach (var initParamInfo in node.ParamsList)
            {
                result.Params.Add(NConvertStandardExpression(initParamInfo, context));
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

            return result;
        }

        private static BaseExpressionNode NConvertStandardExpressionConceptNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertStandardExpressionVarNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertStandardExpressionFactNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertStandardExpressionEntityRefNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertStandardExpressionQuestionVarNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertStandardExpressionValueNode(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertEntityConditionExpression(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static BaseExpressionNode NConvertSimpleConcept(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static List<LogicalAnnotation> NConvertAnnotation(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            var resultsList = new List<LogicalAnnotation>();

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = context.EntityDictionary;

            NConvertFact(node, newContext);

            var initFactsList = newContext.ResultsList;

            foreach(var initFact in initFactsList)
            {
                var resultItem = new LogicalAnnotation();
                resultItem.RuleInstance = initFact;
                resultsList.Add(resultItem);
            }
            
            return resultsList;
        }
    }
}
