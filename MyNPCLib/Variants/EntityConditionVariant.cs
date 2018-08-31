using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class EntityConditionVariant : BaseVariant
    {
        public override KindOfVariant Kind => KindOfVariant.EntityCondition;

        public override bool IsEntity => true;
        public override EntityConditionVariant AsEntity => this;
    }
}
