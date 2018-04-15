using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum OldHumanoidHState
    {
        Stop,
        Walk,
        Run,
        LookAt,
        AimAt,
        Rotate,
        Move
    }

    public interface IOldHumanoidHStateCommand : IOldMoveHumanoidCommand
    {
        OldHumanoidHState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
