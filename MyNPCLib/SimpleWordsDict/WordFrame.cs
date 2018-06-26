using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    [Serializable]
    public class WordFrame : IObjectToString
    {
        public string Word { get; set; }
        public IList<BaseGrammaticalWordFrame> GrammaticalWordFrames { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Word)} = {Word}");
            if (GrammaticalWordFrames == null)
            {
                sb.AppendLine($"{spaces}{nameof(GrammaticalWordFrames)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(GrammaticalWordFrames)}");
                foreach (var grammaticalWordFrame in GrammaticalWordFrames)
                {
                    sb.Append(grammaticalWordFrame.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(GrammaticalWordFrames)}");
            }
            return sb.ToString();
        }
    }
}
