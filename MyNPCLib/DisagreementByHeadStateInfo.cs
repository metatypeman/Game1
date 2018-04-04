﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class DisagreementByHeadStateInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.HeadState;
        public List<int> CurrentProcessesId { get; set; }
        public HumanoidHeadState CurrentValue { get; set; } = HumanoidHeadState.LookingForward;
        public int TargetProcessId { get; set; }
        public HumanoidHeadState TargetValue { get; set; } = HumanoidHeadState.LookingForward;

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByHeadStateInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByHeadStateInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }

            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }
}