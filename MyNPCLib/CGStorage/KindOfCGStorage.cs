using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public enum KindOfCGStorage
    {
        Host,
        Global,
        Sharding,
        Local,
        Query,
        IndexedQuery,
        QueryResult,
        PassiveList,
        Consolidated,
        OtherProxy
    }
}
