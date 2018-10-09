using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class NounGrammaticalWordFrame: BaseGrammaticalWordFrame
    {
        public override GrammaticalPartOfSpeech PartOfSpeech => GrammaticalPartOfSpeech.Noun;
        public override bool IsNoun => true;
        public override NounGrammaticalWordFrame AsNoun => this;
        public bool IsName { get; set; }
        public bool IsShortForm { get; set; }
        public GrammaticalGender Gender { get; set; } = GrammaticalGender.Neuter;
        public GrammaticalNumberOfWord Number { get; set; } = GrammaticalNumberOfWord.Neuter;
        public bool IsCountable { get; set; }
        public bool IsGerund { get; set; }
        /// <summary>
        /// Example: `father's` is possessive, `father` is not possessive. 
        /// </summary>
        public bool IsPossessive { get; set; }
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(IsName)} = {IsName}");
            sb.AppendLine($"{spaces}{nameof(IsShortForm)} = {IsShortForm}");
            sb.AppendLine($"{spaces}{nameof(Gender)} = {Gender}");
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(IsCountable)} = {IsCountable}");
            sb.AppendLine($"{spaces}{nameof(IsGerund)} = {IsGerund}");
            sb.AppendLine($"{spaces}{nameof(IsPossessive)} = {IsPossessive}");
            return sb.ToString();
        }
    }
}
