using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class FuzzyLogicalValueVariant : BaseVariant
    {
        public override bool IsFuzzyLogicalValue => true;
        public override FuzzyLogicalValueVariant AsFuzzyLogicalValue => this;
    }
}
