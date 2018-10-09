using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class DefaultHostCGStorage : BaseRealStorage
    {
        public DefaultHostCGStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Host;
    }
}
