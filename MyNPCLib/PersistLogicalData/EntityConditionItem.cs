using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class EntityConditionItem : IRefToRecord, ILogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public string Name { get; set; }
        public ulong Key { get; set; }
        public string VariableName { get; set; }
        public ulong VariableKey { get; set; }
        public IList<LogicalAnnotation> Annotations { get; set; }

        public EntityConditionItem Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new EntityConditionItem();
            result.Name = Name;
            result.Key = Key;
            result.VariableName = VariableName;
            result.VariableKey = VariableKey;
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
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
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(VariableName)} = {VariableName}");
            sb.AppendLine($"{spaces}{nameof(VariableKey)} = {VariableKey}");
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
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(VariableName)} = {VariableName}");
            sb.AppendLine($"{spaces}{nameof(VariableKey)} = {VariableKey}");
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
