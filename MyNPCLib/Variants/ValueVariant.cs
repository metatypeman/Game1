using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class ValueVariant : BaseVariant
    {
        public override KindOfVariant Kind => KindOfVariant.Value;

        public override bool IsValue => true;
        public override ValueVariant AsValue => this;

        public object Value => throw new NotImplementedException();
    }
}
