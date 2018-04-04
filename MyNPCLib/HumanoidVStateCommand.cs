﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class HumanoidVStateCommand : HumanoidBodyCommand, IHumanoidVStateCommand
    {
        public override HumanoidBodyCommandKind Kind => HumanoidBodyCommandKind.VState;
        public HumanoidVState State { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public override string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidVStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidVStateCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            return sb.ToString();
        }
    }
}