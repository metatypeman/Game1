using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InternalHumanoidThingsCommand: InternalMoveHumanoidCommand, IInternalHumanoidThingsCommand
    {
        public override InternalMoveHumanoidCommandKind Kind => InternalMoveHumanoidCommandKind.Things;
        public InternalKindOfHumanoidThingsCommand State { get; set; }
        public int InstanceId { get; set; }

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
