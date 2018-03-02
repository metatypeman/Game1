using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HumanoidHeadStateCommand : MoveHumanoidCommand, IHumanoidHeadStateCommand
    {
        public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HeadState;
        public HumanoidHeadState State { get; set; }
        public Vector3? TargetPosition { get; set; }
        public float Speed { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidHeadStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidHeadStateCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");

            if (TargetPosition.HasValue)
            {
                var targetPosition = TargetPosition.Value;
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }
            sb.AppendLine($"{spaces}{nameof(Speed)} = {Speed}");
            return sb.ToString();
        }
    }
}
