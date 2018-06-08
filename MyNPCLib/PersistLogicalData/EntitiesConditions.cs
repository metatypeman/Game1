using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class EntitiesConditions : ILogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public IList<EntityConditionItem> Items { get; set; }
        public IList<LogicalAnnotation> Annotations { get; set; }

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
            if (Items == null)
            {
                sb.AppendLine($"{spaces}{nameof(Items)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Items)}");
                foreach (var item in Items)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Items)}");
            }
            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
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
            if (Items == null)
            {
                sb.AppendLine($"{spaces}{nameof(Items)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Items)}");
                foreach (var item in Items)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Items)}");
            }
            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }
    }
}
