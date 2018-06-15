using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.ConvertingPersistLogicalDataToIndexing
{
    public class ContextOfConvertingToIndexed
    {
        public IDictionary<RuleInstance, IndexedRuleInstance> RuleInstancesDict = new Dictionary<RuleInstance, IndexedRuleInstance>();
        public IDictionary<BaseExpressionNode, IndexedBelongToEntity> BelongToEntityDict = new Dictionary<BaseExpressionNode, IndexedBelongToEntity>();
        public IDictionary<EntitiesConditions, IndexedEntitiesConditions> EntitiesConditionsDict = new Dictionary<EntitiesConditions, IndexedEntitiesConditions>();
        public IDictionary<VariablesQuantificationPart, IndexedVariablesQuantificationPart> VariablesQuantificationPartDict = new Dictionary<VariablesQuantificationPart, IndexedVariablesQuantificationPart>();
        public IDictionary<RulePart, IndexedRulePart> RulePartDict = new Dictionary<RulePart, IndexedRulePart>();
        public IDictionary<IfConditionsPart, IndexedIfConditionsPart> IfConditionsPartDict = new Dictionary<IfConditionsPart, IndexedIfConditionsPart>();
        public IDictionary<NotContradictPart, IndexedNotContradictPart> NotContradictPartDict = new Dictionary<NotContradictPart, IndexedNotContradictPart>();
        public IDictionary<AccessPolicyToFactModality, IndexedAccessPolicyToFactModality> AccessPolicyToFactModalityDict = new Dictionary<AccessPolicyToFactModality, IndexedAccessPolicyToFactModality>();
        public IDictionary<DesirableFuzzyModality, IndexedDesirableFuzzyModality> DesirableFuzzyModalityDict = new Dictionary<DesirableFuzzyModality, IndexedDesirableFuzzyModality>();
        public IDictionary<NecessityFuzzyModality, IndexedNecessityFuzzyModality> NecessityFuzzyModalityDict = new Dictionary<NecessityFuzzyModality, IndexedNecessityFuzzyModality>();
        public IDictionary<ImperativeFuzzyModality, IndexedImperativeFuzzyModality> ImperativeFuzzyModalityDict = new Dictionary<ImperativeFuzzyModality, IndexedImperativeFuzzyModality>();
        public IDictionary<IntentionallyFuzzyModality, IndexedIntentionallyFuzzyModality> IntentionallyFuzzyModalityDict = new Dictionary<IntentionallyFuzzyModality, IndexedIntentionallyFuzzyModality>();
        public IDictionary<PriorityFuzzyModality, IndexedPriorityFuzzyModality> PriorityFuzzyModalityDict = new Dictionary<PriorityFuzzyModality, IndexedPriorityFuzzyModality>();
        public IDictionary<RealityFuzzyModality, IndexedRealityFuzzyModality> RealityFuzzyModalityDict = new Dictionary<RealityFuzzyModality, IndexedRealityFuzzyModality>();
        public IDictionary<ProbabilityFuzzyModality, IndexedProbabilityFuzzyModality> ProbabilityFuzzyModalityDict = new Dictionary<ProbabilityFuzzyModality, IndexedProbabilityFuzzyModality>();
        public IDictionary<CertaintyFactorFuzzyModality, IndexedCertaintyFactorFuzzyModality> CertaintyFactorFuzzyModalityDict = new Dictionary<CertaintyFactorFuzzyModality, IndexedCertaintyFactorFuzzyModality>();
        public IDictionary<MoralQualityFuzzyModality, IndexedMoralQualityFuzzyModality> MoralQualityFuzzyModalityDict = new Dictionary<MoralQualityFuzzyModality, IndexedMoralQualityFuzzyModality>();
        public IDictionary<QuantityQualityFuzzyModality, IndexedQuantityQualityFuzzyModality> QuantityQualityFuzzyModalityDict = new Dictionary<QuantityQualityFuzzyModality, IndexedQuantityQualityFuzzyModality>();
    }
}
