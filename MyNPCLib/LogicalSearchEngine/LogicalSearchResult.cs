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
            mEntityDictionary = storage.EntityDictionary;
        }

        public ICGStorage Storage { get; private set; }
        private IEntityDictionary mEntityDictionary;
        public IndexedRuleInstance QueryExpression { get; set; }
        public IList<LogicalSearchResultItem> Items { get; set; }

        public IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultsListOfVar(keyOfVar);
        }

        public IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(ulong keyOfVar)
        {
            var targetSearchResultItemsList = Items;
            var resultList = new List<ResultOfVarOfQueryToRelation>();

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
                var resultItem = targetSearchResultItem.GetResultOfVar(keyOfVar);

                if (resultItem == null)
                {
                    continue;
                }

                resultList.Add(resultItem);
            }

            return resultList;
        }

        public IList<BaseVariant> GetResultsListOfVarAsVariant(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultsListOfVarAsVariant(keyOfVar);
        }

        public IList<BaseVariant> GetResultsListOfVarAsVariant(ulong keyOfVar)
        {
            var targetSearchResultItemsList = Items;
            var resultList = new List<BaseVariant>();

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
                var resultItem = targetSearchResultItem.GetResultOfVar(keyOfVar);

                if (resultItem == null)
                {
                    continue;
                }

                resultList.Add(resultItem.AsVariant);
            }

            return resultList;
        }

        public IList<object> GetResultsListOfVarAsObject(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultsListOfVarAsObject(keyOfVar);
        }

        public IList<object> GetResultsListOfVarAsObject(ulong keyOfVar)
        {
            var targetSearchResultItemsList = Items;
            var resultList = new List<object>();

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
                var resultItem = targetSearchResultItem.GetResultOfVar(keyOfVar);

                if (resultItem == null)
                {
                    continue;
                }

                var resultItemAsObject = resultItem.AsObject;

                if(resultItemAsObject != null)
                {
                    continue;
                }

                resultList.Add(resultItemAsObject);
            }

            return resultList;
        }

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

            return resultItem.AsObject;
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
