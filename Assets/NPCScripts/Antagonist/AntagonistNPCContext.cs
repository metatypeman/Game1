using Assets.NPCScripts.Antagonist.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Antagonist
{
    public class AntagonistNPCContext : BaseNPCContextWithBlackBoard<AntagonistBlackBoard>
    {
        public AntagonistNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<AntagonistBootNPCProcess>();
            AddTypeOfProcess<AntagonistKeyListenerNPCProcess>();
        }

        public override void Bootstrap()
        {
            Bootstrap<AntagonistBootNPCProcess>();
        }
    }
}
