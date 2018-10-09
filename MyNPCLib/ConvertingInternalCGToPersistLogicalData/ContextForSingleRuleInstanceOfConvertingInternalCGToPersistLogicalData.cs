using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.ConvertingInternalCGToPersistLogicalData
{
    public class ContextForSingleRuleInstanceOfConvertingInternalCGToPersistLogicalData
    {
        public RuleInstance CurrentRuleInstance { get; set; }
        public Dictionary<string, string> EntityConditionsDict { get; set; } = new Dictionary<string, string>();
    }
}
