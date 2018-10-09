using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class TmpConcreteNPCContext: BaseNPCContext
    {
        public TmpConcreteNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null)
            : base(entityLogger, entityDictionary, npcProcessInfoCache)
        {
            AddTypeOfProcess<TmpConcreteNPCProcess>();
            AddTypeOfProcess<TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess>();
            Bootstrap<TmpConcreteNPCProcess>();
        }
    }
}
