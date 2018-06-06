using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class RulePart : IObjectToString, IShortObjectToString
    {
        public bool IsActive { get; set; }
        public RuleInstance Parent { get; set; }
        public RulePart NextPart { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(IsActive)} = {IsActive}");

            if(Parent == null)
            {
                sb.AppendLine($"{spaces}{nameof(Parent)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Parent)}");
                sb.Append(Parent.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
            }

            if(NextPart == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextPart)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextPart)}");
                sb.Append(NextPart.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NextPart)}");
            }

            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsActive)} = {IsActive}");
            return sb.ToString();
        }
    }
}
