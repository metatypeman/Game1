﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class GlobalCGStorage: BaseCGStorage
    {
        public GlobalCGStorage(ContextOfCGStorage context)
            : base(context)
        {
        }

        public override KindOfCGStorage Kind => KindOfCGStorage.Global;
    }
}
