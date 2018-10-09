using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NPCProcessStartupModeAttribute: Attribute
    {
        public NPCProcessStartupModeAttribute(NPCProcessStartupMode startupMode)
        {
            StartupMode = startupMode;
        }

        public NPCProcessStartupMode StartupMode { get; private set; }
    }
}
