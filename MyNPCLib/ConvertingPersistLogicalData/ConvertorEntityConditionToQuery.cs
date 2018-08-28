using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.ConvertingPersistLogicalData
{
    public static class ConvertorEntityConditionToQuery
    {
        public static ICGStorage Convert(ICGStorage source)
        {
            var mainRuleInstance = source.MainRuleInstance;

            if(mainRuleInstance == null)
            {
                throw new ArgumentNullException(nameof(mainRuleInstance));
            }

#if DEBUG
            var debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");
#endif

            var newMainRuleInstance = mainRuleInstance.Clone();

            var entityDictionary = source.Context.EntityDictionary;

            var keysOfAdditionalRuleInstances = new List<ulong>();

            ReplaceVarToQuestionParam(newMainRuleInstance, entityDictionary, ref keysOfAdditionalRuleInstances);

#if DEBUG
            LogInstance.Log($"keysOfAdditionalRuleInstances.Count = {keysOfAdditionalRuleInstances.Count}");
#endif
            var keyOfMainRuleInstance = newMainRuleInstance.Key;
            var otherRuleInstancesList = source.AllRuleInstances.Where(p => p.Key != keyOfMainRuleInstance).ToList();

#if DEBUG
            LogInstance.Log($"otherRuleInstancesList.Count = {otherRuleInstancesList.Count}");
#endif

            FindAllKeysOfAdditionalRuleInstances(otherRuleInstancesList, ref keysOfAdditionalRuleInstances);

#if DEBUG
            LogInstance.Log($"keysOfAdditionalRuleInstances.Count = {keysOfAdditionalRuleInstances.Count}");
#endif

            var targetRuleInstancesList = CopyAllOfTargetAdditionalRuleInstances(otherRuleInstancesList, keysOfAdditionalRuleInstances);

            if(!targetRuleInstancesList.Any(p => p.Key != keyOfMainRuleInstance))
            {
                targetRuleInstancesList.Add(newMainRuleInstance);
            }

            var package = new RuleInstancePackage();
            package.MainRuleInstance = newMainRuleInstance;
            package.AllRuleInstances = targetRuleInstancesList;

            var queryStorage = new QueryCGStorage(source.Context, package);
            return queryStorage;
        }

        private static void ReplaceVarToQuestionParam(RuleInstance ruleInstance, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

            if(ruleInstance.Part_1 != null)
            {
                ReplaceVarToQuestionParam(ruleInstance.Part_1, entityDictionary, ref keysOfAdditionalRuleInstances);
            }

            if(ruleInstance.Part_2 != null)
            {
                ReplaceVarToQuestionParam(ruleInstance.Part_2, entityDictionary, ref keysOfAdditionalRuleInstances);
            }

            FillAllKeysOfAnnotation(ruleInstance.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParam(RulePart rulePart, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"rulePart = {rulePart}");
#endif

            ReplaceVarToQuestionParamInExpression(rulePart.Expression, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
            FillAllKeysOfAnnotation(rulePart.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInExpression(BaseExpressionNode expressionNode, RelationExpressionNode parentRelation, int paramIndex, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var kind = expressionNode.Kind;

            switch(kind)
            {
                case KindOfExpressionNode.And:
                    ReplaceVarToQuestionParamInOperatorAndExpression(expressionNode.AsOperatorAnd, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Or:
                    ReplaceVarToQuestionParamInOperatorOrExpression(expressionNode.AsOperatorOr, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Not:
                    ReplaceVarToQuestionParamInOperatorNotExpression(expressionNode.AsOperatorNot, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Relation:
                    ReplaceVarToQuestionParamInRelationExpression(expressionNode.AsRelation, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Concept:
                    ReplaceVarToQuestionParamInConceptExpression(expressionNode.AsConcept, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.EntityRef:
                    ReplaceVarToQuestionParamInEntityRefExpression(expressionNode.AsEntityRef, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.EntityCondition:
                    ReplaceVarToQuestionParamInEntityConditionExpression(expressionNode.AsEntityCondition, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Var:
                    ReplaceVarToQuestionParamInVarExpression(expressionNode.AsVar, parentRelation, paramIndex, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.QuestionVar:
                    ReplaceVarToQuestionParamInQuestionVarExpression(expressionNode.AsQuestionVar, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Value:
                    ReplaceVarToQuestionParamInValueExpression(expressionNode.AsValue, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.FuzzyLogicValue:
                    ReplaceVarToQuestionParamInFuzzyLogicValueExpression(expressionNode.AsFuzzyLogicValue, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Fact:
                    ReplaceVarToQuestionParamInFactExpression(expressionNode.AsFact, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;
            }

            FillAllKeysOfAnnotation(expressionNode.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInOperatorNotExpression(OperatorNotExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInOperatorAndExpression(OperatorAndExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInOperatorOrExpression(OperatorOrExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInConceptExpression(ConceptExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInEntityRefExpression(EntityRefExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInEntityConditionExpression(EntityConditionExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInVarExpression(VarExpressionNode expressionNode, RelationExpressionNode parentRelation, int paramIndex, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInQuestionVarExpression(QuestionVarExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInFactExpression(FactExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInRelationExpression(RelationExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInValueExpression(ValueExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInFuzzyLogicValueExpression(FuzzyLogicValueExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            throw new NotImplementedException();
        }

        private static void FindAllKeysOfAdditionalRuleInstances(IList<RuleInstance> rulesInstancesList, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            throw new NotImplementedException();
        }

        private static void FillAllKeysOfAnnotation(IList<LogicalAnnotation> annotations, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            throw new NotImplementedException();
        }
        /*
                    switch(kind)
        {
            case KindOfExpressionNode.And:
            case KindOfExpressionNode.Or:
            case KindOfExpressionNode.Not:
            case KindOfExpressionNode.Relation:
            case KindOfExpressionNode.Concept:
            case KindOfExpressionNode.EntityRef:
            case KindOfExpressionNode.EntityCondition:
            case KindOfExpressionNode.Var:
            case KindOfExpressionNode.QuestionVar:
            case KindOfExpressionNode.Value:
            case KindOfExpressionNode.FuzzyLogicValue:
            case KindOfExpressionNode.Fact:
            case KindOfExpressionNode.ParamStub:

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
         */

        /*
public virtual bool IsUnaryOperator => false;
public virtual UnaryOperatorExpressionNode UnaryOperator => null;
public virtual bool IsOperatorNot => false;
public virtual OperatorNotExpressionNode AsOperatorNot => null;
public virtual bool IsBinaryOperator => false;
public virtual BinaryOperatorExpressionNode AsBinaryOperator => null;
public virtual bool IsOperatorAnd => false;
public virtual OperatorAndExpressionNode AsOperatorAnd => null;
public virtual bool IsOperatorOr => false;
public virtual OperatorOrExpressionNode AsOperatorOr => null;
public virtual bool IsBaseRef => false;
public virtual BaseRefExpressionNode AsBaseRef => null;
public virtual bool IsConcept => false;
public virtual ConceptExpressionNode AsConcept => null;
public virtual bool IsEntityRef => false;
public virtual EntityRefExpressionNode AsEntityRef => null;
public virtual bool IsEntityCondition => false;
public virtual EntityConditionExpressionNode AsEntityCondition => null;
public virtual bool IsVar => false;
public virtual VarExpressionNode AsVar => null;
public virtual bool IsQuestionVar => false;
public virtual QuestionVarExpressionNode AsQuestionVar => null;
public virtual bool IsFact => false;
public virtual FactExpressionNode AsFact => null;
public virtual bool IsRelation => false;
public virtual RelationExpressionNode AsRelation => null;
public virtual bool IsValue => false;
public virtual ValueExpressionNode AsValue => null;
public virtual bool IsFuzzyLogicValue => false;
public virtual FuzzyLogicValueExpressionNode AsFuzzyLogicValue => null;
public virtual bool IsParamStub => false;
public virtual ParamStubExpressionNode AsParamStub => null; 
*/

        /*
                private static void ReplaceVarToQuestionParamInOperatorNotExpression(OperatorNotExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInOperatorAndExpression(OperatorAndExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInOperatorOrExpression(OperatorOrExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInConceptExpression(ConceptExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInEntityRefExpression(EntityRefExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInEntityConditionExpression(EntityConditionExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInVarExpression(VarExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInQuestionVarExpression(QuestionVarExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInFactExpression(FactExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInRelationExpression(RelationExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInValueExpression(ValueExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInFuzzyLogicValueExpression(FuzzyLogicValueExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        private static void ReplaceVarToQuestionParamInParamStubExpression(ParamStubExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
*/

        private static IList<RuleInstance> CopyAllOfTargetAdditionalRuleInstances(IList<RuleInstance> rulesInstancesList, List<ulong> keysOfAdditionalRuleInstances)
        {
            throw new NotImplementedException();
        }
    }
}
