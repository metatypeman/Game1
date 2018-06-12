using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.DebugHelperForPersistLogicalData
{
    public static class DebugHelperForRuleInstance
    {
        public static string ToString(RuleInstance source)
        {
            return ToString(source, 0u);
        }

        public static string ToString(RuleInstance source, uint n)
        {
            var spaces = StringHelper.Spaces(n);

            var sb = new StringBuilder();
            sb.Append($"{{:{source.Name}:}}");//tmp
            return sb.ToString();
        }
    }
}
