using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class OriginOfVarOfQueryToRelation : IObjectToString, IObjectToBriefString
    {
        public ulong KeyOfRuleInstance { get; set; }
        public IndexedRuleInstance IndexedRuleInstance { get; set; }
        public IndexedRulePart IndexedRulePart { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(KeyOfRuleInstance)} = {KeyOfRuleInstance}");
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
            sb.AppendLine($"{spaces}{KeyOfRuleInstance} = {KeyOfRuleInstance}");
            return sb.ToString();
        }
    }
}
