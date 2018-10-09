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
        public Dictionary<string, object> DefaultValuesMap { get; set; } = new Dictionary<string, object>();
        public Dictionary<ulong, Type> IndexedParametersMap { get; set; } = new Dictionary<ulong, Type>();
        public Dictionary<ulong, object> IndexedDefaultValuesMap { get; set; } = new Dictionary<ulong, object>();

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

            if (DefaultValuesMap == null)
            {
                sb.AppendLine($"{spaces}{nameof(DefaultValuesMap)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DefaultValuesMap)}");
                foreach (var parameterItem in DefaultValuesMap)
                {
                    sb.AppendLine($"{nextSpaces} Key = {parameterItem.Key}; Value = {parameterItem.Value}");
                }
                sb.AppendLine($"{spaces}End {nameof(DefaultValuesMap)}");
            }

            if (IndexedParametersMap == null)
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

            if (IndexedDefaultValuesMap == null)
            {
                sb.AppendLine($"{spaces}{nameof(IndexedDefaultValuesMap)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(IndexedDefaultValuesMap)}");
                foreach (var parameterItem in IndexedDefaultValuesMap)
                {
                    sb.AppendLine($"{nextSpaces} Key = {parameterItem.Key}; Value = {parameterItem.Value}");
                }
                sb.AppendLine($"{spaces}End {nameof(IndexedDefaultValuesMap)}");
            }
            return sb.ToString();
        }
    }
}
