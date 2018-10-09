using System;
using System.Collections.Generic;
using System.Numerics;
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
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
