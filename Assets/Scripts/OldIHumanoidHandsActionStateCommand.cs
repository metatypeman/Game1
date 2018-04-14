using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum OldHumanoidHandsActionState
    {
        Empty,
        StrongAim
    }

    public interface OldIHumanoidHandsActionStateCommand : IMoveHumanoidCommand
    {
        OldHumanoidHandsActionState State { get; }
    }
}
