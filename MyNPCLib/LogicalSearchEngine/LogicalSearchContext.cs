using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchContext: IObjectToString
    {
        public IndexedRuleInstance QueryExpression { get; set; }
        public IEntityDictionary EntityDictionary { get; set; }
        public IList<SettingsOfStorageForSearchingInThisSession> DataSourcesSettingsOrderedByPriorityList { get; set; }
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
            if (DataSourcesSettingsOrderedByPriorityAndUseProductionsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(DataSourcesSettingsOrderedByPriorityAndUseProductionsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DataSourcesSettingsOrderedByPriorityAndUseProductionsList)}");
                foreach (var dataSourcesSettings in DataSourcesSettingsOrderedByPriorityAndUseProductionsList)
                {
                    sb.Append(dataSourcesSettings.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(DataSourcesSettingsOrderedByPriorityAndUseProductionsList)}");
            }
            return sb.ToString();
        }
    }
}
