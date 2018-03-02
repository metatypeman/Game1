using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class MoveHumanoidCommandsPackage : IMoveHumanoidCommandsPackage
    {
        public List<IMoveHumanoidCommand> Commands { get; set; } = new List<IMoveHumanoidCommand>();

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(MoveHumanoidCommandsPackage)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(MoveHumanoidCommandsPackage)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Commands == null)
            {
                sb.AppendLine($"{spaces}{nameof(Commands)} == null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(Commands)}.Count = {Commands.Count}");
                sb.AppendLine($"{spaces}Begin {nameof(Commands)}");
                foreach (var command in Commands)
                {
                    sb.Append(command.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Commands)}");
            }
            return sb.ToString();
        }
    }
}
