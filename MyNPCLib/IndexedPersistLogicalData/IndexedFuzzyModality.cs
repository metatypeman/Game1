﻿using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public abstract class IndexedFuzzyModality : IObjectToString, IShortObjectToString
    {
        public abstract KindOfModality Kind { get; }
        public IndexedRuleInstance Parent { get; set; }
        public FuzzyModality Origin { get; set; }

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
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");

            if (Parent == null)
            {
                sb.AppendLine($"{spaces}{nameof(Parent)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Parent)}");
                sb.Append(Parent.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
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
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
