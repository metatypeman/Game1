using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class LocalCGStorage : BaseCGStorage
    {
        public override KindOfCGStorage Kind => KindOfCGStorage.Local;
    }
}
