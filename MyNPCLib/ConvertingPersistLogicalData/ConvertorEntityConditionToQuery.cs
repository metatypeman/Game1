using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
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

            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParam(RuleInstance ruleInstance, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParam(RulePart rulePart, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
        {
#if DEBUG
            LogInstance.Log($"rulePart = {rulePart}");
#endif

            throw new NotImplementedException();
        }

        private static void ReplaceVarToQuestionParamInExpression(BaseExpressionNode expressionNode, IEntityDictionary entityDictionary, ref List<ulong> keysOfAdditionalRuleInstances)
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
    }
}
