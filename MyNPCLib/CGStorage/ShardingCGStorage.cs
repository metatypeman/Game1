using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ShardingCGStorage : BaseRealStorage
    {
        public ShardingCGStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Sharding;
    }
}
