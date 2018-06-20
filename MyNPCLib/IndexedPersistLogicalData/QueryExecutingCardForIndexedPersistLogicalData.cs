using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class QueryExecutingCardForIndexedPersistLogicalData : IObjectToString, IObjectToBriefString
    {
        public ulong TargetRelation { get; set; }
        public int CountParams { get; set; }
        public IList<QueryExecutingCardAboutVar> VarsInfoList { get; set; }
        public IList<QueryExecutingCardAboutKnownInfo> KnownInfoList { get; set; }
        public IList<ResultOfQueryToRelation> ResultsOfQueryToRelationList { get; set; } = new List<ResultOfQueryToRelation>();
        public IndexedRuleInstance SenderIndexedRuleInstance { get; set; }
        public IndexedRulePart SenderIndexedRulePart { get; set; }
        public BaseExpressionNode SenderExpressionNode { get; set; }

        public string GetSenderIndexedRuleInstanceHumanizeDbgString()
        {
            if(SenderIndexedRuleInstance == null)
            {
                return string.Empty;
            }

            var origin = SenderIndexedRuleInstance.Origin;

            if(origin == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(origin);
        }

        public string GetSenderIndexedRulePartHumanizeDbgString()
        {
            if (SenderIndexedRulePart == null)
            {
                return string.Empty;
            }

            var origin = SenderIndexedRulePart.Origin;

            if (origin == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(origin);
        }

        public string GetSenderExpressionNodeHumanizeDbgString()
        {
            if (SenderExpressionNode == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(SenderExpressionNode);
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
            sb.AppendLine($"{spaces}{nameof(TargetRelation)} = {TargetRelation}");
            sb.AppendLine($"{spaces}{nameof(CountParams)} = {CountParams}");
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
            if (ResultsOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultsOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultsOfQueryToRelationList)}");
                foreach (var resultOfQueryToRelation in ResultsOfQueryToRelationList)
                {
                    sb.Append(resultOfQueryToRelation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultsOfQueryToRelationList)}");
            }
            return sb.ToString();
        }

        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (ResultsOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultsOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultsOfQueryToRelationList)}");
                foreach (var resultOfQueryToRelation in ResultsOfQueryToRelationList)
                {
                    sb.Append(resultOfQueryToRelation.ToBriefString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultsOfQueryToRelationList)}");
            }
            return sb.ToString();
        }
    }
}
