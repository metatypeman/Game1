using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class HumanoidBodyCommand : IHumanoidBodyCommand
    {
        public abstract HumanoidBodyCommandKind Kind { get; }
        public ulong InitiatingProcessId { get; set; }
        public abstract string ToString(uint n);
        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(InitiatingProcessId)} = {InitiatingProcessId}");
            return sb.ToString();
        }
    }
}
