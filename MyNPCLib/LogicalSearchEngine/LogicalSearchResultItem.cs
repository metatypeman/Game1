using MyNPCLib.CGStorage;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchResultItem : IObjectToString
    {
        public LogicalSearchResultItem(IEntityDictionary entityDictionary, ICGStorage storage, LogicalSearchResult parent)
        {
            mEntityDictionary = entityDictionary;
            Storage = storage;
            mParent = parent;
        }

        private IEntityDictionary mEntityDictionary;
        public IndexedRuleInstance QueryExpression { get; set; }

        public ICGStorage Storage { get; set; }
        public LogicalSearchResult mParent;

        public IList<ResultOfVarOfQueryToRelation> ResultOfVarOfQueryToRelationList { get; set; }
        private Dictionary<ulong, ResultOfVarOfQueryToRelation> mResultOfVarOfQueryToRelationDict = new Dictionary<ulong, ResultOfVarOfQueryToRelation>();

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

        public ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            if(mResultOfVarOfQueryToRelationDict.ContainsKey(keyOfVar))
            {
                return mResultOfVarOfQueryToRelationDict[keyOfVar];
            }

            return null;
        }

        public void Ready()
        {
            if(ResultOfVarOfQueryToRelationList == null)
            {
                return;
            }

            mResultOfVarOfQueryToRelationDict = ResultOfVarOfQueryToRelationList.ToDictionary(p => p.KeyOfVar, p => p);
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
