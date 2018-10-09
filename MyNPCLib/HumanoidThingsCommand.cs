using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class HumanoidThingsCommand : HumanoidBodyCommand, IHumanoidThingsCommand
    {
        public override HumanoidBodyCommandKind Kind => HumanoidBodyCommandKind.Things;
        public KindOfHumanoidThingsCommand State { get; set; }
        public BaseAbstractLogicalObject Thing { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public override string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            sb.AppendLine($"{spaces}{nameof(Thing)} = {Thing.ToString(nextN)}");
            return sb.ToString();
        }
    }
}
