using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class EntityVariant: BaseVariant
    {
        public EntityVariant(EntityRefExpressionNode expressionNode)
        {
            mExpressionNode = expressionNode;
            Key = expressionNode.Key;
            Name = expressionNode.Name;
        }

        public override KindOfVariant Kind => KindOfVariant.Entity;
        public override bool IsEntity => true;
        public override EntityVariant AsEntity => this;

        private EntityRefExpressionNode mExpressionNode;
        public ulong Key { get; private set; }
        public string Name { get; private set; }
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }
    }
}
