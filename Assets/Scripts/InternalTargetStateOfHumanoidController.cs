using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib;

namespace Assets.Scripts
{
    public class InternalTargetStateOfHumanoidController : IObjectToString
    {
        public HumanoidHState? HState { get; set; }
        public Vector3? TargetPosition { get; set; }
        public HumanoidVState? VState { get; set; }
        public HumanoidHandsState? HandsState { get; set; }
        public HumanoidHandsActionState? HandsActionState { get; set; }
        public HumanoidHeadState? HeadState { get; set; }
        public Vector3? TargetHeadPosition { get; set; }
        public KindOfHumanoidThingsCommand? KindOfThingsCommand { get; set; }
        public ulong? EntityIdOfThing { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            if (HState.HasValue)
            {
                var state = HState.Value;
                sb.AppendLine($"{spaces}{nameof(HState)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(HState)} = null");
            }

            if (TargetPosition.HasValue)
            {
                var targetPosition = TargetPosition.Value;
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }

            if (VState.HasValue)
            {
                var state = VState.Value;
                sb.AppendLine($"{spaces}{nameof(VState)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(VState)} = null");
            }

            if (HandsState.HasValue)
            {
                var state = HandsState.Value;
                sb.AppendLine($"{spaces}{nameof(HandsState)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(HandsState)} = null");
            }

            if (HandsActionState.HasValue)
            {
                var state = HandsActionState.Value;
                sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(HandsActionState)} = null");
            }

            if (HeadState.HasValue)
            {
                var state = HeadState.Value;
                sb.AppendLine($"{spaces}{nameof(HeadState)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(HeadState)} = null");
            }

            if (TargetHeadPosition.HasValue)
            {
                var state = TargetHeadPosition.Value;
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
            }

            if(KindOfThingsCommand.HasValue)
            {
                var state = KindOfThingsCommand.Value;
                sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = null");
            }

            if (EntityIdOfThing.HasValue)
            {
                var state = EntityIdOfThing.Value;
                sb.AppendLine($"{spaces}{nameof(EntityIdOfThing)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(EntityIdOfThing)} = null");
            }

            return sb.ToString();
        }
    }
}
