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
            //SelfLogicalObject["name"] = "Tom";

            AddTypeOfProcess<HipsterBootNPCProcess>();
            AddTypeOfProcess<HipsterKeyListenerNPCProcess>();
            AddTypeOfProcess<HipsterGoToPointNPCProcess>();
            AddTypeOfProcess<HipsterSoundEntityConditionNPCProcess>(new SoundEventProcessOptions() {
                Kind = KindOfSoundEvent.EntityCondition
            });
            AddTypeOfProcess<HipsterSoundCommandNPCProcess>(new SoundEventProcessOptions()
            {
                Kind = KindOfSoundEvent.Command,
                ActionName = "go"
            });
        }

        //public override void Bootstrap()
        //{
        //Bootstrap<HipsterBootNPCProcess>();
        //}

//        protected override void OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
//        {
//#if DEBUG
//            Log($"logicalSoundPackage = {logicalSoundPackage}");
//#endif

//            GlobalCGStorage.Append(logicalSoundPackage.SoundFactsDataSource);

//            var actionName = string.Empty;

//            {
//                var queryStr = "{: ?Z(?X,?Y)[: {: action :} :] :}";

//                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, EntityDictionary);
//                var query = queryStorage.MainRuleInstance;

//#if DEBUG
//                {
//                    var debugStr = DebugHelperForRuleInstance.ToString(query);

//                    Log($"debugStr = {debugStr}");
//                }
//#endif
//                var querySearchResultCGStorage = MainCGStorage.Search(queryStorage);

//                var keyOfActionQuestionVar = EntityDictionary.GetKey("?Z");

//#if DEBUG
//                Log($"keyOfActionQuestionVar = {keyOfActionQuestionVar}");
//#endif

//                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

//#if DEBUG
//                LogInstance.Log($"actionExpression = {actionExpression}");
//#endif
//                if (actionExpression != null)
//                {
//                    actionName = actionExpression?.FoundExpression?.AsRelation.Name;
//                }
//            }

//#if DEBUG
//            Log($"!!!!!!!! :) actionName = {actionName}");
//#endif

//            if (string.IsNullOrWhiteSpace(actionName))
//            {
//                return;
//            }

//#if DEBUG
//            Log("NEXT");
//#endif

//            if (actionName == "go")
//            {
//                DispatchGo(actionName);
//                return;
//            }

//#if DEBUG
//            Log("Undefined action");
//#endif
//        }

//        private void DispatchGo(string actionName)
//        {
//#if DEBUG
//            Log($"actionName = {actionName}");
//#endif

//            var queryStr = "{: direction(go,?X) :}";

//            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, EntityDictionary);
//            var query = queryStorage.MainRuleInstance;

//#if DEBUG
//            {
//                var debugStr = DebugHelperForRuleInstance.ToString(query);

//                Log($"debugStr (for going) = {debugStr}");
//            }
//#endif
//            var querySearchResultCGStorage = MainCGStorage.Search(queryStorage);

//            var varNameOfDirection = "?X";

//            var targetValueOfDirection = querySearchResultCGStorage.GetResultOfVarAsVariant(varNameOfDirection);

//#if DEBUG
//            LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");
//#endif

//            var targetObject = GetLogicalObject(targetValueOfDirection);
//            var targetPosition = targetObject.GetValue<System.Numerics.Vector3?>("global position");

//#if DEBUG
//            Log($"targetPosition = {targetPosition}");
//#endif

//            var command = HipsterGoToPointNPCProcess.CreateCommand(targetPosition.Value);

//#if DEBUG
//            Log($"command = {command}");
//#endif

//            Send(command);
//        }
    }
}
