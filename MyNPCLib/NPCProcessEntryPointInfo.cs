using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessEntryPointInfo : IObjectToString
    {
        public MethodInfo MethodInfo { get; set; }
        public Dictionary<string, Type> ParametersMap { get; set; } = new Dictionary<string, Type>();
        public Dictionary<ulong, Type> IndexedParametersMap { get; set; } = new Dictionary<ulong, Type>();

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCProcessEntryPointInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCProcessEntryPointInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            if(ParametersMap == null)
            {
                sb.AppendLine($"{spaces}{nameof(ParametersMap)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ParametersMap)}");
                foreach(var parameterItem in ParametersMap)
                {
                    sb.AppendLine($"{nextSpaces} Key = {parameterItem.Key}; Value = {parameterItem.Value}");
                }
                sb.AppendLine($"{spaces}End {nameof(ParametersMap)}");
            }
            if(IndexedParametersMap == null)
            {
                sb.AppendLine($"{spaces}{nameof(IndexedParametersMap)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IndexedParametersMap)}");
                foreach (var parameterItem in IndexedParametersMap)
                {
                    sb.AppendLine($"{nextSpaces} Key = {parameterItem.Key}; Value = {parameterItem.Value}");
                }
                sb.AppendLine($"{spaces}End {nameof(IndexedParametersMap)}");
            }
            return sb.ToString();
        }
    }
}
