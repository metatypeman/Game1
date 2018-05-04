using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObject: BaseAbstractLogicalObject
    {
        public override IList<ulong> CurrentEnitiesIdList()
        {
            return new List<ulong>();//tmp
        }
    }
}
