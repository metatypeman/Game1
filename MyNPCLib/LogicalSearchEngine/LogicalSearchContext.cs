using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchContext: IObjectToString
    {
        public IndexedRuleInstance QueryExpression { get; set; }
        public IDictionary<int, IList<SettingsOfStorageForSearchingInThisSession>> DataSourcesSettings { get; set; }
        public IList<SettingsOfStorageForSearchingInThisSession> DataSourcesSettingsOrderedByPriorityAndUseProductionsList { get; set; }

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

            if (DataSourcesSettings == null)
            {
                sb.AppendLine($"{spaces}{nameof(DataSourcesSettings)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DataSourcesSettings)}");
                var doubleNext = nextN + 4;
                foreach (var dataSourceSettingsGroup in DataSourcesSettings)
                {
                    sb.AppendLine($"{nextNSpaces}PriorityOfGroup = {dataSourceSettingsGroup.Key}");
                    sb.AppendLine($"{nextNSpaces}Count Items in Group = {dataSourceSettingsGroup.Value.Count}");
                    foreach (var dataSourceSettings in dataSourceSettingsGroup.Value)
                    {
                        sb.Append(dataSourceSettings.ToString(doubleNext));
                    }
                }
                sb.AppendLine($"{spaces}End {nameof(DataSourcesSettings)}");
            }
            return sb.ToString();
        }
    }
}
