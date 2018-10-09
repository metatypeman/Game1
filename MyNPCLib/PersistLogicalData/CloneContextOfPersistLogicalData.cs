using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public class CloneContextOfPersistLogicalData
    {
        public Dictionary<RuleInstance, RuleInstance> RuleInstancesDict { get; set; } = new Dictionary<RuleInstance, RuleInstance>();
        public Dictionary<RulePart, RulePart> RulePartsDict { get; set; } = new Dictionary<RulePart, RulePart>();
    }
}
