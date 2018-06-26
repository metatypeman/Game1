using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class VerbGrammaticalWordFrame : BaseGrammaticalWordFrame
    {
        public override GrammaticalPartOfSpeech PartOfSpeech => GrammaticalPartOfSpeech.Verb;
        public override bool IsVerb => true;
        public override VerbGrammaticalWordFrame AsVerb => this;
        public VerbType VerbType { get; set; } = VerbType.BaseForm;
        public GrammaticalNumberOfWord Number { get; set; } = GrammaticalNumberOfWord.Neuter;
        public GrammaticalPerson Person { get; set; } = GrammaticalPerson.Neuter;
        public GrammaticalTenses Tense { get; set; } = GrammaticalTenses.All;
        public bool IsModal { get; set; }
        public bool IsFormOfToBe { get; set; }
        public bool IsFormOfToHave { get; set; }
        public bool IsFormOfToDo { get; set; }
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(VerbType)} = {VerbType}");
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(Person)} = {Person}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(IsModal)} = {IsModal}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToBe)} = {IsFormOfToBe}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToHave)} = {IsFormOfToHave}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToDo)} = {IsFormOfToDo}");
            return sb.ToString();
        }
    }
}
