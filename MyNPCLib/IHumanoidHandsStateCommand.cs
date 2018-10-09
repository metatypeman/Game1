using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum HumanoidHandsState
    {
        FreeHands,
        HasRifle
    }

    public interface IHumanoidHandsStateCommand : IHumanoidBodyCommand
    {
        HumanoidHandsState State { get; }
    }
}
