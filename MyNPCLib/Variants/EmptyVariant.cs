using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class EmptyVariant: BaseVariant
    {
        public override KindOfVariant Kind => KindOfVariant.Empty;

        public override bool IsEmpty => true;
        public override EmptyVariant AsEmpty => this;
    }
}
