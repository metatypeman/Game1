using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class HumanoidThingsCommand: MoveHumanoidCommand, IHumanoidThingsCommand
    {
        public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.Things;
        public KindOfHumanoidThingsCommand State { get; set; }
        public int InstanceId { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidThingsCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidThingsCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(int n)
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
