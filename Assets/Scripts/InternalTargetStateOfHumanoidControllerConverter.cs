using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNPCLib;
using UnityEngine;

namespace Assets.Scripts
{
    public static class InternalTargetStateOfHumanoidControllerConverter
    {
        public static InternalTargetStateOfHumanoidController ConvertToInternal(TargetStateOfHumanoidBody state)
        {
            var result = new InternalTargetStateOfHumanoidController();
            
            if(state.HState.HasValue)
            {
                result.HState = InternalStatesConverter.HumanoidHStateToInternal(state.HState.Value);
            }
            else
            {
                result.HState = null;
            }

            if (state.TargetPosition == null)
            {
                result.TargetPosition = null;
            }
            else
            {
                result.TargetPosition = (Vector3)state.TargetPosition;
            }

            if (state.VState.HasValue)
            {
                result.VState = InternalStatesConverter.HumanoidVStateToInternal(state.VState.Value);
            }
            else
            {
                result.VState = null;
            }

            if (state.HandsState.HasValue)
            {
                result.HandsState = InternalStatesConverter.HumanoidHandsStateToInternal(state.HandsState.Value);
            }
            else
            {
                result.HandsState = null;
            }

            if (state.HandsActionState.HasValue)
            {
                result.HandsActionState = InternalStatesConverter.HumanoidHandsActionStateToInternal(state.HandsActionState.Value);
            }
            else
            {
                result.HandsActionState = null;
            }

            if (state.HeadState.HasValue)
            {
                result.HeadState = InternalStatesConverter.HumanoidHeadStateToInternal(state.HeadState.Value);
            }
            else
            {
                result.HeadState = null;
            }

            if (state.TargetHeadPosition == null)
            {
                result.TargetHeadPosition = null;
            }
            else
            {
                result.TargetHeadPosition = (Vector3)result.TargetHeadPosition;
            }

            if (state.KindOfThingsCommand.HasValue)
            {
                result.KindOfThingsCommand = InternalStatesConverter.KindOfHumanoidThingsCommandToInternal(state.KindOfThingsCommand.Value);
            }
            else
            {
                result.KindOfThingsCommand = null;
            }

            if (state.InstanceOfThingId.HasValue)
            {
                result.InstanceOfThingId = state.InstanceOfThingId.Value;
            }
            else
            {
                result.InstanceOfThingId = null;
            }

            return result;
        }
    }
}
