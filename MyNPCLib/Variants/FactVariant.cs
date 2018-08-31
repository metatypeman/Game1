using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class FactVariant : BaseVariant
    {
        public override KindOfVariant Kind => KindOfVariant.Fact;

        public override bool IsFact => true;
        public override FactVariant AsFact => this;
    }
}
