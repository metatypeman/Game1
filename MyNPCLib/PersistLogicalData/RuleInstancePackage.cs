using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public class RuleInstancePackage
    {
        public RuleInstancePackage()
        {
        }

        public RuleInstancePackage(RuleInstance ruleInstance)
        {
            MainRuleInstance = ruleInstance;
            AllRuleInstances = new List<RuleInstance>() { ruleInstance };
        }

        public RuleInstance MainRuleInstance { get; set; }
        public IList<RuleInstance> AllRuleInstances { get; set; }
    }
}
