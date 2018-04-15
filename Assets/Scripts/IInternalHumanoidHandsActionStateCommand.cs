using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum InternalHumanoidHandsActionState
    {
        Empty,
        StrongAim
    }

    public interface IInternalHumanoidHandsActionStateCommand : IInternalMoveHumanoidCommand
    {
        InternalHumanoidHandsActionState State { get; }
    }
}
