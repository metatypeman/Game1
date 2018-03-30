using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class TmpConcreteNPCContext: BaseNPCContext
    {
        public TmpConcreteNPCContext(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null)
            : base(entityDictionary, npcProcessInfoCache)
        {
            AddTypeOfProcess<TmpConcreteNPCProcess>();
            AddTypeOfProcess<TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess>();
            Bootstrap<TmpConcreteNPCProcess>();
        }
    }
}
