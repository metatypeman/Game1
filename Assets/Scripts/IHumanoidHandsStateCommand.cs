using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum HumanoidHandsState
    {
        FreeHands,
        HasRifle
    }

    public interface IHumanoidHandsStateCommand : IMoveHumanoidCommand
    {
        HumanoidHandsState State { get; }
    }
}
