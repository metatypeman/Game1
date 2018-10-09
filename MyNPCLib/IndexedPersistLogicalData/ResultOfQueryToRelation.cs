using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ResultOfQueryToRelation : IObjectToString, IObjectToBriefString
    {
        public IList<ResultOfVarOfQueryToRelation> ResultOfVarOfQueryToRelationList { get; set; } = new List<ResultOfVarOfQueryToRelation>();
        
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
            if (ResultOfVarOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultOfVarOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultOfVarOfQueryToRelationList)}");
                foreach (var resultOfVarOfQueryToRelation in ResultOfVarOfQueryToRelationList)
                {
                    sb.Append(resultOfVarOfQueryToRelation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultOfVarOfQueryToRelationList)}");
            }
            return sb.ToString();
        }

        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (ResultOfVarOfQueryToRelationList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ResultOfVarOfQueryToRelationList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ResultOfVarOfQueryToRelationList)}");
                foreach (var resultOfVarOfQueryToRelation in ResultOfVarOfQueryToRelationList)
                {
                    sb.Append(resultOfVarOfQueryToRelation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ResultOfVarOfQueryToRelationList)}");
            }
            return sb.ToString();
        }
    }
}
