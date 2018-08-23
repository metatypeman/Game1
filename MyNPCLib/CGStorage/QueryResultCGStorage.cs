using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class QueryResultCGStorage : BaseProxyStorage
    {
        public QueryResultCGStorage(ContextOfCGStorage context, LogicalSearchResult logicalSearchResult)
            : base(context)
        {
            mLogicalSearchResult = logicalSearchResult;
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.QueryResult;

        private LogicalSearchResult mLogicalSearchResult;

        public override ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            var targetSearchResultItemsList = mLogicalSearchResult.Items;

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
                var resultItem = targetSearchResultItem.GetResultOfVar(keyOfVar);

                if(resultItem == null)
                {
                    continue;
                }

                return resultItem;
            }

            return null;
        }
    }
}
