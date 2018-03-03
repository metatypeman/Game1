using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum KindOfHumanoidThingsCommand
    {
        Undefined,
        Take,
        PutToBagpack,
        PutToSurface
    }

    public interface IHumanoidThingsCommand : IMoveHumanoidCommand
    {
        KindOfHumanoidThingsCommand State { get; }
        int InstanceId { get; set; }
    }
}
