using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum InternalHumanoidHeadState
    {
        LookingForward,
        LookAt,
        Rotate
    }

    public interface IInternalHumanoidHeadStateCommand : IInternalMoveHumanoidCommand
    {
        InternalHumanoidHeadState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
