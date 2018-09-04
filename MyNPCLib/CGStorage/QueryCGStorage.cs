using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class QueryCGStorage : BaseRealStorage
    {
        public QueryCGStorage(IEntityDictionary entityDictionary, RuleInstancePackage ruleInstancePackage)
            : base(entityDictionary, ruleInstancePackage)
        {
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.Query;
    }
}
