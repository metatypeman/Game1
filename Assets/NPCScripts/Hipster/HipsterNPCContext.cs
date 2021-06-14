//using Assets.NPCScripts.Common.Logic;
//using Assets.NPCScripts.Common.Logic.Processes;
//using Assets.NPCScripts.Hipster.Processes;
//using MyNPCLib;
//using MyNPCLib.DebugHelperForPersistLogicalData;
//using MyNPCLib.LogicalSoundModeling;
//using MyNPCLib.Parser.LogicalExpression;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Hipster
//{
//    public class HipsterNPCContext : BaseNPCContextWithBlackBoard<CommonBlackBoard>
//    {
//        public HipsterNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
//            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
//        {
//            AddTypeOfProcess<HipsterBootNPCProcess>();
//            AddTypeOfProcess<HipsterKeyListenerNPCProcess>();
//            AddTypeOfProcess<GoToPointNPCProcess>();
//            AddTypeOfProcess<SoundEntityConditionNPCProcess>(new SoundEventProcessOptions() {
//                Kind = KindOfSoundEvent.EntityCondition
//            });
//            AddTypeOfProcess<SoundGoCommandNPCProcess>(new SoundEventProcessOptions()
//            {
//                Kind = KindOfSoundEvent.Command,
//                ActionName = "go"
//            });
//            AddTypeOfProcess<SoundStopCommandNPCProcess>(new SoundEventProcessOptions()
//            {
//                Kind = KindOfSoundEvent.Command,
//                ActionName = "stop"
//            });
//            AddTypeOfProcess<SoundContinueCommandNPCProcess>(new SoundEventProcessOptions()
//            {
//                Kind = KindOfSoundEvent.Command,
//                ActionName = "continue"
//            });
//        }

//        //public override void Bootstrap()
//        //{
//        //Bootstrap<HipsterBootNPCProcess>();
//        //}
//    }
//}
