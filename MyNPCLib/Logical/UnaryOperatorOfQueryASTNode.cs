using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class UnaryOperatorOfQueryASTNode : BaseQueryASTNode
    {
        public override QueryASTNodeKind Kind => QueryASTNodeKind.UnaryOperator;
        public override bool IsUnaryOperator => true;
        public override UnaryOperatorOfQueryASTNode AsUnaryOperator => this;

        public ulong OperatorId { get; set; }

        public BaseQueryASTNode Left { get; set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(OperatorId)} = {OperatorId}");
            if (Left == null)
            {
                sb.AppendLine($"{spaces}{nameof(Left)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Left)}");
                sb.Append(Left.ToString(nextN));
                sb.AppendLine($"{spaces}End{nameof(Left)}");
            }
            return sb.ToString();
        }
    }
}
