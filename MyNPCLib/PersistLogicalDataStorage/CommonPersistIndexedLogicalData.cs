using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalDataStorage
{
    [Serializable]
    public class CommonPersistIndexedLogicalData
    {
        public IDictionary<ulong, IndexedRuleInstance> IndexedRuleInstancesDict { get; set; }
    }
}
