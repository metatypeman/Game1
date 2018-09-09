using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LegacyConvertors;
using MyNPCLib.Logical;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.Parser.LogicalExpression;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestedNPCContext: BaseNPCContextWithBlackBoard<TestedBlackBoard>
    {
        public TestedNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            AddTypeOfProcess<TestedBootNPCProcess>();
            AddTypeOfProcess<TestedKeyListenerNPCProcess>();
            AddTypeOfProcess<TestedGoToEnemyBaseNPCProcess>();
            AddTypeOfProcess<TestedInspectingNPCProcess>();
            AddTypeOfProcess<TestedRunAwayNPCProcess>();
            AddTypeOfProcess<TestedRunAtOurBaseNPCProcess>();
            AddTypeOfProcess<TestedSimpleAimNPCProcess>();
            AddTypeOfProcess<TestedFireToEthanNPCProcess>();
            AddTypeOfProcess<TestedRotateNPCProcess>();
            AddTypeOfProcess<TestedRotateHeadNPCProcess>();
            AddTypeOfProcess<TestedHeadToForvardNPCProcess>();
            AddTypeOfProcess<TestedMoveNPCProcess>();
            AddTypeOfProcess<TestedTakeFromSurfaceNPCProcess>();
            AddTypeOfProcess<TestedHideRifleToBagPackNPCProcess>();
            AddTypeOfProcess<TestedThrowOutToSurfaceRifleToSurfaceNPCProcess>();
            AddTypeOfProcess<TestedStartShootingNPCProcess>();
            AddTypeOfProcess<TestedStopShootingNPCProcess>();
            AddTypeOfProcess<TestedSearchNearNPCProcess>();
            AddTypeOfProcess<TSTGoToPointNPCProcess>();
        }

        public override void Bootstrap()
        {
            Bootstrap<TestedBootNPCProcess>();
        }

        protected override void OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            GlobalCGStorage.Append(logicalSoundPackage.SoundFactsDataSource);

            var actionName = string.Empty;

            {
                var queryStr = "{: ?Z(?X,?Y)[: {: action :} :] :}";

                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, EntityDictionary);
                var query = queryStorage.MainRuleInstance;

#if DEBUG
                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    Log($"debugStr = {debugStr}");
                }
#endif
                var querySearchResultCGStorage = MainCGStorage.Search(queryStorage);

                var keyOfActionQuestionVar = EntityDictionary.GetKey("?Z");

#if DEBUG
                Log($"keyOfActionQuestionVar = {keyOfActionQuestionVar}");
#endif

                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

#if DEBUG
                LogInstance.Log($"actionExpression = {actionExpression}");
#endif
                if (actionExpression != null)
                {
                    actionName = actionExpression?.FoundExpression?.AsRelation.Name;
                }
            }

#if DEBUG
            Log($"!!!!!!!! :) actionName = {actionName}");
#endif

            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

#if DEBUG
            Log("NEXT");
#endif

            if(actionName == "go")
            {
                DispatchGo(actionName);
                return;
            }

#if DEBUG
            Log("Undefined action");
#endif
        }

        private void DispatchGo(string actionName)
        {
#if DEBUG
            Log($"actionName = {actionName}");
#endif

            var queryStr = "{: direction(go,?X) :}";

            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, EntityDictionary);
            var query = queryStorage.MainRuleInstance;

#if DEBUG
            {
                var debugStr = DebugHelperForRuleInstance.ToString(query);

                Log($"debugStr (for going) = {debugStr}");
            }
#endif
            var querySearchResultCGStorage = MainCGStorage.Search(queryStorage);

            var varNameOfDirection = "?X";
            var keyOfVarOfDirection = EntityDictionary.GetKey(varNameOfDirection);

#if DEBUG
            Log($"keyOfVarOfDirection = {keyOfVarOfDirection}");
#endif
            var targetValueOfDirection = querySearchResultCGStorage.GetResultOfVarAsVariant("?X");

#if DEBUG
            LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");
#endif

            var targetValueOfDirection_1 = querySearchResultCGStorage.GetResultOfVar(keyOfVarOfDirection);

#if DEBUG
            LogInstance.Log($"targetValueOfDirection_1 = {targetValueOfDirection_1}");
#endif
            var entityConditionRuleInstance = targetValueOfDirection_1.GetEntityConditionRuleInstance();

#if DEBUG
            LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");
#endif
            var oldEntityConditionQueryString = RuleInstanceToOldEntityConditionConvertor.ConvertToOldQueryString(entityConditionRuleInstance);

#if DEBUG
            LogInstance.Log($"oldEntityConditionQueryString = {oldEntityConditionQueryString}");
#endif

#if DEBUG
            Log($"oldEntityConditionQueryString = {oldEntityConditionQueryString}");
#endif

            //var targetObject = GetLogicalObject(oldEntityConditionQueryString);
            var targetObject = GetLogicalObject(targetValueOfDirection);
            var targetPosition = targetObject.GetValue<System.Numerics.Vector3?>("global position");

#if DEBUG
            Log($"targetPosition = {targetPosition}");
#endif

            var command = TSTGoToPointNPCProcess.CreateCommand(targetPosition.Value);

#if DEBUG
            Log($"command = {command}");
#endif

            Send(command);
        }
    }
}
