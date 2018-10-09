using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class GlobalCGStorage: BaseRealStorage
    {
        public GlobalCGStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Global;
    }
}
