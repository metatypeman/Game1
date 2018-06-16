using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ResultOfVarOfQueryToRelation : IObjectToString
    {
        public ulong KeyOfVar { get; set; }
        public BaseExpressionNode FoundExpression { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(KeyOfVar)} = {KeyOfVar}");
            if (FoundExpression == null)
            {
                sb.AppendLine($"{spaces}{nameof(FoundExpression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(FoundExpression)}");
                sb.Append(FoundExpression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(FoundExpression)}");
            }
            return sb.ToString();
        }
    }
}
