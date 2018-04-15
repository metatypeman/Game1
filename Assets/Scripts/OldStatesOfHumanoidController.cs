using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib;

namespace Assets.Scripts
{
    public class OldStatesOfHumanoidController : IObjectToString
    {
        public OldHumanoidHState HState = OldHumanoidHState.Stop;
        public Vector3? TargetPosition;
        public OldHumanoidVState VState = OldHumanoidVState.Ground;
        public OldHumanoidHandsState HandsState = OldHumanoidHandsState.FreeHands;
        public OldHumanoidHandsActionState HandsActionState = OldHumanoidHandsActionState.Empty;
        public OldHumanoidHeadState HeadState = OldHumanoidHeadState.LookingForward;
        public Vector3? TargetHeadPosition;
        public OldKindOfHumanoidThingsCommand KindOfThingsCommand = OldKindOfHumanoidThingsCommand.Undefined;
        public int InstanceOfThingId;

        public OldStatesOfHumanoidController Clone()
        {
            var result = new OldStatesOfHumanoidController();
            result.HState = HState;
            result.TargetPosition = TargetPosition;
            result.VState = VState;
            result.HandsState = HandsState;
            result.HandsActionState = HandsActionState;
            result.HeadState = HeadState;
            result.TargetHeadPosition = TargetHeadPosition;
            result.KindOfThingsCommand = KindOfThingsCommand;
            result.InstanceOfThingId = InstanceOfThingId;
            return result;
        }

        public void Append(OldStatesOfHumanoidController source)
        {
            HState = source.HState;
            TargetPosition = source.TargetPosition;
            VState = source.VState;
            HandsState = source.HandsState;
            HandsActionState = source.HandsActionState;
            HeadState = source.HeadState;
            TargetHeadPosition = source.TargetHeadPosition;
            KindOfThingsCommand = source.KindOfThingsCommand;
            InstanceOfThingId = source.InstanceOfThingId;
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(HState)} = {HState}");

            if (TargetPosition.HasValue)
            {
                var targetPosition = TargetPosition.Value;
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {targetPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }

            sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
            sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");
            sb.AppendLine($"{spaces}{nameof(HeadState)} = {HeadState}");
            if (TargetHeadPosition.HasValue)
            {
                var targetPosition = TargetHeadPosition.Value;
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {targetPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
            }

            sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {KindOfThingsCommand}");
            sb.AppendLine($"{spaces}{nameof(InstanceOfThingId)} = {InstanceOfThingId}");

            return sb.ToString();
        }
    }
}
