using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class BinaryOperatorOfQueryASTNode: BaseQueryASTNode
    {
        public override QueryASTNodeKind Kind => QueryASTNodeKind.BinaryOperator;
        public override bool IsBinaryOperator => true;
        public override BinaryOperatorOfQueryASTNode AsBinaryOperator => this;

        public KindOfBinaryOperators OperatorId { get; set; } = KindOfBinaryOperators.And;
        public BaseQueryASTNode Right { get; set; }
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
            if (Right == null)
            {
                sb.AppendLine($"{spaces}{nameof(Right)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Right)}");
                sb.Append(Right.ToString(nextN));
                sb.AppendLine($"{spaces}End{nameof(Right)}");
            }
            return sb.ToString();
        }
    }
}
