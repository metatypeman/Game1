﻿using Assets.NPCScripts.Common.Logic;
using Assets.NPCScripts.Common.Logic.Processes;
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
    public class PixKeeperNPCContext : BaseNPCContextWithBlackBoard<CommonBlackBoard>
    {
        public PixKeeperNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            //AddTypeOfProcess<PixKeeperBootNPCProcess>();
            AddTypeOfProcess<PixKeeperKeyListenerNPCProcess>();
            AddTypeOfProcess<GoToPointNPCProcess>();
            AddTypeOfProcess<SoundEntityConditionNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.EntityCondition
            });
            AddTypeOfProcess<SoundGoCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "go"
            });
            AddTypeOfProcess<SoundStopCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "stop"
            });
            AddTypeOfProcess<SoundContinueCommandNPCProcess>(new SoundEventProcessOptions()
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
