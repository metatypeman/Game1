using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class DisagreementByHStateInfo : BaseDisagreementInfo
    {
        public override HumanoidStateKind Kind => HumanoidStateKind.HState;
        public HumanoidHState CurrentValue { get; set; } = HumanoidHState.Stop;
        public HumanoidHState TargetValue { get; set; } = HumanoidHState.Stop;

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
