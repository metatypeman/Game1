using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum OldKindOfHumanoidThingsCommand
    {
        Undefined,
        Take,
        PutToBagpack,
        ThrowOutToSurface
    }

    public interface IOldHumanoidThingsCommand : IOldMoveHumanoidCommand
    {
        OldKindOfHumanoidThingsCommand State { get; }
        int InstanceId { get; set; }
    }
}
