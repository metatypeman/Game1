using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class LocalCGStorage : BaseRealStorage
    {
        public LocalCGStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Local;
    }
}
