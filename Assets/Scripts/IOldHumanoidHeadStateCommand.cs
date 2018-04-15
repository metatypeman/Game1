using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum OldHumanoidHeadState
    {
        LookingForward,
        LookAt,
        Rotate
    }

    public interface IOldHumanoidHeadStateCommand : IOldMoveHumanoidCommand
    {
        OldHumanoidHeadState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
