using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum HumanoidHeadState
    {
        LookingForward,
        LookAt,
        Rotate
    }

    public interface IHumanoidHeadStateCommand : IHumanoidBodyCommand
    {
        HumanoidHeadState State { get; }
        object TargetPosition { get; }
        float Speed { get; }
    }
}
