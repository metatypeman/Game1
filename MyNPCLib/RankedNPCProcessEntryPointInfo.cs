using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class RankedNPCProcessEntryPointInfo : IObjectToString
    {
        public float Rank { get; set; }
        public NPCProcessEntryPointInfo EntryPoint { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Rank)} = {Rank}");
            if(EntryPoint == null)
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
