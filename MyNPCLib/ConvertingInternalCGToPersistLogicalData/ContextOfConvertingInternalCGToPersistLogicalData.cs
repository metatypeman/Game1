using MyNPCLib.InternalCG;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.ConvertingInternalCGToPersistLogicalData
{
    public class ContextOfConvertingInternalCGToPersistLogicalData
    {
        public IEntityDictionary EntityDictionary { get; set; }
        public Dictionary<InternalConceptualGraph, RuleInstance> RuleInstancesDict { get; set; } = new Dictionary<InternalConceptualGraph, RuleInstance>();
        public List<RuleInstance> AnnotationsList { get; set; } = new List<RuleInstance>();
    }
}
