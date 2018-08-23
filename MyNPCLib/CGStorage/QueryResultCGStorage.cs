using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class QueryResultCGStorage : BaseRealStorage
    {
        public QueryResultCGStorage(ContextOfCGStorage context)
            : base(context)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.QueryResult;
    }
}
