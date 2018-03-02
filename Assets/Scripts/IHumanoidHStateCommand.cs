using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
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

    public interface IHumanoidHStateCommand : IMoveHumanoidCommand
    {
        HumanoidHState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
