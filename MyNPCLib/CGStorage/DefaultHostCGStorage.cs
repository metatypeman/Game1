﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class DefaultHostCGStorage : BaseRealStorage
    {
        public DefaultHostCGStorage(ContextOfCGStorage context)
            : base(context)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Host;
    }
}
