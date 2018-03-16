using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessInfo : IObjectToString
    {
        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCProcessInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCProcessInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
