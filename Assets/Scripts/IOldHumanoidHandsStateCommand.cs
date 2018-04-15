using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum OldHumanoidHandsState
    {
        FreeHands,
        HasRifle
    }

    public interface IOldHumanoidHandsStateCommand : IOldMoveHumanoidCommand
    {
        OldHumanoidHandsState State { get; }
    }
}
