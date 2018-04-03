﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class HumanoidHeadStateCommand : HumanoidBodyCommand, IHumanoidHeadStateCommand
    {
        public override HumanoidBodyCommandKind Kind => HumanoidBodyCommandKind.HeadState;
        public HumanoidHeadState State { get; set; }
        public object TargetPosition { get; set; }
        public float Speed { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public override string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidHeadStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidHeadStateCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");

            if(TargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
            }
            sb.AppendLine($"{spaces}{nameof(Speed)} = {Speed}");
            return sb.ToString();
        }
    }
}
