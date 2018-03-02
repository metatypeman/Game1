using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class HumanoidHandsStateCommand : MoveHumanoidCommand, IHumanoidHandsStateCommand
    {
        public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HandsState;
        public HumanoidHandsState State { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidHandsStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidHandsStateCommand)}");
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
