using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IStatesOfHumanoidBodyHost
    {
        HumanoidHState HState { get; }
        object TargetPosition { get; }
        HumanoidVState VState { get; }
        HumanoidHandsState HandsState { get; }
        HumanoidHandsActionState HandsActionState { get; }
        HumanoidHeadState HeadState { get; }
        object TargetHeadPosition { get; }
        KindOfHumanoidThingsCommand KindOfThingsCommand { get; }
        int InstanceOfThingId { get; }
    }
}
