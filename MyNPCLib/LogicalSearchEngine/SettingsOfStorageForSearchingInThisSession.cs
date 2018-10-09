using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class SettingsOfStorageForSearchingInThisSession: IObjectToString
    {
        public ICGStorage Storage { get; set; }
        public bool UseFacts { get; set; }
        public bool UseAdditionalInstances { get; set; }
        public bool UseProductions { get; set; }        
        public int? MaxDeph { get; set; }
        public int Priority { get; set; }

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
            if (Storage == null)
            {
                sb.AppendLine($"{spaces}{nameof(Storage)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Storage)}");
                sb.Append(Storage.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Storage)}");
            }
            sb.AppendLine($"{spaces}{nameof(UseFacts)} = {UseFacts}");
            sb.AppendLine($"{spaces}{nameof(UseAdditionalInstances)} = {UseAdditionalInstances}");
            sb.AppendLine($"{spaces}{nameof(UseProductions)} = {UseProductions}");
            sb.AppendLine($"{spaces}{nameof(MaxDeph)} = {MaxDeph}");
            sb.AppendLine($"{spaces}{nameof(Priority)} = {Priority}");
            return sb.ToString();
        }
    }
}
