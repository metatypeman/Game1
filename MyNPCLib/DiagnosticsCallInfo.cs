using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class DiagnosticsCallInfo : IObjectToString
    {
        public string FullClassName { get; set; }
        public string MethodName { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(FullClassName)} = {FullClassName}");
            sb.AppendLine($"{spaces}{nameof(MethodName)} = {MethodName}");
            return sb.ToString();
        }
    }
}
