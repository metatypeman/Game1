using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public abstract class BaseVariant : IObjectToString
    {
        public abstract KindOfVariant Kind { get; }

        public virtual bool IsEmpty => false;
        public virtual EmptyVariant AsEmpty => null;

        public virtual bool IsConcept => false;
        public virtual ConceptVariant AsConcept => null;

        public virtual bool IsEntity => false;
        public virtual EntityVariant AsEntity => null;

        public virtual bool IsValue => false;
        public virtual ValueVariant AsValue => null;

        public virtual bool IsFuzzyLogicalValue => false;
        public virtual FuzzyLogicalValueVariant AsFuzzyLogicalValue => null;

        public virtual bool IsFact => false;
        public virtual FactVariant AsFact => null;

        public virtual bool IsEntityCondition => false;
        public virtual EntityConditionVariant AsEntityCondition => null;

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
