using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ContextOfSemanticAnalyzer
    {
        public ConceptualGraph OuterConceptualGraph { get; set; }
        public ConceptualGraph ConceptualGraph { get; set; }
        public RelationStorageOfSemanticAnalyzer RelationStorage { get; private set; } = new RelationStorageOfSemanticAnalyzer();
    }
}
