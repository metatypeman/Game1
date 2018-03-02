using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum HumanoidHandsActionState
    {
        Empty,
        StrongAim
    }

    public interface IHumanoidHandsActionStateCommand : IMoveHumanoidCommand
    {
        HumanoidHandsActionState State { get; }
    }
}
