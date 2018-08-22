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
        public TestedNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCHostContext npcHostContext, QueriesCache queriesCache)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext, queriesCache)
        {
            mEntityDictionary = entityDictionary;

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

        private IEntityDictionary mEntityDictionary;

        public override void Bootstrap()
        {
            Bootstrap<TestedBootNPCProcess>();
        }

        protected override void OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            var globalEntityDictionary = mEntityDictionary;

            var context = new ContextOfCGStorage(globalEntityDictionary);
            context.Init();

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

                var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

                context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
            }

            var actionName = string.Empty;

            {
                var queryStr = "{: ?Z(?X,?Y)[: {: action :} :] :}";

                var queryPackage = RuleInstanceFactory.ConvertStringToRuleInstancePackage(queryStr, globalEntityDictionary);
                //var queryPackage = CreateAnnotatedQueryForGoToGreenWaypoint(globalEntityDictionary);
                var queryPassiveListStorage = new PassiveListGCStorage(context, queryPackage.AllRuleInstances);
                var query = queryPackage.MainRuleInstance;

#if DEBUG
                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    Log($"debugStr = {debugStr}");
                }
#endif

                var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

                var searcher = new LogicalSearcher(context);

                var searchOptions = new LogicalSearchOptions();
                var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
                globalStorageOptions.Storage = context.GlobalCGStorage;
                globalStorageOptions.MaxDeph = null;
                globalStorageOptions.UseFacts = true;
                globalStorageOptions.UseProductions = true;
                globalStorageOptions.Priority = 1;

                searchOptions.DataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };

                searchOptions.QueryExpression = indexedQuery;

                var rearchResult = searcher.Run(searchOptions);

#if DEBUG
                Log($"rearchResult = {rearchResult}");
#endif

                var targetSearchResultItemsList = rearchResult.Items;

#if DEBUG
                Log($"targetSearchResultItemsList.Count = {targetSearchResultItemsList.Count}");
#endif

                var keyOfActionQuestionVar = globalEntityDictionary.GetKey("?Z");

#if DEBUG
                Log($"keyOfActionQuestionVar = {keyOfActionQuestionVar}");
#endif

                foreach (var targetSearchResultItem in targetSearchResultItemsList)
                {
#if DEBUG
                    Log($"targetSearchResultItem = {targetSearchResultItem}");
#endif

                    var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

#if DEBUG
                    //Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                    {
                        var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                        Log($"debugStr (zzz) = {debugStr}");
                    }
#endif

                    var actionExpression = targetSearchResultItem.GetResultOfVar(keyOfActionQuestionVar);

#if DEBUG
                    Log($"actionExpression = {actionExpression}");
#endif

                    if (actionExpression != null)
                    {
                        actionName = actionExpression?.FoundExpression?.AsRelation.Name;
                    }
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
                DispatchGo(actionName, context);
                return;
            }

#if DEBUG
            Log("Undefined action");
#endif
        }

        private void DispatchGo(string actionName, ContextOfCGStorage context)
        {
#if DEBUG
            Log($"actionName = {actionName}");
#endif
            var globalEntityDictionary = mEntityDictionary;

            var queryStr = "{: direction(go,?X) :}";

            var queryPackage = RuleInstanceFactory.ConvertStringToRuleInstancePackage(queryStr, globalEntityDictionary);
            //var queryPackage = CreateQueryForDirectionOfGoing(globalEntityDictionary, actionName);
            var queryPackagePassiveListStorage = new PassiveListGCStorage(context, queryPackage.AllRuleInstances);
            var query = queryPackage.MainRuleInstance;

#if DEBUG
            {
                var debugStr = DebugHelperForRuleInstance.ToString(query);

                Log($"debugStr (for going) = {debugStr}");
            }
#endif

            var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

            var searcher = new LogicalSearcher(context);

            var searchOptions = new LogicalSearchOptions();
            var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
            globalStorageOptions.Storage = context.GlobalCGStorage;
            globalStorageOptions.MaxDeph = null;
            globalStorageOptions.UseFacts = true;
            globalStorageOptions.UseProductions = true;
            globalStorageOptions.Priority = 1;

            searchOptions.DataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };

            searchOptions.QueryExpression = indexedQuery;

            var rearchResult = searcher.Run(searchOptions);

#if DEBUG
            Log($"rearchResult = {rearchResult}");
#endif

            var targetSearchResultItemsList = rearchResult.Items;

#if DEBUG
            Log($"targetSearchResultItemsList.Count = {targetSearchResultItemsList.Count}");
#endif

            var varNameOfDirection = "?X";
            var keyOfVarOfDirection = globalEntityDictionary.GetKey(varNameOfDirection);

#if DEBUG
            Log($"keyOfVarOfDirection = {keyOfVarOfDirection}");
#endif

            var oldEntityConditionQueryString = string.Empty;

            foreach (var targetSearchResultItem in targetSearchResultItemsList)
            {
#if DEBUG
                Log($"targetSearchResultItem = {targetSearchResultItem}");
#endif

                var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

#if DEBUG
                //Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                {
                    var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                    Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
                }
#endif

                var targetValueOfDirection = targetSearchResultItem.GetResultOfVar(keyOfVarOfDirection);

#if DEBUG
                Log($"targetValueOfDirection = {targetValueOfDirection}");
#endif

                var foundExpressionOfValueOfDirection = targetValueOfDirection.FoundExpression;

                if (foundExpressionOfValueOfDirection.IsEntityCondition)
                {
                    var foundExpressionOfValueOfDirectionAsEntityCondition = foundExpressionOfValueOfDirection.AsEntityCondition;

#if DEBUG
                    Log($"foundExpressionOfValueOfDirectionAsEntityCondition = {foundExpressionOfValueOfDirectionAsEntityCondition}");
#endif

                    var dbgContentString = context.GlobalCGStorage.GetContentAsDbgStr();

#if DEBUG
                    Log($"dbgContentString = {dbgContentString}");
#endif

                    var entityConditionRec = completeFoundRuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == foundExpressionOfValueOfDirectionAsEntityCondition.Key);

#if DEBUG
                    Log($"entityConditionRec = {entityConditionRec}");
#endif

                    var keyOfEntityConditionFact = entityConditionRec.Key;

#if DEBUG
                    Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");
#endif

                    var entityConditionRuleInstance = context.GlobalCGStorage.GetRuleInstanceByKey(keyOfEntityConditionFact);

#if DEBUG
                    Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");
#endif

                    oldEntityConditionQueryString = RuleInstanceToOldEntityConditionConvertor.ConvertToOldQueryString(entityConditionRuleInstance);              
                }
            }

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
