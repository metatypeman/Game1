using MyNPCLib.CGStorage;
using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchOptions : IObjectToString
    {
        public bool EntityIdOnly { get; set; }
        public bool IgnoreAccessPolicy { get; set; } = true;
        public ICGStorage QuerySource { get; set; }
        public IndexedRuleInstance QueryExpression { get; set; }
        public ICGStorage DataSource { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(EntityIdOnly)} = {EntityIdOnly}");
            sb.AppendLine($"{spaces}{nameof(IgnoreAccessPolicy)} = {IgnoreAccessPolicy}");
            if (QuerySource == null)
            {
                sb.AppendLine($"{spaces}{nameof(QuerySource)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QuerySource)}");
                sb.Append(QuerySource.MainIndexedRuleInstance?.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(QuerySource)}");
            }
            if (QueryExpression == null)
            {
                sb.AppendLine($"{spaces}{nameof(QueryExpression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QueryExpression)}");
                sb.Append(QueryExpression.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(QueryExpression)}");
            }
            return sb.ToString();
        }
    }
}
