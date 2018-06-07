﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    /// <summary>
    /// Represents instance of rule (or fact) in the storage.
    /// </summary>
    [Serializable]
    public class RuleInstance: IObjectToString, IShortObjectToString
    {
        public string Name { get; set; }
        public ulong Key { get; set; }
        public bool IsPart_1_Active { get; set; }
        public bool IsPart_2_Active { get; set; }
        public RulePart Part_1 { get; set; }
        public RulePart Part_2 { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsPart_1_Active)} = {IsPart_1_Active}");
            sb.AppendLine($"{spaces}{nameof(IsPart_2_Active)} = {IsPart_2_Active}");
            if(Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }

            if(Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
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
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsPart_1_Active)} = {IsPart_1_Active}");
            sb.AppendLine($"{spaces}{nameof(IsPart_2_Active)} = {IsPart_2_Active}");
            if (Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }

            if (Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
            }
            return sb.ToString();
        }
    }
}