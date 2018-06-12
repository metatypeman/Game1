using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalDataStorage
{
    [Serializable]
    public class CommonPersistLogicalData
    {
        public IList<RuleInstance> RuleInstancesList { get; set; }
        public IDictionary<ulong, IndexedRuleInstance> IndexedRuleInstancesDict { get; set; }
    }
}
