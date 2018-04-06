﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class ProxyForNPCResourceProcess : BaseProxyForNPCProcess
    {
        public ProxyForNPCResourceProcess(ulong id, INPCContext context)
            : base(id, context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Resource;
    }
}
