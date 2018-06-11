using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchOptions : IObjectToString
    {
        public IndexedRuleInstance QueryExpression { get; set; }
        public IList<SettingsOfStorageForSearchingInThisSession> DataSourcesSettings { get; set; }

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
            if (DataSourcesSettings == null)
            {
                sb.AppendLine($"{spaces}{nameof(DataSourcesSettings)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DataSourcesSettings)}");
                foreach (var dataSourceSettings in DataSourcesSettings)
                {
                    sb.Append(dataSourceSettings.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(DataSourcesSettings)}");
            }
            return sb.ToString();
        }
    }
}
