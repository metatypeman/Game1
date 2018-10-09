using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseProxyStorage: BaseCGStorage
    {
        protected BaseProxyStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
        }
    }
}
