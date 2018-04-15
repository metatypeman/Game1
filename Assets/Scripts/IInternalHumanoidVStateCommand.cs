using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum InternalHumanoidVState
    {
        Ground,
        Jump,
        Crouch
    }

    public interface IInternalHumanoidVStateCommand : IInternalMoveHumanoidCommand
    {
        InternalHumanoidVState State { get; }
    }
}
