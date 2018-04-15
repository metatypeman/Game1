using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum InternalKindOfHumanoidThingsCommand
    {
        Undefined,
        Take,
        PutToBagpack,
        ThrowOutToSurface
    }

    public interface IInternalHumanoidThingsCommand : IInternalMoveHumanoidCommand
    {
        InternalKindOfHumanoidThingsCommand State { get; }
        int InstanceId { get; set; }
    }
}
