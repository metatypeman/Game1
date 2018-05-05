using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCHostContext
    {
        INPCBodyHost BodyHost { get; }
        INPCHandHost RightHandHost { get; }
        INPCHandHost LeftHandHost { get; }
        ILogicalStorage HostLogicalStorage { get; }
        IPassiveLogicalObject SelfLogicalObject { get; }
    }
}
