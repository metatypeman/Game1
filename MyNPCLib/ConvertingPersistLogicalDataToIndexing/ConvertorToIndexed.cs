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

            result.IsPart_1_Active = source.IsPart_1_Active;
            result.IsPart_2_Active = source.IsPart_2_Active;

            if(source.Part_1 != null)
            {
                result.Part_1 = ConvertRulePart(source.Part_1, context);
            }

            if(source.Part_2 != null)
            {
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

            if(source.DesirableModality != null)
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
