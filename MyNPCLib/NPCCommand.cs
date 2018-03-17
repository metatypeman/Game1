using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCCommand : INPCCommand
    {
        public string Name { get; set; }
        public int InitiatingProcessId { get; set; }
        public KindOfLinkingToInitiator KindOfLinkingToInitiator { get; set; } = KindOfLinkingToInitiator.Standalone;
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCCommand)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCCommand)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(InitiatingProcessId)} = {InitiatingProcessId}");
            sb.AppendLine($"{spaces}{nameof(KindOfLinkingToInitiator)} = {KindOfLinkingToInitiator}");
            if(Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                var nextN = n + 4;
                var nextSpaces = StringHelper.Spaces(nextN);
                sb.AppendLine($"{spaces}Begin{nameof(Params)}");
                foreach(var paramItem in Params)
                {
                    sb.AppendLine($"{nextSpaces}ParamName = {paramItem.Key}; ParamValue = {paramItem.Value}");
                }
                sb.AppendLine($"{spaces}End{nameof(Params)}");
            }
            return sb.ToString();
        }
    }
}
