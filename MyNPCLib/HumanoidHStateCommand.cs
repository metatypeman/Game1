using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class HumanoidHStateCommand : HumanoidBodyCommand, IHumanoidHStateCommand
    {
        public override HumanoidBodyCommandKind Kind => HumanoidBodyCommandKind.HState;
        public HumanoidHState State { get; set; }
        public object TargetPosition { get; set; }
        public float Speed { get; set; }

        public NPCCommand ToNPCCommand()
        {
            return ToNPCCommand(this);
        }

        public static implicit operator NPCCommand(HumanoidHStateCommand command)
        {
            return ToNPCCommand(command);
        }

        public static NPCCommand ToNPCCommand(HumanoidHStateCommand command)
        {
            throw new NotImplementedException();
            return new NPCCommand();
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public override string ToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidHStateCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidHStateCommand)}");
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
