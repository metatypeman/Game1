using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class FuzzyLogicalValueVariant : BaseVariant
    {
        public FuzzyLogicalValueVariant(float value)
        {
            Value = value;
        }

        public FuzzyLogicalValueVariant(float value, IList<LogicalAnnotation> annotations)
            : this(value)
        {
            Annotations = annotations;
        }

        public override KindOfVariant Kind => KindOfVariant.FuzzyLogicalValue;
        public override bool IsFuzzyLogicalValue => true;
        public override FuzzyLogicalValueVariant AsFuzzyLogicalValue => this;

        public IList<LogicalAnnotation> Annotations { get; private set; }
        public float Value { get; private set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Value)} = {Value}");
            return sb.ToString();
        }
    }
}
