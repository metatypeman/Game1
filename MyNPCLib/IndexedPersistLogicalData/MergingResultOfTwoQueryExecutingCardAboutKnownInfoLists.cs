using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class MergingResultOfTwoQueryExecutingCardAboutKnownInfoLists: IObjectToString
    {
        public bool IsSuccess { get; set; }
        public IList<QueryExecutingCardAboutKnownInfo> KnownInfoList { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(IsSuccess)} = {IsSuccess}");
            if (KnownInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(KnownInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(KnownInfoList)}");
                foreach (var knownInfo in KnownInfoList)
                {
                    sb.Append(knownInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(KnownInfoList)}");
            }
            return sb.ToString();
        }
    }
}
