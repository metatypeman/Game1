using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum HumanoidHeadState
    {
        LookingForward,
        LookAt,
        Rotate
    }

    public interface IHumanoidHeadStateCommand : IMoveHumanoidCommand
    {
        HumanoidHeadState State { get; }
        Vector3? TargetPosition { get; }
        float Speed { get; }
    }
}
