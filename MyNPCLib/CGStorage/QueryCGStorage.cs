using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class QueryCGStorage : BaseCGStorage
    {
        public QueryCGStorage(ContextOfCGStorage context)
            : base(context)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Query;
    }
}
