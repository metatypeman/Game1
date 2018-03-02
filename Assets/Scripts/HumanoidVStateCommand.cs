using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class HumanoidVStateCommand : MoveHumanoidCommand, IHumanoidVStateCommand
    {
        public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.VState;
        public HumanoidVState State { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidVStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidVStateCommand)}");
            return sb.ToString();
        }

        public override string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            return sb.ToString();
        }
    }
}
