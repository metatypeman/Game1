using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.Variants;
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
            return mLogicalSearchResult.GetResultOfVar(keyOfVar);
        }

        public override BaseVariant GetResultOfVarAsVariant(ulong keyOfVar)
        {
            return mLogicalSearchResult.GetResultOfVarAsVariant(keyOfVar);
        }

        public override object GetResultOfVarAsObject(ulong keyOfVar)
        {
            return mLogicalSearchResult.GetResultOfVarAsObject(keyOfVar);
        }
    }
}
