using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StatesOfHumanoidBodyController : IObjectToString
    {
        public HumanoidHState HState = HumanoidHState.Stop;
        public object TargetPosition;
        public HumanoidVState VState = HumanoidVState.Ground;
        public HumanoidHandsState HandsState = HumanoidHandsState.FreeHands;
        public HumanoidHandsActionState HandsActionState = HumanoidHandsActionState.Empty;
        public HumanoidHeadState HeadState = HumanoidHeadState.LookingForward;
        public object TargetHeadPosition;
        public KindOfHumanoidThingsCommand KindOfThingsCommand = KindOfHumanoidThingsCommand.Undefined;
        public int InstanceOfThingId;

        public StatesOfHumanoidBodyController Clone()
        {
            var result = new StatesOfHumanoidBodyController();
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

        public void Append(StatesOfHumanoidBodyController source)
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
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(StatesOfHumanoidBodyController)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(StatesOfHumanoidBodyController)}");
            return sb.ToString();
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(HState)} = {HState}");

            if(TargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
            }

            sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
            sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");
            sb.AppendLine($"{spaces}{nameof(HeadState)} = {HeadState}");
            if (TargetHeadPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {TargetHeadPosition}");
            }

            sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {KindOfThingsCommand}");
            sb.AppendLine($"{spaces}{nameof(InstanceOfThingId)} = {InstanceOfThingId}");

            return sb.ToString();
        }

    }
}
