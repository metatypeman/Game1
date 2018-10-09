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
            //var debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            //LogInstance.Log($"debugStr (query) = {debugStr}");
#endif

            var newMainRuleInstance = mainRuleInstance.Clone();
            newMainRuleInstance.Kind = KindOfRuleInstance.QuestionVars;

            var entityDictionary = source.EntityDictionary;

            var keysOfAdditionalRuleInstances = new List<ulong>();

            ReplaceVarToQuestionParam(newMainRuleInstance, entityDictionary, ref keysOfAdditionalRuleInstances);

#if DEBUG
            //LogInstance.Log($"keysOfAdditionalRuleInstances.Count = {keysOfAdditionalRuleInstances.Count}");
#endif
            var keyOfMainRuleInstance = newMainRuleInstance.Key;
            var otherRuleInstancesList = source.AllRuleInstances.Where(p => p.Key != keyOfMainRuleInstance).ToList();

#if DEBUG
            //LogInstance.Log($"otherRuleInstancesList.Count = {otherRuleInstancesList.Count}");
#endif

            IList<RuleInstance> targetRuleInstancesList = null;

            if (keysOfAdditionalRuleInstances.Count == 0)
            {
                targetRuleInstancesList = new List<RuleInstance>();
            }
            else
            {
                keysOfAdditionalRuleInstances = keysOfAdditionalRuleInstances.Distinct().ToList();

                FindAllKeysOfAdditionalRuleInstances(otherRuleInstancesList, ref keysOfAdditionalRuleInstances);

                keysOfAdditionalRuleInstances = keysOfAdditionalRuleInstances.Distinct().ToList();

#if DEBUG
                //LogInstance.Log($"keysOfAdditionalRuleInstances.Count (2) = {keysOfAdditionalRuleInstances.Count}");
#endif

                targetRuleInstancesList = CopyAllOfTargetAdditionalRuleInstances(otherRuleInstancesList, keysOfAdditionalRuleInstances);

                if (!targetRuleInstancesList.Any(p => p.Key != keyOfMainRuleInstance))
                {
                    targetRuleInstancesList.Add(newMainRuleInstance);
                }
            }

            var package = new RuleInstancePackage();
            package.MainRuleInstance = newMainRuleInstance;
            package.AllRuleInstances = targetRuleInstancesList;

            var queryStorage = new QueryCGStorage(entityDictionary, package);
            return queryStorage;
        }

        private static void ReplaceVarToQuestionParam(RuleInstance ruleInstance, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
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
            //LogInstance.Log($"rulePart = {rulePart}");
#endif

            ReplaceVarToQuestionParamInExpression(rulePart.Expression, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
            FillAllKeysOfAnnotation(rulePart.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInExpression(BaseExpressionNode expressionNode, RelationExpressionNode parentRelation, int paramIndex, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
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
                    break;

                case KindOfExpressionNode.EntityRef:
                    break;

                case KindOfExpressionNode.EntityCondition:
                    ReplaceVarToQuestionParamInEntityConditionExpression(expressionNode.AsEntityCondition, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Var:
                    ReplaceVarToQuestionParamInVarExpression(expressionNode.AsVar, parentRelation, paramIndex, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.QuestionVar:
                    break;

                case KindOfExpressionNode.Value:
                    break;

                case KindOfExpressionNode.Fact:
                    ReplaceVarToQuestionParamInFactExpression(expressionNode.AsFact, entityDictionary, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.ParamStub:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            FillAllKeysOfAnnotation(expressionNode.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInOperatorNotExpression(OperatorNotExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            ReplaceVarToQuestionParamInExpression(expressionNode.Left, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInOperatorAndExpression(OperatorAndExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            ReplaceVarToQuestionParamInExpression(expressionNode.Left, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
            ReplaceVarToQuestionParamInExpression(expressionNode.Right, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInOperatorOrExpression(OperatorOrExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");

#endif
            ReplaceVarToQuestionParamInExpression(expressionNode.Left, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
            ReplaceVarToQuestionParamInExpression(expressionNode.Right, null, -1, entityDictionary, ref keysOfAdditionalRuleInstances);
        }

        private static void ReplaceVarToQuestionParamInEntityConditionExpression(EntityConditionExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            keysOfAdditionalRuleInstances.Add(expressionNode.Key);
        }

        private static void ReplaceVarToQuestionParamInVarExpression(VarExpressionNode expressionNode, RelationExpressionNode parentRelation, int paramIndex, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
            //LogInstance.Log($"parentRelation = {parentRelation}");
            //LogInstance.Log($"paramIndex = {paramIndex}");
#endif

            var newExpressionNode = new QuestionVarExpressionNode();
            var nodeName = expressionNode.Name.Replace("@", "?");
            var nodeKey = entityDictionary.GetKey(nodeName);
            newExpressionNode.Name = nodeName;
            newExpressionNode.Key = nodeKey;

            parentRelation.Params[paramIndex] = newExpressionNode;
        }

        private static void ReplaceVarToQuestionParamInFactExpression(FactExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            keysOfAdditionalRuleInstances.Add(expressionNode.Key);
        }

        private static void ReplaceVarToQuestionParamInRelationExpression(RelationExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif
            var paramsList = expressionNode.Params.ToList();

            if (paramsList.Count > 0)
            {
                var n = 0;

                foreach (var paramInfo in paramsList)
                {
                    ReplaceVarToQuestionParamInExpression(paramInfo, expressionNode, n, entityDictionary, ref keysOfAdditionalRuleInstances);
                    n++;
                }
            }
        }

        private static void FindAllKeysOfAdditionalRuleInstances(IList<RuleInstance> rulesInstancesList, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            var rulesInstancesDict = rulesInstancesList.ToDictionary(p => p.Key, p => p);

            var initKeysOfkeysOfAdditionalRuleInstances = keysOfAdditionalRuleInstances.ToList();

            var targetKeysOfkeysOfAdditionalRuleInstances = initKeysOfkeysOfAdditionalRuleInstances;
            List<ulong> newKeysOfAdditionalRuleInstances = null;

            while (targetKeysOfkeysOfAdditionalRuleInstances.Count > 0)
            {
                newKeysOfAdditionalRuleInstances = new List<ulong>();

                foreach(var key in targetKeysOfkeysOfAdditionalRuleInstances)
                {
                    var ruleInstance = rulesInstancesDict[key];
                    FindAllKeysOfAdditionalRuleInstance(ruleInstance, ref newKeysOfAdditionalRuleInstances);
                }

                if(newKeysOfAdditionalRuleInstances.Count == 0)
                {
                    break;
                }

                targetKeysOfkeysOfAdditionalRuleInstances = newKeysOfAdditionalRuleInstances.Distinct().Where(p => !initKeysOfkeysOfAdditionalRuleInstances.Contains(p)).ToList();
            }
        }

        private static void FindAllKeysOfAdditionalRuleInstance(RuleInstance ruleInstance, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            if(ruleInstance.Part_1 != null)
            {
                FindAllKeysOfRulePartOfAdditionalRuleInstance(ruleInstance.Part_1, ref keysOfAdditionalRuleInstances);
            }

            if(ruleInstance.Part_2 != null)
            {
                FindAllKeysOfRulePartOfAdditionalRuleInstance(ruleInstance.Part_2, ref keysOfAdditionalRuleInstances);
            }

            FillAllKeysOfAnnotation(ruleInstance.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void FindAllKeysOfRulePartOfAdditionalRuleInstance(RulePart rulePart, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            FindAllKeysOfExpressionOfAdditionalRuleInstance(rulePart.Expression, ref keysOfAdditionalRuleInstances);
            FillAllKeysOfAnnotation(rulePart.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void FindAllKeysOfExpressionOfAdditionalRuleInstance(BaseExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            var kind = expressionNode.Kind;

            switch (kind)
            {
                case KindOfExpressionNode.And:
                    FillAllKeysInOperatorAndExpression(expressionNode.AsOperatorAnd, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Or:
                    FillAllKeysInOperatorOrExpression(expressionNode.AsOperatorOr, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Not:
                    FillAllKeysInOperatorNotExpression(expressionNode.AsOperatorNot, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Relation:
                    FillAllKeysInRelationExpression(expressionNode.AsRelation, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Concept:
                    break;

                case KindOfExpressionNode.EntityRef:
                    break;

                case KindOfExpressionNode.EntityCondition:
                    FillAllKeysInEntityConditionExpression(expressionNode.AsEntityCondition, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.Var:
                    break;

                case KindOfExpressionNode.QuestionVar:
                    break;

                case KindOfExpressionNode.Value:
                    break;

                case KindOfExpressionNode.Fact:
                    FillAllKeysInFactExpression(expressionNode.AsFact, ref keysOfAdditionalRuleInstances);
                    break;

                case KindOfExpressionNode.ParamStub:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            FillAllKeysOfAnnotation(expressionNode.Annotations, ref keysOfAdditionalRuleInstances);
        }

        private static void FillAllKeysInOperatorNotExpression(OperatorNotExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            FindAllKeysOfExpressionOfAdditionalRuleInstance(expressionNode.Left, ref keysOfAdditionalRuleInstances);
        }

        private static void FillAllKeysInOperatorAndExpression(OperatorAndExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            FindAllKeysOfExpressionOfAdditionalRuleInstance(expressionNode.Left, ref keysOfAdditionalRuleInstances);
            FindAllKeysOfExpressionOfAdditionalRuleInstance(expressionNode.Right, ref keysOfAdditionalRuleInstances);
        }

        private static void FillAllKeysInOperatorOrExpression(OperatorOrExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            FindAllKeysOfExpressionOfAdditionalRuleInstance(expressionNode.Left, ref keysOfAdditionalRuleInstances);
            FindAllKeysOfExpressionOfAdditionalRuleInstance(expressionNode.Right, ref keysOfAdditionalRuleInstances);
        }

        private static void FillAllKeysInEntityConditionExpression(EntityConditionExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            keysOfAdditionalRuleInstances.Add(expressionNode.Key);
        }

        private static void FillAllKeysInFactExpression(FactExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            keysOfAdditionalRuleInstances.Add(expressionNode.Key);
        }

        private static void FillAllKeysInRelationExpression(RelationExpressionNode expressionNode, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            foreach (var paramInfo in expressionNode.Params)
            {
                FindAllKeysOfExpressionOfAdditionalRuleInstance(paramInfo, ref keysOfAdditionalRuleInstances);
            }
        }

        private static void FillAllKeysOfAnnotation(IList<LogicalAnnotation> annotations, ref List<ulong> keysOfAdditionalRuleInstances)
        {
            if(annotations.IsEmpty())
            {
                return;
            }

            foreach(var annotation in annotations)
            {
                keysOfAdditionalRuleInstances.Add(annotation.RuleInstanceKey);
            }
        }

        private static IList<RuleInstance> CopyAllOfTargetAdditionalRuleInstances(IList<RuleInstance> rulesInstancesList, List<ulong> keysOfAdditionalRuleInstances)
        {
            var result = new List<RuleInstance>();
            var rulesInstancesDict = rulesInstancesList.ToDictionary(p => p.Key, p => p);

            foreach(var key in keysOfAdditionalRuleInstances)
            {
                var ruleInstance = rulesInstancesDict[key];
                var newRuleInstance = ruleInstance.Clone();
                result.Add(newRuleInstance);
            }

            return result;
        }
    }
}
