using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public class RuleInstancePackage
    {
        public RuleInstance MainRuleInstance { get; set; }
        public IList<RuleInstance> AllRuleInstances { get; set; }
    }
}
