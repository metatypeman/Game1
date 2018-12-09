using Assets.NPCScripts.PixKeeper.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.PixKeeper
{
    public class PixKeeperNPCContext : BaseNPCContextWithBlackBoard<PixKeeperBlackBoard>
    {
        public PixKeeperNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<PixKeeperBootNPCProcess>();
            AddTypeOfProcess<PixKeeperKeyListenerNPCProcess>();
        }

        //public override void Bootstrap()
        //{
        //    Bootstrap<PixKeeperBootNPCProcess>();
        //}
    }
}
