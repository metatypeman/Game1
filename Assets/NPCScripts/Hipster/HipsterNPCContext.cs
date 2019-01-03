using Assets.NPCScripts.Hipster.Processes;
using MyNPCLib;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.Parser.LogicalExpression;
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
            //AddTypeOfProcess<HipsterKeyListenerNPCProcess>();
            AddTypeOfProcess<HipsterGoToPointNPCProcess>();
            AddTypeOfProcess<HipsterSoundEntityConditionNPCProcess>(new SoundEventProcessOptions() {
                Kind = KindOfSoundEvent.EntityCondition
            });
            AddTypeOfProcess<HipsterSoundGoCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "go"
            });
            AddTypeOfProcess<HipsterSoundStopCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "stop"
            });
            AddTypeOfProcess<HipsterSoundContinueCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "continue"
            });
        }

        //public override void Bootstrap()
        //{
        //Bootstrap<HipsterBootNPCProcess>();
        //}
    }
}
