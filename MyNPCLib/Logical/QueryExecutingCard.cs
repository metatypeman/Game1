using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class QueryExecutingCard : IObjectToString
    {
        public IList<ulong> EntitiesIdList { get; set; }

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
            var sb = new StringBuilder();
            if (EntitiesIdList == null)
            {
                sb.AppendLine($"{spaces}{nameof(EntitiesIdList)} = null");
            }
            else
            {
                var nextN = n + 4;
                var nextSpaces = StringHelper.Spaces(nextN);
                sb.AppendLine($"{spaces}EntitiesIdList.Count = {EntitiesIdList.Count}");
                sb.AppendLine($"{spaces}Begin{nameof(EntitiesIdList)}");
                foreach (var entityId in EntitiesIdList)
                {
                    sb.AppendLine($"{nextSpaces}entityId = {entityId}");
                }
                sb.AppendLine($"{spaces}End{nameof(EntitiesIdList)}");
            }
            return sb.ToString();
        }
    }
}
