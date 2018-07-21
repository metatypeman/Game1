using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedRuleInstance: IIndexedLogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public KindOfRuleInstance Kind { get; set; }
        public ulong Key { get; set; }
        public RuleInstance Origin { get; set; }
        public ulong ModuleKey { get; set; }
        public IndexedBelongToEntity BelongToEntity { get; set; }
        public IndexedVariablesQuantificationPart VariablesQuantification { get; set; }
        public IndexedEntitiesConditions EntitiesConditions { get; set; }
        public bool IsPart_1_Active { get; set; }
        public bool IsPart_2_Active { get; set; }
        public IndexedRulePart Part_1 { get; set; }
        public IndexedRulePart Part_2 { get; set; }
        public IndexedIfConditionsPart IfConditions { get; set; }
        public IndexedNotContradictPart NotContradict { get; set; }
        public IndexedAccessPolicyToFactModality AccessPolicyToFactModality { get; set; }
        public IndexedDesirableFuzzyModality DesirableModality { get; set; }
        public IndexedNecessityFuzzyModality NecessityModality { get; set; }
        public IndexedImperativeFuzzyModality ImperativeModality { get; set; }
        public IndexedIntentionallyFuzzyModality IntentionallyModality { get; set; }
        public IndexedPriorityFuzzyModality PriorityModality { get; set; }
        public IndexedRealityFuzzyModality RealityModality { get; set; }
        public IndexedProbabilityFuzzyModality ProbabilityModality { get; set; }
        public IndexedCertaintyFactorFuzzyModality CertaintyFactor { get; set; }
        public IndexedMoralQualityFuzzyModality MoralQualityModality { get; set; }
        public IndexedQuantityQualityFuzzyModality QuantityQualityModality { get; set; }
        public IList<IndexedLogicalAnnotation> Annotations { get; set; }

        public void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
            var resultsOfQueryToRelationList = new List<ResultOfQueryToRelation>();

            if (IsPart_1_Active)
            {
                var queryExecutingCardForPart_1 = new QueryExecutingCardForIndexedPersistLogicalData();
                queryExecutingCardForPart_1.SenderIndexedRuleInstance = this;

                Part_1.FillExecutingCard(queryExecutingCardForPart_1, context);

                foreach (var resultOfQueryToRelation in queryExecutingCardForPart_1.ResultsOfQueryToRelationList)
                {
                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }

            if (IsPart_2_Active)
            {
                var queryExecutingCardForPart_2 = new QueryExecutingCardForIndexedPersistLogicalData();
                queryExecutingCardForPart_2.SenderIndexedRuleInstance = this;
                Part_2.FillExecutingCard(queryExecutingCardForPart_2, context);

                foreach (var resultOfQueryToRelation in queryExecutingCardForPart_2.ResultsOfQueryToRelationList)
                {
                    queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                }
            }
        }

        public string GetHumanizeDbgString()
        {
            if (Origin == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(Origin);
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();       
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }

            sb.AppendLine($"{spaces}{nameof(ModuleKey)} = {ModuleKey}");
            if (BelongToEntity == null)
            {
                sb.AppendLine($"{spaces}{nameof(BelongToEntity)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(BelongToEntity)}");
                sb.Append(BelongToEntity.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(BelongToEntity)}");
            }
            if (VariablesQuantification == null)
            {
                sb.AppendLine($"{spaces}{nameof(VariablesQuantification)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VariablesQuantification)}");
                sb.Append(VariablesQuantification.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VariablesQuantification)}");
            }

            if (EntitiesConditions == null)
            {
                sb.AppendLine($"{spaces}{nameof(EntitiesConditions)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(EntitiesConditions)}");
                sb.Append(EntitiesConditions.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(EntitiesConditions)}");
            }

            sb.AppendLine($"{spaces}{nameof(IsPart_1_Active)} = {IsPart_1_Active}");
            sb.AppendLine($"{spaces}{nameof(IsPart_2_Active)} = {IsPart_2_Active}");
            if (Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }

            if (Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
            }

            if (IfConditions == null)
            {
                sb.AppendLine($"{spaces}{nameof(IfConditions)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IfConditions)}");
                sb.Append(IfConditions.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IfConditions)}");
            }

            if (NotContradict == null)
            {
                sb.AppendLine($"{spaces}{nameof(NotContradict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NotContradict)}");
                sb.Append(NotContradict.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NotContradict)}");
            }

            if (AccessPolicyToFactModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(AccessPolicyToFactModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AccessPolicyToFactModality)}");
                sb.Append(AccessPolicyToFactModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AccessPolicyToFactModality)}");
            }

            if (DesirableModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(DesirableModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DesirableModality)}");
                sb.Append(DesirableModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(DesirableModality)}");
            }

            if (NecessityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(NecessityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NecessityModality)}");
                sb.Append(NecessityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NecessityModality)}");
            }

            if (ImperativeModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(ImperativeModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ImperativeModality)}");
                sb.Append(ImperativeModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ImperativeModality)}");
            }

            if (IntentionallyModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(IntentionallyModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IntentionallyModality)}");
                sb.Append(IntentionallyModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IntentionallyModality)}");
            }
            if (PriorityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(PriorityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PriorityModality)}");
                sb.Append(PriorityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(PriorityModality)}");
            }

            if (RealityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(RealityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RealityModality)}");
                sb.Append(RealityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RealityModality)}");
            }

            if (ProbabilityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(ProbabilityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ProbabilityModality)}");
                sb.Append(ProbabilityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ProbabilityModality)}");
            }
            if (CertaintyFactor == null)
            {
                sb.AppendLine($"{spaces}{nameof(CertaintyFactor)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CertaintyFactor)}");
                sb.Append(CertaintyFactor.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(CertaintyFactor)}");
            }

            if (MoralQualityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(MoralQualityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(MoralQualityModality)}");
                sb.Append(MoralQualityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(MoralQualityModality)}");
            }

            if (QuantityQualityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(QuantityQualityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QuantityQualityModality)}");
                sb.Append(QuantityQualityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(QuantityQualityModality)}");
            }

            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }

            sb.AppendLine($"{spaces}{nameof(ModuleKey)} = {ModuleKey}");
            if (BelongToEntity == null)
            {
                sb.AppendLine($"{spaces}{nameof(BelongToEntity)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(BelongToEntity)}");
                sb.Append(BelongToEntity.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(BelongToEntity)}");
            }
            if (VariablesQuantification == null)
            {
                sb.AppendLine($"{spaces}{nameof(VariablesQuantification)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VariablesQuantification)}");
                sb.Append(VariablesQuantification.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VariablesQuantification)}");
            }
            if (EntitiesConditions == null)
            {
                sb.AppendLine($"{spaces}{nameof(EntitiesConditions)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(EntitiesConditions)}");
                sb.Append(EntitiesConditions.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(EntitiesConditions)}");
            }

            sb.AppendLine($"{spaces}{nameof(IsPart_1_Active)} = {IsPart_1_Active}");
            sb.AppendLine($"{spaces}{nameof(IsPart_2_Active)} = {IsPart_2_Active}");
            if (Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }

            if (Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
            }

            if (IfConditions == null)
            {
                sb.AppendLine($"{spaces}{nameof(IfConditions)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IfConditions)}");
                sb.Append(IfConditions.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IfConditions)}");
            }

            if (NotContradict == null)
            {
                sb.AppendLine($"{spaces}{nameof(NotContradict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NotContradict)}");
                sb.Append(NotContradict.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NotContradict)}");
            }

            if (AccessPolicyToFactModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(AccessPolicyToFactModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AccessPolicyToFactModality)}");
                sb.Append(AccessPolicyToFactModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AccessPolicyToFactModality)}");
            }

            if (DesirableModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(DesirableModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DesirableModality)}");
                sb.Append(DesirableModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(DesirableModality)}");
            }

            if (NecessityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(NecessityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NecessityModality)}");
                sb.Append(NecessityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NecessityModality)}");
            }

            if (ImperativeModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(ImperativeModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ImperativeModality)}");
                sb.Append(ImperativeModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ImperativeModality)}");
            }

            if (IntentionallyModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(IntentionallyModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IntentionallyModality)}");
                sb.Append(IntentionallyModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IntentionallyModality)}");
            }

            if (PriorityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(PriorityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PriorityModality)}");
                sb.Append(PriorityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(PriorityModality)}");
            }

            if (RealityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(RealityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RealityModality)}");
                sb.Append(RealityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RealityModality)}");
            }

            if (ProbabilityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(ProbabilityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ProbabilityModality)}");
                sb.Append(ProbabilityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(ProbabilityModality)}");
            }

            if (CertaintyFactor == null)
            {
                sb.AppendLine($"{spaces}{nameof(CertaintyFactor)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CertaintyFactor)}");
                sb.Append(CertaintyFactor.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(CertaintyFactor)}");
            }

            if (MoralQualityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(MoralQualityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(MoralQualityModality)}");
                sb.Append(MoralQualityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(MoralQualityModality)}");
            }

            if (QuantityQualityModality == null)
            {
                sb.AppendLine($"{spaces}{nameof(QuantityQualityModality)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QuantityQualityModality)}");
                sb.Append(QuantityQualityModality.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(QuantityQualityModality)}");
            }

            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }
    }
}
