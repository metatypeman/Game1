using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseDisagreementInfo : IObjectToString
    {
        public abstract HumanoidStateKind Kind { get; }
        public List<ulong> CurrentProcessesId { get; set; }
        public ulong TargetProcessId { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            return sb.ToString();
        }
    }
}
