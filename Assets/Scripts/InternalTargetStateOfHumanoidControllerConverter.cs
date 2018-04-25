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
            result.HState = state.HState;

            if (state.TargetPosition == null)
            {
                result.TargetPosition = null;
            }
            else
            {
                result.TargetPosition = (Vector3)state.TargetPosition;
            }

            result.VState = state.VState;
            result.HandsState = state.HandsState;
            result.HandsActionState = state.HandsActionState;
            result.HeadState = state.HeadState;

            if (state.TargetHeadPosition == null)
            {
                result.TargetHeadPosition = null;
            }
            else
            {
                result.TargetHeadPosition = (Vector3)result.TargetHeadPosition;
            }

            result.KindOfThingsCommand = state.KindOfThingsCommand;
            result.InstanceOfThingId = state.InstanceOfThingId;

            return result;
        }
    }
}
