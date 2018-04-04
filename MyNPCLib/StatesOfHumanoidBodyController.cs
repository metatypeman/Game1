using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StatesOfHumanoidBodyController : IObjectToString
    {
        public HumanoidHState HState = HumanoidHState.Stop;
        public Vector3? TargetPosition;
        public HumanoidVState VState = HumanoidVState.Ground;
        public HumanoidHandsState HandsState = HumanoidHandsState.FreeHands;
        public HumanoidHandsActionState HandsActionState = HumanoidHandsActionState.Empty;
        public HumanoidHeadState HeadState = HumanoidHeadState.LookingForward;
        public Vector3? TargetHeadPosition;
        public KindOfHumanoidThingsCommand KindOfThingsCommand = KindOfHumanoidThingsCommand.Undefined;
        public int InstanceOfThingId;

        public StatesOfHumanoidController Clone()
        {
            var result = new StatesOfHumanoidController();
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

        public void Append(StatesOfHumanoidController source)
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
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(StatesOfHumanoidController)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(StatesOfHumanoidController)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
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
