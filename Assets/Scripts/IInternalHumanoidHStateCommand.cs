using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum InternalHumanoidHState
    {
        Stop,
        Walk,
        Run,
        LookAt,
        AimAt,
        Rotate,
        Move
    }

    public interface IInternalHumanoidHStateCommand : IInternalMoveHumanoidCommand
    {
        InternalHumanoidHState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
