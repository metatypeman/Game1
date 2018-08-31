using MyNPCLib.CGStorage;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchResult: IObjectToString
    {
        public LogicalSearchResult(ICGStorage storage)
        {
            Storage = storage;
            mEntityDictionary = storage.Context.EntityDictionary;
        }

        public ICGStorage Storage { get; private set; }
        private IEntityDictionary mEntityDictionary;
        public IndexedRuleInstance QueryExpression { get; set; }
        public IList<LogicalSearchResultItem> Items { get; set; }

        public ResultOfVarOfQueryToRelation GetResultOfVar(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVar(keyOfVar);
        }

        public ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            var targetSearchResultItemsList = Items;

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
                var resultItem = targetSearchResultItem.GetResultOfVar(keyOfVar);

                if (resultItem == null)
                {
                    continue;
                }

                return resultItem;
            }

            return null;
        }

        public BaseVariant GetResultOfVarAsVariant(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVarAsVariant(keyOfVar);
        }

        public BaseVariant GetResultOfVarAsVariant(ulong keyOfVar)
        {
            var resultItem = GetResultOfVar(keyOfVar);

            if (resultItem == null)
            {
                return null;
            }

            return resultItem.AsVariant;
        }

        public object GetResultOfVarAsObject(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVarAsObject(keyOfVar);
        }

        public object GetResultOfVarAsObject(ulong keyOfVar)
        {
            var resultItem = GetResultOfVar(keyOfVar);

            if (resultItem == null)
            {
                return null;
            }

            return resultItem.AsVariant;
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
            if (Items == null)
            {
                sb.AppendLine($"{spaces}{nameof(Items)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Items)}");
                foreach (var item in Items)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Items)}");
            }
            return sb.ToString();
        }
    }
}
