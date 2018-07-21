using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ShardingCGStorage : BaseCGStorage
    {
        public ShardingCGStorage(ContextOfCGStorage context)
            : base(context)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Sharding;
    }
}
