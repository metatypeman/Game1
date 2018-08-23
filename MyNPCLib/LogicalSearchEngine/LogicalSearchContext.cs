using MyNPCLib.CGStorage;
using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    [Obsolete]
    public class LogicalSearchContext: IObjectToString
    {
        [Obsolete]
        public IndexedRuleInstance QueryExpression { get; set; }
        [Obsolete]
        public IEntityDictionary EntityDictionary { get; set; }
        [Obsolete]
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
            var nextNSpaces = StringHelper.Spaces(nextN);
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
            return sb.ToString();
        }
    }
}
