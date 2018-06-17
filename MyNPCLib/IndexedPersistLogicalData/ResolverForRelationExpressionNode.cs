using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class ResolverForRelationExpressionNode: ResolverForBaseExpressionNode
    {
        public RelationExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Relation;
        public ulong Key { get; set; }
        public int CountParams { get; set; }
        public IList<ResolverForBaseExpressionNode> Params { get; set; }
        public IList<QueryExecutingCardAboutVar> VarsInfoList { get; set; }
        public IList<QueryExecutingCardAboutKnownInfo> KnownInfoList { get; set; }

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage source, ContextOfQueryExecutingCardForIndexedPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"Begin Key = {Key}");
            LogInstance.Log($"Params.Count = {Params.Count}");
            foreach (var param in Params)
            {
                LogInstance.Log($"param = {param}");
            }
#endif

            var indexedRulePartsOfFactsList = source.GetIndexedRulePartOfFactsByKeyOfRelation(Key);

#if DEBUG
            LogInstance.Log($"indexedRulePartsOfFactsList?.Count = {indexedRulePartsOfFactsList?.Count}");
#endif

            if (indexedRulePartsOfFactsList != null)
            {
                foreach (var indexedRulePartsOfFacts in indexedRulePartsOfFactsList)
                {
#if DEBUG
                    LogInstance.Log($"this = {this}");
                    //LogInstance.Log($"indexedRulePartsOfFacts = {indexedRulePartsOfFacts}");
#endif
                    var queryExecutingCardForTargetFact = new QueryExecutingCardForIndexedPersistLogicalData();
                    queryExecutingCardForTargetFact.TargetRelation = Key;
                    queryExecutingCardForTargetFact.CountParams = CountParams;
                    queryExecutingCardForTargetFact.VarsInfoList = VarsInfoList;
                    queryExecutingCardForTargetFact.KnownInfoList = KnownInfoList;

                    indexedRulePartsOfFacts.FillExecutingCardForCallingFromRelation(queryExecutingCardForTargetFact, source, context);

#if DEBUG
                    LogInstance.Log($"queryExecutingCardForTargetFact = {queryExecutingCardForTargetFact}");
#endif

                    foreach (var resultOfQueryToRelation in queryExecutingCardForTargetFact.ResultsOfQueryToRelationList)
                    {
                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultOfQueryToRelation);
                    }
                }
            }

#if DEBUG
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(CountParams)} = {CountParams}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            if (VarsInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsInfoList)}");
                foreach (var varItem in VarsInfoList)
                {
                    sb.Append(varItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsInfoList)}");
            }
            if (KnownInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(KnownInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(KnownInfoList)}");
                foreach (var knownInfo in KnownInfoList)
                {
                    sb.Append(knownInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(KnownInfoList)}");
            }
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(CountParams)} = {CountParams}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            if (VarsInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsInfoList)}");
                foreach (var varItem in VarsInfoList)
                {
                    sb.Append(varItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsInfoList)}");
            }
            if (KnownInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(KnownInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(KnownInfoList)}");
                foreach (var knownInfo in KnownInfoList)
                {
                    sb.Append(knownInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(KnownInfoList)}");
            }
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
