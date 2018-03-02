using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BehaviourFlagsOfHumanoidController : IObjectToString
    {
        public bool HasRifle;
        public bool Walk;
        public bool IsAim;
        public bool IsDead;

        public void Append(BehaviourFlagsOfHumanoidController source)
        {
            HasRifle = source.HasRifle;
            Walk = source.Walk;
            IsAim = source.IsAim;
            IsDead = source.IsDead;
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(BehaviourFlagsOfHumanoidController)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(BehaviourFlagsOfHumanoidController)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(HasRifle)} = {HasRifle}");
            sb.AppendLine($"{spaces}{nameof(Walk)} = {Walk}");
            sb.AppendLine($"{spaces}{nameof(IsAim)} = {IsAim}");
            sb.AppendLine($"{spaces}{nameof(IsDead)} = {IsDead}");

            return sb.ToString();
        }
    }
}
