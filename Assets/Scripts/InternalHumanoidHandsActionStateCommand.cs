using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InternalHumanoidHandsActionStateCommand : InternalMoveHumanoidCommand, IInternalHumanoidHandsActionStateCommand
    {
        public override InternalMoveHumanoidCommandKind Kind => InternalMoveHumanoidCommandKind.HandsActionState;
        public InternalHumanoidHandsActionState State { get; set; }

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
