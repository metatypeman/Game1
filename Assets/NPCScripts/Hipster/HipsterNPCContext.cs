using Assets.NPCScripts.Hipster.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Hipster
{
    public class HipsterNPCContext : BaseNPCContextWithBlackBoard<HipsterBlackBoard>
    {
        public HipsterNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<HipsterBootNPCProcess>();
            AddTypeOfProcess<HipsterKeyListenerNPCProcess>();
            AddTypeOfProcess<HipsterGoToPointNPCProcess>();
        }

        //public override void Bootstrap()
        //{
            //Bootstrap<HipsterBootNPCProcess>();
        //}
    }
}
