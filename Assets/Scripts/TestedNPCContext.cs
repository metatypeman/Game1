using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestedNPCContext: BaseNPCContextWithBlackBoard<TestedBlackBoard>
    {
        public TestedNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext, QueriesCache queriesCache)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext, queriesCache)
        {
            AddTypeOfProcess<TestedBootNPCProcess>();
            AddTypeOfProcess<TestedKeyListenerNPCProcess>();
            AddTypeOfProcess<TestedGoToEnemyBaseNPCProcess>();
            AddTypeOfProcess<TestedInspectingNPCProcess>();
            AddTypeOfProcess<TestedRunAwayNPCProcess>();
            AddTypeOfProcess<TestedRunAtOurBaseNPCProcess>();
            AddTypeOfProcess<TestedSimpleAimNPCProcess>();
            AddTypeOfProcess<TestedFireToEthanNPCProcess>();
            AddTypeOfProcess<TestedRotateNPCProcess>();
            AddTypeOfProcess<TestedRotateHeadNPCProcess>();
            AddTypeOfProcess<TestedHeadToForvardNPCProcess>();
            AddTypeOfProcess<TestedMoveNPCProcess>();
            AddTypeOfProcess<TestedTakeFromSurfaceNPCProcess>();
            AddTypeOfProcess<TestedHideRifleToBagPackNPCProcess>();
            AddTypeOfProcess<TestedThrowOutToSurfaceRifleToSurfaceNPCProcess>();
            AddTypeOfProcess<TestedStartShootingNPCProcess>();
            AddTypeOfProcess<TestedStopShootingNPCProcess>();
            AddTypeOfProcess<TestedSearchNearNPCProcess>();
        }

        public override void Bootstrap()
        {
            Bootstrap<TestedBootNPCProcess>();
        }
    }
}
