using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class PrepositionGrammaticalWordFrame: BaseGrammaticalWordFrame
    {
        public override GrammaticalPartOfSpeech PartOfSpeech => GrammaticalPartOfSpeech.Preposition;
        public override bool IsPreposition => true;
        public override PrepositionGrammaticalWordFrame AsPreposition => this;

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            return sb.ToString();
        }
    }
}
