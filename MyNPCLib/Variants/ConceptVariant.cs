using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Variants
{
    public class ConceptVariant : BaseVariant
    {
        public ConceptVariant(ulong key, string name)
        {
            Key = key;
            Name = name;
        }

        public ConceptVariant(ulong key, string name, IList<LogicalAnnotation> annotations)
            : this(key, name)
        {
            Annotations = annotations;
        }

        public override KindOfVariant Kind => KindOfVariant.Concept;

        public override bool IsConcept => true;
        public override ConceptVariant AsConcept => this;

        public IList<LogicalAnnotation> Annotations { get; private set; }
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
