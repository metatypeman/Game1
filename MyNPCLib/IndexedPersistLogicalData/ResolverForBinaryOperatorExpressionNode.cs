using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public abstract class ResolverForBinaryOperatorExpressionNode : ResolverForBaseExpressionNode
    {
        public ResolverForBaseExpressionNode Left { get; set; }
        public ResolverForBaseExpressionNode Right { get; set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            if (Left == null)
            {
                sb.AppendLine($"{spaces}{nameof(Left)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Left)}");
                sb.Append(Left.PropertiesToShortSting(nextN));
                sb.AppendLine($"{spaces}End{nameof(Left)}");
            }
            if (Right == null)
            {
                sb.AppendLine($"{spaces}{nameof(Right)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Right)}");
                sb.Append(Right.PropertiesToShortSting(nextN));
                sb.AppendLine($"{spaces}End{nameof(Right)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            if (Left == null)
            {
                sb.AppendLine($"{spaces}{nameof(Left)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Left)}");
                sb.Append(Left.PropertiesToShortSting(nextN));
                sb.AppendLine($"{spaces}End{nameof(Left)}");
            }

            if (Right == null)
            {
                sb.AppendLine($"{spaces}{nameof(Right)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(Right)}");
                sb.Append(Right.PropertiesToShortSting(nextN));
                sb.AppendLine($"{spaces}End{nameof(Right)}");
            }
            return sb.ToString();
        }
    }
}
