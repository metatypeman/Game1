using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class NumeralGrammaticalWordFrame: BaseGrammaticalWordFrame
    {
        public override GrammaticalPartOfSpeech PartOfSpeech => GrammaticalPartOfSpeech.Numeral;
        public override bool IsNumeral => true;
        public override NumeralGrammaticalWordFrame AsNumeral => this;
        public NumeralType NumeralType { get; set; } = NumeralType.Undefined;
        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(NumeralType)} = {NumeralType}");
            return sb.ToString();
        }
    }
}
