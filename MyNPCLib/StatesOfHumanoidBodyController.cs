using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib
{
    public class StatesOfHumanoidBodyController : IObjectToString, IStatesOfHumanoidBodyHost
    {
        public HumanoidHState HState { get; set; } = HumanoidHState.Stop;
        public Vector3? TargetPosition { get; set; }
        public HumanoidVState VState { get; set; } = HumanoidVState.Ground;
        public HumanoidHandsState HandsState { get; set; } = HumanoidHandsState.FreeHands;
        public HumanoidHandsActionState HandsActionState { get; set; } = HumanoidHandsActionState.Empty;
        public HumanoidHeadState HeadState { get; set; } = HumanoidHeadState.LookingForward;
        public Vector3? TargetHeadPosition { get; set; }
        public KindOfHumanoidThingsCommand KindOfThingsCommand { get; set; } = KindOfHumanoidThingsCommand.Undefined;
        public ulong? EntityIdOfThing { get; set; }

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
            result.EntityIdOfThing = EntityIdOfThing;
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
            EntityIdOfThing = source.EntityIdOfThing;
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
            sb.AppendLine($"{spaces}{nameof(EntityIdOfThing)} = {EntityIdOfThing}");

            return sb.ToString();
        }
    }
}
