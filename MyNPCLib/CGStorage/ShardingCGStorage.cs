using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ShardingCGStorage : BaseCGStorage
    {
        public override KindOfCGStorage Kind => KindOfCGStorage.Sharding;
    }
}
