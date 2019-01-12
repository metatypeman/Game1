using Assets.NPCScripts.Antagonist.Processes;
using Assets.NPCScripts.Common.Logic;
using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Antagonist
{
    public class AntagonistNPCContext : BaseNPCContextWithBlackBoard<CommonBlackBoard>
    {
        public AntagonistNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<AntagonistBootNPCProcess>();
            AddTypeOfProcess<AntagonistKeyListenerNPCProcess>();
            AddTypeOfProcess<TakeFromSurfaceNPCProcess>();
            AddTypeOfProcess<SimpleAimNPCProcess>();
            AddTypeOfProcess<StartShootingNPCProcess>();
            AddTypeOfProcess<StopShootingNPCProcess>();
        }

        public override void Bootstrap()
        {
            Bootstrap<AntagonistBootNPCProcess>();
        }
    }
}
