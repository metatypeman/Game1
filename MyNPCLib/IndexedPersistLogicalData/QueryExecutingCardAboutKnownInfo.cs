using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class QueryExecutingCardAboutKnownInfo : IObjectToString
    {
        public KindOfExpressionNode Kind { get; set; }
        public ulong Key { get; set; }
        public object Value { get; set; }
        public ulong? KeyOfVar { get; set; }
        public int? Position { get; set; }
        public BaseExpressionNode Expression { get; set; }

        public QueryExecutingCardAboutKnownInfo Clone()
        {
            var result = new QueryExecutingCardAboutKnownInfo();
            result.Kind = Kind;
            result.Key = Key;
            result.Value = Value;
            result.Expression = Expression;
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
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            sb.AppendLine($"{spaces}{nameof(KeyOfVar)} = {KeyOfVar}");
            sb.AppendLine($"{spaces}{nameof(Position)} = {Position}");
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
            return sb.ToString();
        }
    }
}
