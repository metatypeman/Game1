﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class TargetStateOfHumanoidBody : IObjectToString
    {
        public HumanoidHState? HState { get; set; }
        public object TargetPosition { get; set; }
        public HumanoidVState? VState { get; set; }
        public HumanoidHandsState? HandsState { get; set; }
        public HumanoidHandsActionState? HandsActionState { get; set; }
        public HumanoidHeadState? HeadState { get; set; }
        public object TargetHeadPosition { get; set; }
        public KindOfHumanoidThingsCommand? KindOfThingsCommand { get; set; }
        public int? InstanceOfThingId { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(TargetStateOfHumanoidBody)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(TargetStateOfHumanoidBody)}");
            return sb.ToString();
        }

        public string PropertiesToSting(uint n)
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

            if(TargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
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

            if(TargetHeadPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {TargetHeadPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
            }

            if (KindOfThingsCommand.HasValue)
            {
                var state = KindOfThingsCommand.Value;
                sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = null");
            }

            if (InstanceOfThingId.HasValue)
            {
                var state = InstanceOfThingId.Value;
                sb.AppendLine($"{spaces}{nameof(InstanceOfThingId)} = {state}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(InstanceOfThingId)} = null");
            }

            return sb.ToString();
        }
    }
}