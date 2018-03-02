using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TargetStateOfHumanoidController : IObjectToString
    {
        public HumanoidHState? HState { get; set; }
        public Vector3? TargetPosition { get; set; }
        public HumanoidVState? VState { get; set; }
        public HumanoidHandsState? HandsState { get; set; }
        public HumanoidHandsActionState? HandsActionState { get; set; }
        public HumanoidHeadState? HeadState { get; set; }
        public Vector3? TargetHeadPosition { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(TargetStateOfHumanoidController)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(TargetStateOfHumanoidController)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
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

            return sb.ToString();
        }
    }
}
