using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCProcess
    {
        StateOfNPCProcess State { get; }
        INPCResourcesManager Body { get; }
        INPCResourcesManager LeftHand { get; }
        INPCResourcesManager RightHand { get; }
    }
}
