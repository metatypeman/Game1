using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.PhraseTree
{
    public class PrepositionalPhrase : BaseNounLikePhrase
    {
        public ATNExtendedToken Preposition { get; set; }
        public override bool IsPrepositionalPhrase => true;
        public override PrepositionalPhrase AsPrepositionalPhrase => this;

        public override BaseNounLikePhrase Fork()
        {
            var result = new PrepositionalPhrase();
            result.Preposition = Preposition;
            result.Object = Object?.Fork();
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Preposition == null)
            {
                sb.AppendLine($"{spaces}{nameof(Preposition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Preposition)}");
                sb.Append(Preposition.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Preposition)}");
            }
            if (Object == null)
            {
                sb.AppendLine($"{spaces}{nameof(Object)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Object)}");
                sb.Append(Object.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Object)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (Preposition == null)
            {
                sb.AppendLine($"{spaces}{nameof(Preposition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Preposition)}");
                sb.Append(Preposition.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Preposition)}");
            }
            if (Object == null)
            {
                sb.AppendLine($"{spaces}{nameof(Object)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Object)}");
                sb.Append(Object.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Object)}");
            }
            return sb.ToString();
        }
    }
}
