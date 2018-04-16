﻿using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestedNPCContext: BaseNPCContextWithBlackBoard<TestedBlackBoard>
    {
        public TestedNPCContext(IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<TestedBootNPCProcess>();
            AddTypeOfProcess<TestedKeyListenerProcess>();
            AddTypeOfProcess<TestedGoToEnemyBaseProcess>();
        }

        public override void Bootstrap()
        {
            Bootstrap<TestedBootNPCProcess>();
        }
    }
}