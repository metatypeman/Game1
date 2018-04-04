using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class DisagreementByTargetHeadPositionInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.TargetHeadPosition;
        public List<int> CurrentProcessesId { get; set; }
        public Vector3? CurrentValue { get; set; }
        public int TargetProcessId { get; set; }
        public Vector3? TargetValue { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByTargetHeadPositionInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByTargetHeadPositionInfo)}");
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
