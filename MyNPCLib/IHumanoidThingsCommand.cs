using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum KindOfHumanoidThingsCommand
    {
        Undefined,
        Take,
        PutToBagpack,
        ThrowOutToSurface
    }

    public interface IHumanoidThingsCommand : IHumanoidBodyCommand
    {
        KindOfHumanoidThingsCommand State { get; }
        int InstanceId { get; set; }
    }
}
