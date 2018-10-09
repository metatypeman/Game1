using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class ProxyForNPCResourceProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCResourceProcess(IEntityLogger entityLogger, ulong id, INPCContext context)
            : base(entityLogger, id, context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Resource;
    }
}
