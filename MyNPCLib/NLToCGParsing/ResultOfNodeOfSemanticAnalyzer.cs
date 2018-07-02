using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ResultOfNodeOfSemanticAnalyzer : IObjectToString
    {
        public RolesStorageOfSemanticAnalyzer RolesDict { get; set; } = new RolesStorageOfSemanticAnalyzer();

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
            if (RolesDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(RolesDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RolesDict)}");
                sb.Append(RolesDict.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RolesDict)}");
            }
            return sb.ToString();
        }
    }
}
