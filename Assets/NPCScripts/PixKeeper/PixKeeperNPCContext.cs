using Assets.NPCScripts.PixKeeper.Processes;
using MyNPCLib;
using MyNPCLib.LogicalSoundModeling;
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
            //AddTypeOfProcess<PixKeeperBootNPCProcess>();
            AddTypeOfProcess<PixKeeperKeyListenerNPCProcess>();
            AddTypeOfProcess<PixKeeperGoToPointNPCProcess>();
            AddTypeOfProcess<PixKeeperSoundEntityConditionNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.EntityCondition
            });
            AddTypeOfProcess<PixKeeperSoundGoCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "go"
            });
            AddTypeOfProcess<PixKeeperSoundStopCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "stop"
            });
            AddTypeOfProcess<PixKeeperSoundContinueCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "continue"
            });
        }

        //public override void Bootstrap()
        //{
        //    Bootstrap<PixKeeperBootNPCProcess>();
        //}
    }
}
