using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class GCParsingResult : IObjectToString
    {
        public ConceptualGraph FistItem { get; set; }
        public IList<ConceptualGraph> Items { get; set; }

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
            if (FistItem == null)
            {
                sb.AppendLine($"{spaces}{nameof(FistItem)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(FistItem)}");
                sb.Append(FistItem.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(FistItem)}");
            }
            sb.AppendLine($"{spaces}Begin {nameof(Items)}");
            foreach (var item in Items)
            {
                sb.Append(item.ToString(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Items)}");
            return sb.ToString();
        }
    }
}
