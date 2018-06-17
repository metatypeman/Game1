﻿using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchResultItem : IObjectToString
    {
        public LogicalSearchResultItem(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
        }
        private IEntityDictionary mEntityDictionary;
        public IndexedRuleInstance QueryExpression { get; set; }
        public IList<ResultOfVarOfQueryToRelation> ResultOfVarOfQueryToRelationList { get; set; }
        private readonly object mRuleInstanceLockObj = new object();
        private RuleInstance mRuleInstance;
        public RuleInstance RuleInstance
        {
            get
            {
                lock(mRuleInstanceLockObj)
                {
                    if(mRuleInstance == null)
                    {
                        mRuleInstance = ConvertorToCompleteRuleInstance.Convert(this, mEntityDictionary);
                    }

                    return mRuleInstance;
                }
            }
        }

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
            if (QueryExpression == null)
            {
                sb.AppendLine($"{spaces}{nameof(QueryExpression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QueryExpression)}");
                sb.Append(QueryExpression.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(QueryExpression)}");
            }
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
