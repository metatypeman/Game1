using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class ProxyForNPCAbstractProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCAbstractProcess(ulong id)
            : base(id)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
    }
}
