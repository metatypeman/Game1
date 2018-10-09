using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedAccessPolicyToFactModality: IIndexedLogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public AccessPolicyToFactModality Origin { get; set; }
        public KindOfAccessPolicyToFact Kind { get; set; }
        public ResolverForBaseExpressionNode Expression { get; set; }
        public IList<IndexedLogicalAnnotation> Annotations { get; set; }

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
            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
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
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
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
            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
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
