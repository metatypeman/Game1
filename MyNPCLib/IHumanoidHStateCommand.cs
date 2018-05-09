using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib
{
    public enum HumanoidHState
    {
        Stop,
        Walk,
        Run,
        LookAt,
        AimAt,
        Rotate,
        Move
    }

    public interface IHumanoidHStateCommand: IHumanoidBodyCommand
    {
        HumanoidHState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
