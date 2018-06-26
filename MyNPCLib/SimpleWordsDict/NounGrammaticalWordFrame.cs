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
        public GrammaticalGender Gender { get; set; }
        public GrammaticalNumberOfWord Number { get; set; }
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(IsName)} = {IsName}");
            sb.AppendLine($"{spaces}{nameof(Gender)} = {Gender}");
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            return sb.ToString();
        }
    }
}
