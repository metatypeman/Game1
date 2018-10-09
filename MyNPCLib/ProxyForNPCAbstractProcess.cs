using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class ProxyForNPCAbstractProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCAbstractProcess(IEntityLogger entityLogger, ulong id, INPCContext context)
            : base(entityLogger, id, context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
    }
}
