using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ResultOfQueryToRelation : IObjectToString, IObjectToBriefString
    {
        public IndexedRuleInstance IndexedRuleInstance { get; set; }
        public IndexedRulePart IndexedRulePart { get; set; }
        public IList<ResultOfVarOfQueryToRelation> ResultOfVarOfQueryToRelationList { get; set; } = new List<ResultOfVarOfQueryToRelation>();

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
            if (IndexedRuleInstance == null)
            {
                sb.AppendLine($"{spaces}{nameof(IndexedRuleInstance)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IndexedRuleInstance)}");
                sb.Append(IndexedRuleInstance.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IndexedRuleInstance)}");
            }
            if (IndexedRulePart == null)
            {
                sb.AppendLine($"{spaces}{nameof(IndexedRulePart)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IndexedRulePart)}");
                sb.Append(IndexedRulePart.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(IndexedRulePart)}");
            }
            if (ResultOfVarOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultOfVarOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultOfVarOfQueryToRelationList)}");
                foreach (var resultOfVarOfQueryToRelation in ResultOfVarOfQueryToRelationList)
                {
                    sb.Append(resultOfVarOfQueryToRelation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultOfVarOfQueryToRelationList)}");
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
            sb.AppendLine($"{spaces}IndexedRuleInstance.Key = {IndexedRuleInstance.Key}");
            if (ResultOfVarOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultOfVarOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultOfVarOfQueryToRelationList)}");
                foreach (var resultOfVarOfQueryToRelation in ResultOfVarOfQueryToRelationList)
                {
                    sb.Append(resultOfVarOfQueryToRelation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultOfVarOfQueryToRelationList)}");
            }
            return sb.ToString();
        }
    }
}
