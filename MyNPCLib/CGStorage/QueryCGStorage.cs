using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class QueryCGStorage : BaseRealStorage
    {
        public QueryCGStorage(ContextOfCGStorage context, RuleInstancePackage ruleInstancePackage)
            : base(context, ruleInstancePackage)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Query;
    }
}
