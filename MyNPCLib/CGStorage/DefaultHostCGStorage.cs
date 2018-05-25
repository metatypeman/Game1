using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class DefaultHostCGStorage : BaseCGStorage
    {
        public override KindOfCGStorage Kind => KindOfCGStorage.Host;
    }
}
