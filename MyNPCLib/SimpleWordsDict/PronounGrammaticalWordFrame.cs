using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class PronounGrammaticalWordFrame : BaseGrammaticalWordFrame
    {
        public override GrammaticalPartOfSpeech PartOfSpeech => GrammaticalPartOfSpeech.Pronoun;
        public override bool IsPronoun => true;
        public override PronounGrammaticalWordFrame AsPronoun => this;
        public GrammaticalGender Gender { get; set; } = GrammaticalGender.Neuter;
        public GrammaticalNumberOfWord Number { get; set; } = GrammaticalNumberOfWord.Neuter;
        public GrammaticalPerson Person { get; set; } = GrammaticalPerson.Neuter;
        public TypeOfPronoun TypeOfPronoun { get; set; } = TypeOfPronoun.Undefined;
        public CaseOfPersonalPronoun Case { get; set; } = CaseOfPersonalPronoun.Undefined;
        public bool IsQuestionWord { get; set; }
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Gender)} = {Gender}");
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(Person)} = {Person}");
            sb.AppendLine($"{spaces}{nameof(TypeOfPronoun)} = {TypeOfPronoun}");
            sb.AppendLine($"{spaces}{nameof(Case)} = {Case}");
            sb.AppendLine($"{spaces}{nameof(IsQuestionWord)} = {IsQuestionWord}");
            return sb.ToString();
        }
    }
}
