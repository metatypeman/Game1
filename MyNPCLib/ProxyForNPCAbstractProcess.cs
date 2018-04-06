using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class ProxyForNPCAbstractProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCAbstractProcess(ulong id, INPCContext context)
            : base(id, context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
    }
}
