using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum HumanoidHandsActionState
    {
        Empty,
        StrongAim
    }

    public interface IHumanoidHandsActionStateCommand : IHumanoidBodyCommand
    {
        HumanoidHandsActionState State { get; }
    }
}
