using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class LogicalAnnotation : ILogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public ulong RuleInstanceKey { get; set; }
        public IList<LogicalAnnotation> Annotations { get; set; }

        public LogicalAnnotation Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new LogicalAnnotation();
            result.RuleInstanceKey = RuleInstanceKey;
            result.Annotations = CloneListOfAnnotations(Annotations, context);
            return result;
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
            sb.AppendLine($"{spaces}{nameof(RuleInstanceKey)} = {RuleInstanceKey}");

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
            sb.AppendLine($"{spaces}{nameof(RuleInstanceKey)} = {RuleInstanceKey}");

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

        public static IList<LogicalAnnotation> CloneListOfAnnotations(IList<LogicalAnnotation> sourceList, CloneContextOfPersistLogicalData context)
        {
            if(sourceList == null)
            {
                return null;
            }

            var resultAnnotationsList = new List<LogicalAnnotation>();
            foreach(var annotation in sourceList)
            {
                resultAnnotationsList.Add(annotation.Clone(context));
            }
            return resultAnnotationsList;
        }
    }
}
