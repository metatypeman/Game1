using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class VisibleObjectsCGStorage: BaseProxyStorage
    {
        public VisibleObjectsCGStorage(IEntityDictionary entityDictionary, StorageOfSpecialEntities storageOfSpecialEntities)
            : base(entityDictionary)
        {

        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.OtherProxy;
    }
}
