using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObject: BaseAbstractLogicalObject
    {
        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => throw new NotImplementedException();
        public override ulong CurrentEntityId => 0ul;//tmp
    }
}
