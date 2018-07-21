using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedLogicalAnnotation: IIndexedLogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public LogicalAnnotation Origin { get; set; }
        public IndexedRuleInstance RuleInstance { get; set; }
        public IList<IndexedLogicalAnnotation> Annotations { get; set; }

        public string GetHumanizeDbgString()
        {
            if(RuleInstance == null)
            {
                return string.Empty;
            }

            return RuleInstance.GetHumanizeDbgString();
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
            if (RuleInstance == null)
            {
                sb.AppendLine($"{spaces}{nameof(RuleInstance)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RuleInstance)}");
                sb.Append(RuleInstance.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RuleInstance)}");
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
            if (RuleInstance == null)
            {
                sb.AppendLine($"{spaces}{nameof(RuleInstance)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RuleInstance)}");
                sb.Append(RuleInstance.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(RuleInstance)}");
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
