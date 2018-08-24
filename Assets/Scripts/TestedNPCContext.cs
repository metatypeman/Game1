﻿using MyNPCLib;
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
        public TestedNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext, QueriesCache queriesCache)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext, queriesCache)
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

            var ruleInstancesList = logicalSoundPackage.SoundFactsDataSource.AllRuleInstances;

#if DEBUG
            Log($"ruleInstancesList.Count = {ruleInstancesList.Count}");
#endif

            foreach (var ruleInstance in ruleInstancesList)
            {
#if DEBUG
                {
                    var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

                    Log($"debugStr = {debugStr}");
                }
#endif

                GlobalCGStorage.AddRuleInstance(ruleInstance);
            }

            var actionName = string.Empty;

            {
                var queryStr = "{: ?Z(?X,?Y)[: {: action :} :] :}";

                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, ContextOfCGStorage);
                var query = queryStorage.MainRuleInstance;

#if DEBUG
                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    Log($"debugStr = {debugStr}");
                }
#endif

                var searcher = new LogicalSearcher(ContextOfCGStorage);

                var searchOptions = new LogicalSearchOptions();
                searchOptions.DataSource = MainCGStorage;
                searchOptions.QuerySource = queryStorage;

                var rearchResult = searcher.Run(searchOptions);

#if DEBUG
                Log($"rearchResult = {rearchResult}");
#endif

                var querySearchResultCGStorage = new QueryResultCGStorage(ContextOfCGStorage, rearchResult);
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

            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, ContextOfCGStorage);
            var query = queryStorage.MainRuleInstance;

#if DEBUG
            {
                var debugStr = DebugHelperForRuleInstance.ToString(query);

                Log($"debugStr (for going) = {debugStr}");
            }
#endif

            var searcher = new LogicalSearcher(ContextOfCGStorage);

            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = MainCGStorage;
            searchOptions.QuerySource = queryStorage;

            var rearchResult = searcher.Run(searchOptions);

#if DEBUG
            Log($"rearchResult = {rearchResult}");
#endif

            var querySearchResultCGStorage = new QueryResultCGStorage(ContextOfCGStorage, rearchResult);

            var varNameOfDirection = "?X";
            var keyOfVarOfDirection = EntityDictionary.GetKey(varNameOfDirection);

#if DEBUG
            Log($"keyOfVarOfDirection = {keyOfVarOfDirection}");
#endif

            var targetValueOfDirection = querySearchResultCGStorage.GetResultOfVar(keyOfVarOfDirection);

#if DEBUG
            LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");
#endif
            var entityConditionRuleInstance = targetValueOfDirection.GetEntityConditionRuleInstance();

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

            var targetObject = GetLogicalObject(oldEntityConditionQueryString);
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
