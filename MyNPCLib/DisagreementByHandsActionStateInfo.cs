using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class DisagreementByHandsActionStateInfo : BaseDisagreementInfo
    {
        public override HumanoidStateKind Kind => HumanoidStateKind.HandsActionState;
        public HumanoidHandsActionState CurrentValue { get; set; } = HumanoidHandsActionState.Empty;
        public HumanoidHandsActionState TargetValue { get; set; } = HumanoidHandsActionState.Empty;

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }
}
