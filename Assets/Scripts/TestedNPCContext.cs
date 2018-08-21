using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LegacyConvertors;
using MyNPCLib.Logical;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.LogicalSoundModeling;
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
                var queryPackage = CreateAnnotatedQueryForGoToGreenWaypoint(globalEntityDictionary);
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

        private static RuleInstancePackage CreateAnnotatedQueryForGoToGreenWaypoint(IEntityDictionary globalEntityDictionary)
        {
            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var annotationInstance = new RuleInstance();
            annotationInstance.Kind = KindOfRuleInstance.Annotation;
            var name = NamesHelper.CreateEntityName();
            annotationInstance.Name = name;
            annotationInstance.Key = globalEntityDictionary.GetKey(name);

            allRuleInstancesList.Add(annotationInstance);

            var partOfAnnotation = new RulePart();
            partOfAnnotation.IsActive = true;
            partOfAnnotation.Parent = annotationInstance;
            annotationInstance.Part_1 = partOfAnnotation;

            var relation = new RelationExpressionNode();
            partOfAnnotation.Expression = relation;
            name = "action";
            relation.Name = name;
            relation.Key = globalEntityDictionary.GetKey(name);
            relation.Params = new List<BaseExpressionNode>();

            var param = new VarExpressionNode();
            relation.Params.Add(param);
            var varName = "@X";
            param.Name = varName;
            param.Key = globalEntityDictionary.GetKey(varName);
            param.Quantifier = KindOfQuantifier.Existential;

            var variablesQuantification = new VariablesQuantificationPart();
            annotationInstance.VariablesQuantification = variablesQuantification;
            variablesQuantification.Items = new List<VarExpressionNode>();

            var varQuant_1 = new VarExpressionNode();
            varQuant_1.Quantifier = KindOfQuantifier.Existential;
            varQuant_1.Name = varName;
            varQuant_1.Key = globalEntityDictionary.GetKey(varName);
            variablesQuantification.Items.Add(varQuant_1);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            result.MainRuleInstance = ruleInstance;
            allRuleInstancesList.Add(ruleInstance);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            var annotation = new LogicalAnnotation();
            expr3.Annotations.Add(annotation);
            annotation.RuleInstanceKey = annotationInstance.Key;

            var relationName = "?Z";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.IsQuestion = true;
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "?X";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "?Y";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //a(?X,?Y)

            return result;
        }

        private void DispatchGo(string actionName, ContextOfCGStorage context)
        {
#if DEBUG
            Log($"actionName = {actionName}");
#endif
            var globalEntityDictionary = mEntityDictionary;

            var query = CreateQueryForDirectionOfGoing(globalEntityDictionary, actionName);

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

        private static RuleInstance CreateQueryForDirectionOfGoing(IEntityDictionary globalEntityDictionary, string actionName)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            var annotation = new LogicalAnnotation();

            var relationName = "direction";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new ConceptExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = actionName;
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "?X";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            return ruleInstance;
        }
    }
}
