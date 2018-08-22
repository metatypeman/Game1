using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseProxyStorage: BaseCGStorage
    {
        protected BaseProxyStorage(ContextOfCGStorage context)
            : base(context)
        {
        }
    }
}
