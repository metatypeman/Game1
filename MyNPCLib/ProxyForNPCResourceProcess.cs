using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class ProxyForNPCResourceProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCResourceProcess(ulong id)
            : base(id)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Resource;
    }
}
