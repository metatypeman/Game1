﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface INPCRayScaner
    {
        List<InternalVisionObject> VisibleObjects { get; }
    }
}
