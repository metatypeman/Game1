using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCProcessInvocablePackage : IObjectToString
    {
        public BaseNPCProcess Process { get; set; }
        public NPCInternalCommand Command { get; set; }
        public float RankOfEntryPoint { get; set; }
        public NPCProcessEntryPointInfo EntryPoint { get; set; }

        public INPCProcess RunAsync()
        {
            return Process.RunAsync(Command, EntryPoint);
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Process == null)
            {
                sb.AppendLine($"{spaces}{nameof(Process)} = null");
            }
            else
            {
                sb.Append($"{spaces}Process.Id = {Process.Id}");
                sb.Append($"{spaces}Process.State = {Process.State}");
                sb.Append($"{spaces}Process.Kind = {Process.Kind}");
            }
            if (Command == null)
            {
                sb.AppendLine($"{spaces}{nameof(Command)} = null");
            }
            else
            {
                sb.Append($"{spaces}{nameof(Command)} = {Command.ToString(nextN)}");
            }
            sb.AppendLine($"{spaces}{nameof(RankOfEntryPoint)} = {RankOfEntryPoint}");
            if (EntryPoint == null)
            {
                sb.AppendLine($"{spaces}{nameof(EntryPoint)} = null");
            }
            else
            {
                sb.Append($"{spaces}{nameof(EntryPoint)} = {EntryPoint.ToString(nextN)}");
            }
            return sb.ToString();
        }
    }
}
