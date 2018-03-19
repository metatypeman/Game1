using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class TmpConcreteNPCContext: BaseNPCContext
    {
        public TmpConcreteNPCContext(IEntityDictionary entityDictionary = null)
            : base(entityDictionary)
        {
            AddTypeOfProcess<TmpConcreteNPCProcess>();
            Bootstrap<TmpConcreteNPCProcess>();
        }
    }
}
