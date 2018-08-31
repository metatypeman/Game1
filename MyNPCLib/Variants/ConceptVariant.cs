using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class ConceptVariant : BaseVariant
    {
        public override KindOfVariant Kind => KindOfVariant.Concept;

        public override bool IsConcept => true;
        public override ConceptVariant AsConcept => this;
    }
}
