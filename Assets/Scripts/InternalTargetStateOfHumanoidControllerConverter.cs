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

            result.TargetPosition = VectorsConvertor.NullableNumericToUnity(state.TargetPosition);

            result.VState = state.VState;
            result.HandsState = state.HandsState;
            result.HandsActionState = state.HandsActionState;
            result.HeadState = state.HeadState;

            result.TargetHeadPosition = VectorsConvertor.NullableNumericToUnity(state.TargetHeadPosition);

            result.KindOfThingsCommand = state.KindOfThingsCommand;
            result.EntityIdOfThing = state.EntityIdOfThing;

            return result;
        }
    }
}
