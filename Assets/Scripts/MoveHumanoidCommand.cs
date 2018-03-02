using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class MoveHumanoidCommand : IMoveHumanoidCommand
    {
        public abstract MoveHumanoidCommandKind Kind { get; }
        public abstract string ToString(int n);
        public virtual string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
