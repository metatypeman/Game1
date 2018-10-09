using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class OptionsOfFillExecutingCard : IObjectToString
    {
        public bool EntityIdOnly { get; set; }
        public bool UseAccessPolicy { get; set; }
        public List<IndexedAccessPolicyToFactModality> AccessPolicyToFactModalityList { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(UseAccessPolicy)} = {UseAccessPolicy}");
            if (AccessPolicyToFactModalityList == null)
            {
                sb.AppendLine($"{spaces}{nameof(AccessPolicyToFactModalityList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AccessPolicyToFactModalityList)}");
                foreach (var accessPolicy in AccessPolicyToFactModalityList)
                {
                    sb.Append(accessPolicy.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(AccessPolicyToFactModalityList)}");
            }
            return sb.ToString();
        }
    }
}
