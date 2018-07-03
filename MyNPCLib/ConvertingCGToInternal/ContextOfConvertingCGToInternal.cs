using MyNPCLib.CG;
using MyNPCLib.InternalCG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.ConvertingCGToInternal
{
    public class ContextOfConvertingCGToInternal
    {
        public Dictionary<ConceptualGraph, InternalConceptualGraph> ConceptualGraphsDict { get; set; } = new Dictionary<ConceptualGraph, InternalConceptualGraph>();
    }
}
