using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class HumanoidThingsCommand : HumanoidBodyCommand, IHumanoidThingsCommand
    {
        public override HumanoidBodyCommandKind Kind => HumanoidBodyCommandKind.Things;
        public KindOfHumanoidThingsCommand State { get; set; }
        public int InstanceId { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidThingsCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidThingsCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            sb.AppendLine($"{spaces}{nameof(InstanceId)} = {InstanceId}");
            return sb.ToString();
        }
    }
}
