using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib
{
    public interface IStatesOfHumanoidBodyHost
    {
        HumanoidHState HState { get; }
        Vector3? TargetPosition { get; }
        HumanoidVState VState { get; }
        HumanoidHandsState HandsState { get; }
        HumanoidHandsActionState HandsActionState { get; }
        HumanoidHeadState HeadState { get; }
        Vector3? TargetHeadPosition { get; }
        KindOfHumanoidThingsCommand KindOfThingsCommand { get; }
        ulong? EntityIdOfThing { get; }
    }
}
