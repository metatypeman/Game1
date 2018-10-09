using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class VarExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Var;
        public KindOfQuantifier Quantifier { get; set; }
        public RelationExpressionNode LinkedRelation { get; set; }
        public override bool IsVar => true;
        public override VarExpressionNode AsVar => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new VarExpressionNode();
            result.Quantifier = Quantifier;
            FillForClone(result, context);
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Quantifier)} = {Quantifier}");
            if (LinkedRelation == null)
            {
                sb.AppendLine($"{spaces}{nameof(LinkedRelation)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LinkedRelation)}");
                sb.Append(LinkedRelation.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(LinkedRelation)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            sb.AppendLine($"{spaces}{nameof(Quantifier)} = {Quantifier}");
            if (LinkedRelation == null)
            {
                sb.AppendLine($"{spaces}{nameof(LinkedRelation)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LinkedRelation)}");
                sb.Append(LinkedRelation.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(LinkedRelation)}");
            }
            return sb.ToString();
        }
    }
}
