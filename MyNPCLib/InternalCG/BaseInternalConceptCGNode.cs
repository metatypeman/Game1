using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public abstract class BaseInternalConceptCGNode : BaseInternalCGNode
    {
        public override bool IsGraphOrConceptNode => true;
        public override BaseInternalConceptCGNode AsGraphOrConceptNode => this;
        public KindOfInternalGraphOrConceptNode KindOfGraphOrConcept { get; set; } = KindOfInternalGraphOrConceptNode.Undefined;
        public int? Power { get; set; }
        public KindOfTense Tense { get; set; } = KindOfTense.Undefined;
        public KindOfAspect Aspect { get; set; } = KindOfAspect.Undefined;
        public bool? Conditional { get; set; }
        public KindOfVoice Voice { get; set; } = KindOfVoice.Undefined;

        public void AddInputNode(InternalRelationCGNode node)
        {
            NAddInputNode(node);
            node.NAddOutputNode(this);
        }

        public void RemoveInputNode(InternalRelationCGNode node)
        {
            NRemoveInputNode(node);
            node.NRemoveOutputNode(this);
        }

        public void AddOutputNode(InternalRelationCGNode node)
        {
            NAddOutputNode(node);
            node.NAddInputNode(this);
        }

        public void RemoveOutputNode(InternalRelationCGNode node)
        {
            NRemoveOutputNode(node);
            node.NRemoveInputNode(this);
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(KindOfGraphOrConcept)} = {KindOfGraphOrConcept}");
            sb.AppendLine($"{spaces}{nameof(Power)} = {Power}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Conditional)} = {Conditional}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(KindOfGraphOrConcept)} = {KindOfGraphOrConcept}");
            sb.AppendLine($"{spaces}{nameof(Power)} = {Power}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Conditional)} = {Conditional}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
