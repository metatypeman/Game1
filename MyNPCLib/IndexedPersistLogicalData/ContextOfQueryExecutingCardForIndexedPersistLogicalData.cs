using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ContextOfQueryExecutingCardForIndexedPersistLogicalData: IObjectToString
    {
        public IndexedRuleInstance QueryExpression { get; set; }
        public bool UseProductions { get; set; }
        public int? MaxDeph { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(UseProductions)} = {UseProductions}");
            sb.AppendLine($"{spaces}{nameof(MaxDeph)} = {MaxDeph}");
            return sb.ToString();
        }
    }
}
