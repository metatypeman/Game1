using MyNPCLib;
using MyNPCLib.CG;
using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingCGToInternal;
using MyNPCLib.ConvertingInternalCGToPersistLogicalData;
using MyNPCLib.ConvertingPersistLogicalData;
using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.Dot;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.NLToCGParsing;
using MyNPCLib.Parser.LogicalExpression;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using MyNPCLib.SimpleWordsDict;
using OpenNLP.Tools.PosTagger;
using SharpWordNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TmpSandBox.NPCBehaviour;
using TmpSandBox.TSTConceptualGraphs;

namespace TmpSandBox
{
    class Program
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"CurrentDomain_UnhandledException e.ExceptionObject = {e.ExceptionObject}");
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; 

            var logProxy = new LogProxyForNLog();
            LogInstance.SetLogProxy(logProxy);

            TSTQueryWithParams();
            //TSTHostLogicalObjectStorage();
            //TSTParsingUserQuery();
            //TSTLogicalSoundBus();
            //TSTQueryWithAccessPolicy();
            //TSTQueryEntityCondition();
            //TSTGoToGreenWaypoint();
            //TSTProcessAnnotations();
            //TSTATNParsing();
            //TSTWordNet();
            //TSTTextCGParserRunner();
            //TSTParseNLText();
            //TSTRuleInstance();
            //TSTEntityLogging();
            //TSTConceptualGraph_2();
            //TSTConceptualGraphs();
            //TSTRange();
            //TSTLexer();
            //TSTLogicalAST();
            //TSTCancelTask_2();
            //TSTCancelTask();
            //TSTMyNPCContext();
            //TSTStorageOfNPCProcesses();
            //TSTActivatorOfNPCProcessEntryPointInfo();
            //CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
        }

        private static void TSTQueryWithParams()
        {
            var globalEntityDictionary = new EntityDictionary();
            //var queryStr = "{: !:{public} {color(#12345, @$x1)} :}";
            var queryStr = "{: color = @$x1 :}";
            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary, new QueryParam("@$x1", true));
            var mainRuleInstance = queryStorage.MainRuleInstance;

            var debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr = {debugStr}");
        }

        private static void TSTHostLogicalObjectStorage()
        {
            var globalEntityDictionary = new EntityDictionary();
            var storage = new HostLogicalObjectStorage(globalEntityDictionary);

            var busOfCGStorages = new BusOfCGStorages(globalEntityDictionary);
            busOfCGStorages.AddStorage(storage);

            var queryStr = "{: !:{public} {color(#12345, 0.5l)} :}";
            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            storage.Append(queryStorage);

            queryStr = "{: !:{visible} {direction(#12345, 0.2l)} :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            storage.Append(queryStorage);

            var mainPublicStorage = busOfCGStorages.GeneralStorageWithPublicFacts;
            var visibleStorage = busOfCGStorages.GetStorageWithVisibleFacts(storage.EntityId);

            queryStr = "{: color(#12345, ?x) :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            var searchResult = mainPublicStorage.Search(queryStorage);

            var result = searchResult.GetResultOfVarAsVariant("?x");
            LogInstance.Log($"result = {result}");

            storage["alive"] = true;

            var propertyValue = storage["alive"];
            LogInstance.Log($"propertyValue = {propertyValue}");

            //searchResult = visibleStorage.Search(queryStorage);
            //result = searchResult.GetResultOfVarAsVariant("?x");
            //LogInstance.Log($"result (2)= {result}");
        }

        private static void TSTParsingUserQuery()
        {
            var globalEntityDictionary = new EntityDictionary();

            //var queryStr = "{: know(I, {:class=dog&determiner=the:})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, dog) & !male(dog) | son(dog, cat) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: (know(I, dog) & !male(dog)) | son(dog, cat)  :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, dog) & (!male(dog) | son(dog, cat)) & cat(dog) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, dog) & !!male(dog) | son(dog, cat) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: !know(I, dog) & male(dog) | son(dog, cat) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{:class=dog&determiner=the:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{:class=dog&determiner=true :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            var queryStr = "{:class=dog&determiner=0.5l :}";
            NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: class = dog & determiner = {:time=past & location=forest :} :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: class = dog & determiner = {:time=past & location=forest :}[:{:state:}:] :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: !(class=dog & determiner=the) | !male=neuter :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(male(dog), son(dog, cat)) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(@x = @y = male(dog), son(dog, cat)) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(@x[:{:info_1:}:] = @y = male(dog), son(dog, cat)) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(@x = @y = male(dog)[:{:info_1:}:], son(dog, cat)) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: @x = know(I, dog) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, @$a) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: @x = know(I, {:class=dog&determiner=the:})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: know(I, {:{class=dog&determiner=the}:})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {know(I, {:class=dog&determiner=the:})[:{:class=state:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {know(I, {:{class=dog&determiner=the}:})[:{:class=state:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: know(I, {:class=dog&determiner=the:})[:{:state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {know(I, {:class=dog&determiner=the:})[:{:state:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {know(I, @x1 = {:class=dog&determiner=the:})[:{:state:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {@x[:{:info_0:}:] = know(I[:{:info_1:}:], @x1[:{:info_2:}:] = {:class[:{:info_3:}:]=dog[:{:`info 1`:}:]&determiner=the:})[:{:state:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: {know(I, @x1 = {:class=dog&determiner=the:})[:{:state:}, {:action:}:]}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: ?x(?y[:{:info_1:}:], ?z):}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: ?x(*, *):}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: { ?x(?y, ?z) }:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: { ?x(*, *) }:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{:class=dog & determiner=the:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{:{class = dog & determiner = the[:{:class=state:}:] }:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: {class=dog & determiner=the} :}[:{:state:}, {:action:}:]}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{:{son(@x, @y)} -> {male(@x)&parent(@y, @x)}:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: @x = (know(I, {:class=dog&determiner=the:})[:{:class=state:}:] & !son(a)) & male(tree):}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: (know(I, {:class=dog&determiner=the:})[:{:class=state:}:] & !@x = son(a)) & male(tree):}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: (know(I, {:class=dog&determiner=the:})[:{:class=state:}:] & !@x = son(a)) & male(tree):}[:{:class=state:}:]";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: @x[:{:info_0:}:] = know(I, {:(class=dog&determiner=the) | !determiner=the:})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: know(I, {:(class=dog&determiner=the) | !(determiner=the|class=dog):})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: know(I, {:class=dog&determiner=the | !(determiner=the&class=dog):})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //queryStr = "{: know(I, {:!class=dog&determiner=the | !(determiner=the&class=dog):})[:{:class=state:}:]:}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: ?x(?y, ?z):}[:{:info_1:}:]";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, true) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, false) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);

            //var queryStr = "{: know(I, 0.5l) :}";
            //NTSTParsingUserQuery(queryStr, globalEntityDictionary);
        }

        /*
        //I do not forget about these cases.
        //But I will not make it just now.
        queryStr = "query { ?x(*, *) }";
        queryStr = "rule {{son(@x, @y)} -> {male(@x)&parent(@y, @x)}}";
        queryStr = "fact { know(I, {:class=dog&determiner=the:})[:{:state:}:]}";
        */

        private static void NTSTParsingUserQuery(string queryStr, IEntityDictionary entityDictionary)
        {
            LogInstance.Log($"queryStr = {queryStr}");

            var context = new ContextOfCGStorage(entityDictionary);
            
            var passiveListStorage = RuleInstanceFactory.ConvertStringToPassiveListGCStorage(queryStr, entityDictionary);

            var ruleInstancesList = passiveListStorage.AllRuleInstances;

            LogInstance.Log($"ruleInstancesList.Count = {ruleInstancesList.Count}");

            foreach(var ruleInstance in ruleInstancesList)
            {
                LogInstance.Log($"ruleInstance = {ruleInstance}");

                var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

                LogInstance.Log($"queryStr = {queryStr}");
                LogInstance.Log($"debugStr = {debugStr}");
            }

            LogInstance.Log("End");
        }

        private static void TSTLogicalSoundBus()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var soundBus = new LogicalSoundBus();

            var fakeListener = new TSTFakeLogicalSoundBusListener(new Vector3(200, 0, 0));
            soundBus.AddListener(fakeListener);

            var fakeListener_2 = new TSTFakeLogicalSoundBusListener(new Vector3(2000, 0, 0));
            soundBus.AddListener(fakeListener_2);

            var tstFact = CreateSimpleFact(globalEntityDictionary);

            var soundPackage = new InputLogicalSoundPackage(new Vector3(1, 0, 0), 60, new List<string>() { "human_speech" }, new PassiveListGCStorage(globalEntityDictionary, new List<RuleInstance>() { tstFact }));

            soundBus.PushSoundPackage(soundPackage);

            Thread.Sleep(10000);

            LogInstance.Log("End");
        }

        private static void TSTQueryWithAccessPolicy()
        {
            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var queryStr = "{: !:{public} {color(#12345, black)} :}";
            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            LogInstance.Log($"context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            context.GlobalCGStorage.Append(queryStorage);

            LogInstance.Log($"after context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            var mainRuleInstance = queryStorage.MainRuleInstance;

            var debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            queryStr = "{: !:{visible} {color(#12345, red)} :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            LogInstance.Log($"context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            context.GlobalCGStorage.Append(queryStorage);

            LogInstance.Log($"after context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            mainRuleInstance = queryStorage.MainRuleInstance;

            debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            queryStr = "{: !:{visible} {color(#12345, ?x)} :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            var globalDataSource = context.GlobalCGStorage;

            var searchOptions = new LogicalSearchOptions();
            searchOptions.IgnoreAccessPolicy = false;
            searchOptions.QuerySource = queryStorage;

            var querySearchResultCGStorage = globalDataSource.Search(searchOptions);

            var resultExpressionsList = querySearchResultCGStorage.GetResultsListOfVar("?X");

            LogInstance.Log($"resultExpressionsList.Count = {resultExpressionsList.Count}");
            foreach(var resultExpression in resultExpressionsList)
            {
                LogInstance.Log($"resultExpression = {resultExpression}");
            }
        }

        private static void TSTQueryEntityCondition()
        {
            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var queryStr = "{: !:{public, visible} {color(#12345, black)} :}";
            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            LogInstance.Log($"context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            context.GlobalCGStorage.Append(queryStorage);

            LogInstance.Log($"after context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            var mainRuleInstance = queryStorage.MainRuleInstance;

            var debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            queryStr = "{: !:public {color(dog, black)} :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            LogInstance.Log($"context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            context.GlobalCGStorage.Append(queryStorage);

            LogInstance.Log($"after context.GlobalCGStorage.AllRuleInstances.Count = {context.GlobalCGStorage.AllRuleInstances.Count}");

            mainRuleInstance = queryStorage.MainRuleInstance;

            debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            queryStr = "{: {color(?X, black)} :}";

            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            var globalDataSource = context.GlobalCGStorage;

            var querySearchResultCGStorage = globalDataSource.Search(queryStorage);

            var resultExpression = querySearchResultCGStorage.GetResultOfVar("?X");

            LogInstance.Log($"resultExpression = {resultExpression}");

            queryStr = "{: color=black :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

            mainRuleInstance = queryStorage.MainRuleInstance;

            debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            queryStorage = ConvertorEntityConditionToQuery.Convert(queryStorage);

            mainRuleInstance = queryStorage.MainRuleInstance;

            debugStr = DebugHelperForRuleInstance.ToString(mainRuleInstance);

            LogInstance.Log($"debugStr (query) = {debugStr}");

            var entitiesIdList = globalDataSource.GetEntitiesIdList(queryStorage);

            LogInstance.Log($"entitiesIdList.Count = {entitiesIdList.Count}");

            if(entitiesIdList.Count == 0)
            {
                return;
            }

            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            var firstEntityId = entitiesIdList.First();

            LogInstance.Log($"firstEntityId = {firstEntityId}");

            var propertyValue = globalDataSource.GetPropertyValueAsVariant(firstEntityId, "color");

            LogInstance.Log($"propertyValue = {propertyValue}");

            globalDataSource.SetPropertyValueAsAsObject(firstEntityId, "distance", new Vector3(1, 1, 1));

            var distanceValue = globalDataSource.GetPropertyValueAsObject(firstEntityId, "distance");

            LogInstance.Log($"distanceValue = {distanceValue}");

            globalDataSource.SetPropertyValueAsAsObject(firstEntityId, "color", "red");

            globalDataSource.SetPropertyValueAsAsObject(firstEntityId, "target", "#dog1");
        }

        private static void TSTGoToGreenWaypoint()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();
            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var paragraph = "Go to Green Waypoint";

            var wordsDict = new WordsDict();

            var cgParserOptions = new CGParserOptions();
            cgParserOptions.WordsDict = wordsDict;
            cgParserOptions.BasePath = AppDomain.CurrentDomain.BaseDirectory;

            var parser = new CGParser(cgParserOptions);

            var result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            var items = result.Items;

            foreach (var graph in items)
            {
                var dotStr = DotConverter.ConvertToString(graph);

                LogInstance.Log($"dotStr = {dotStr}");

                var internalCG = ConvertorCGToInternal.Convert(graph, globalEntityDictionary);

                //LogInstance.Log($"internalCG = {internalCG}");

                dotStr = DotConverter.ConvertToString(internalCG);

                LogInstance.Log($"dotStr (2) = {dotStr}");

                var ruleInstancesList = ConvertorInternalCGToPersistLogicalData.ConvertConceptualGraph(internalCG, globalEntityDictionary);

                var ruleInstancesPassiveListStorage = new PassiveListGCStorage(globalEntityDictionary, ruleInstancesList);

                LogInstance.Log($"ruleInstancesList.Count = {ruleInstancesList.Count}");
                //ruleInstancesList = ruleInstancesList.Take(1).ToList();
                foreach (var ruleInstance in ruleInstancesList)
                {
                    //LogInstance.Log($"ruleInstance = {ruleInstance}");

                    {
                        var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

                        LogInstance.Log($"debugStr = {debugStr}");
                    }

                    context.GlobalCGStorage.Append(ruleInstance);

                    //var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);
                    //indexedRuleInstance.FillIndexedDataAsStorage();

                    //LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");

                    //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
                }
            }

            var actionName = string.Empty;

            {
                var queryStr = "{: ?Z(?X,?Y)[: {: action :} :] :}";

                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);

                //var queryPackage = RuleInstanceFactory.ConvertStringToRuleInstancePackage(queryStr, globalEntityDictionary);

                //var queryStorage = new QueryCGStorage(context, queryPackage);

                //var queryPackage = CreateAnnotatedQueryForGoToGreenWaypoint(globalEntityDictionary);
                //var queryPackagePassiveListStorage = new PassiveListGCStorage(context, queryPackage.AllRuleInstances);
                //var query = queryPackage.MainRuleInstance;
                var query = queryStorage.MainRuleInstance;

                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    LogInstance.Log($"debugStr (query) = {debugStr}");
                }

                //var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

                var searcher = new LogicalSearcher(globalEntityDictionary);

                var searchOptions = new LogicalSearchOptions();
                //var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions.Storage = context.GlobalCGStorage;
                //globalStorageOptions.MaxDeph = null;
                //globalStorageOptions.UseFacts = true;
                //globalStorageOptions.UseAdditionalInstances = true;
                //globalStorageOptions.UseProductions = true;
                //globalStorageOptions.Priority = 1;

                //var globalStorageOptions_1 = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions_1.Storage = queryStorage;
                //globalStorageOptions_1.MaxDeph = null;
                //globalStorageOptions_1.UseFacts = false;
                //globalStorageOptions_1.UseAdditionalInstances = true;
                //globalStorageOptions_1.UseProductions = false;
                //globalStorageOptions_1.Priority = 1;

                //var dataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { context.GlobalCGStorage/*, globalStorageOptions_1*/ };

                //var consolidatedCGStorage = new ConsolidatedCGStorage(context, dataSourcesSettings);

                //searchOptions.DataSource = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };
                searchOptions.DataSource = context.GlobalCGStorage;

                searchOptions.QuerySource = queryStorage;

                var searchResult = searcher.Run(searchOptions);

                LogInstance.Log($"searchResult = {searchResult}");

                var querySearchResultCGStorage = new QueryResultCGStorage(globalEntityDictionary, searchResult);

                var keyOfActionQuestionVar = globalEntityDictionary.GetKey("?Z");

                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

                LogInstance.Log($"actionExpression = {actionExpression}");

                if (actionExpression != null)
                {
                    actionName = actionExpression?.FoundExpression?.AsRelation.Name;
                }

                //var targetSearchResultItemsList = searchResult.Items;

                //LogInstance.Log($"targetSearchResultItemsList.Count = {targetSearchResultItemsList.Count}");

                //LogInstance.Log($"keyOfActionQuestionVar = {keyOfActionQuestionVar}");

                //foreach (var targetSearchResultItem in targetSearchResultItemsList)
                //{
                //    LogInstance.Log($"targetSearchResultItem = {targetSearchResultItem}");

                //    var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

                //    //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                //    {
                //        var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                //        LogInstance.Log($"debugStr (zzz) = {debugStr}");
                //    }

                //    var actionExpression = targetSearchResultItem.GetResultOfVar(keyOfActionQuestionVar);

                //    LogInstance.Log($"actionExpression = {actionExpression}");

                //    if (actionExpression != null)
                //    {
                //        actionName = actionExpression?.FoundExpression?.AsRelation.Name;
                //    }
                //}

                //actionName = "go";
            }

            LogInstance.Log($"!!!!!!!! :) actionName = {actionName}");

            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            LogInstance.Log("NEXT");

            {
                var queryStr = "{: direction(go,?X) :}";

                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, globalEntityDictionary);
                //var queryPackage = RuleInstanceFactory.ConvertStringToRuleInstancePackage(queryStr, globalEntityDictionary);
                //var queryPackage = CreateQueryForDirectionOfGoing(globalEntityDictionary, actionName);
                //var queryStorage = new QueryCGStorage(context, queryPackage);
                //var queryPackagePassiveListStorage = new PassiveListGCStorage(context, queryPackage.AllRuleInstances);
                var query = queryStorage.MainRuleInstance;

                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    LogInstance.Log($"debugStr (for going) = {debugStr}");
                }

                //var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

                var searcher = new LogicalSearcher(globalEntityDictionary);

                var searchOptions = new LogicalSearchOptions();
                //var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions.Storage = context.GlobalCGStorage;
                //globalStorageOptions.MaxDeph = null;
                //globalStorageOptions.UseFacts = true;
                //globalStorageOptions.UseAdditionalInstances = true;
                //globalStorageOptions.UseProductions = true;
                //globalStorageOptions.Priority = 1;

                //var globalStorageOptions_1 = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions_1.Storage = queryStorage;
                //globalStorageOptions_1.MaxDeph = null;
                //globalStorageOptions_1.UseFacts = false;
                //globalStorageOptions_1.UseAdditionalInstances = true;
                //globalStorageOptions_1.UseProductions = false;
                //globalStorageOptions_1.Priority = 1;

                //var dataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions, globalStorageOptions_1 };
                //var consolidatedCGStorage = new ConsolidatedCGStorage(context, dataSourcesSettings);

                //searchOptions.DataSource = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };
                //searchOptions.DataSource = consolidatedCGStorage;
                searchOptions.DataSource = context.GlobalCGStorage;

                searchOptions.QuerySource = queryStorage;
                //searchOptions.QueryExpression = indexedQuery;

                var searchResult = searcher.Run(searchOptions);

                LogInstance.Log($"searchResult = {searchResult}");

                var varNameOfDirection = "?X";
                var keyOfVarOfDirection = globalEntityDictionary.GetKey(varNameOfDirection);

                LogInstance.Log($"keyOfVarOfDirection = {keyOfVarOfDirection}");

                var querySearchResultCGStorage = new QueryResultCGStorage(globalEntityDictionary, searchResult);

                var targetValueOfDirection = querySearchResultCGStorage.GetResultOfVar(keyOfVarOfDirection);

                LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");

                var entityConditionRuleInstance = targetValueOfDirection.GetEntityConditionRuleInstance();

                LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");

                //var oldEntityConditionQueryString = RuleInstanceToOldEntityConditionConvertor.ConvertToOldQueryString(entityConditionRuleInstance);

                //LogInstance.Log($"oldEntityConditionQueryString = {oldEntityConditionQueryString}");

                //var foundExpressionOfValueOfDirection = targetValueOfDirection.FoundExpression;

                //if (foundExpressionOfValueOfDirection.IsEntityCondition)
                //{
                //    //var foundExpressionOfValueOfDirectionAsEntityCondition = foundExpressionOfValueOfDirection.AsEntityCondition;

                //    //LogInstance.Log($"foundExpressionOfValueOfDirectionAsEntityCondition = {foundExpressionOfValueOfDirectionAsEntityCondition}");

                //    //var dbgContentString = context.GlobalCGStorage.GetContentAsDbgStr();

                //    //LogInstance.Log($"dbgContentString = {dbgContentString}");

                //    //var entityConditionRec = targetValueOfDirection.Parent.RuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == foundExpressionOfValueOfDirectionAsEntityCondition.Key);

                //    //var entityConditionRec = completeFoundRuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == foundExpressionOfValueOfDirectionAsEntityCondition.Key);

                //    //LogInstance.Log($"entityConditionRec = {entityConditionRec}");

                //    //var keyOfEntityConditionFact = entityConditionRec.Key;

                //    //LogInstance.Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");

                //    //var entityConditionRuleInstance = context.GlobalCGStorage.GetRuleInstanceByKey(keyOfEntityConditionFact);
                //    var entityConditionRuleInstance = targetValueOfDirection.GetEntityConditionRuleInstance();

                //    LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");

                //    var oldEntityConditionQueryString = RuleInstanceToOldEntityConditionConvertor.ConvertToOldQueryString(entityConditionRuleInstance);

                //    LogInstance.Log($"oldEntityConditionQueryString = {oldEntityConditionQueryString}");
                //}

                //var targetSearchResultItemsList = searchResult.Items;

                //LogInstance.Log($"targetSearchResultItemsList.Count = {targetSearchResultItemsList.Count}");

                //LogInstance.Log($"keyOfVarOfDirection = {keyOfVarOfDirection}");

                //foreach (var targetSearchResultItem in targetSearchResultItemsList)
                //{
                //    LogInstance.Log($"targetSearchResultItem = {targetSearchResultItem}");

                //    var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

                //    //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                //    {
                //        var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                //        LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
                //    }

                //    var targetValueOfDirection = targetSearchResultItem.GetResultOfVar(keyOfVarOfDirection);

                //    LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");

                //    var foundExpressionOfValueOfDirection = targetValueOfDirection.FoundExpression;

                //    if(foundExpressionOfValueOfDirection.IsEntityCondition)
                //    {
                //        var foundExpressionOfValueOfDirectionAsEntityCondition = foundExpressionOfValueOfDirection.AsEntityCondition;

                //        LogInstance.Log($"foundExpressionOfValueOfDirectionAsEntityCondition = {foundExpressionOfValueOfDirectionAsEntityCondition}");

                //        var dbgContentString = context.GlobalCGStorage.GetContentAsDbgStr();

                //        LogInstance.Log($"dbgContentString = {dbgContentString}");

                //        var entityConditionRec = completeFoundRuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == foundExpressionOfValueOfDirectionAsEntityCondition.Key);

                //        LogInstance.Log($"entityConditionRec = {entityConditionRec}");

                //        var keyOfEntityConditionFact = entityConditionRec.Key;

                //        LogInstance.Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");

                //        var entityConditionRuleInstance = context.GlobalCGStorage.GetRuleInstanceByKey(keyOfEntityConditionFact);

                //        LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");

                //        var oldEntityConditionQueryString = RuleInstanceToOldEntityConditionConvertor.ConvertToOldQueryString(entityConditionRuleInstance);

                //        LogInstance.Log($"oldEntityConditionQueryString = {oldEntityConditionQueryString}");
                //    }
                //}
            }

            LogInstance.Log("End");
        }

        private static RuleInstancePackage CreateQueryForDirectionOfGoing(IEntityDictionary globalEntityDictionary, string actionName)
        {
            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

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

            return result;
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

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

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

        private static void TSTProcessAnnotations()
        {
            LogInstance.Log("Begin");
            var globalEntityDictionary = new EntityDictionary();

            LogInstance.Log($"globalEntityDictionary.Name = {globalEntityDictionary.Name}");

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var smokeFact = CreateSimpleFact(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            smokeFact = CreateSimpleRule(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            smokeFact = CreateSimpleFact_2(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            smokeFact = CreateSimpleFact_2_2(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            smokeFact = CreateSimpleFact_3(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            smokeFact = CreateSimpleFact_3_2(globalEntityDictionary);
            AddSmokeFact(smokeFact, context.GlobalCGStorage);

            var annotatedFactsPackage = CreateAnnotaredFact(globalEntityDictionary);

            var annotatedFactList = annotatedFactsPackage.AllRuleInstances;
            LogInstance.Log($"annotatedFactList.Count = {annotatedFactList.Count}");

            var annotatedFactPassiveListStorage = new PassiveListGCStorage(globalEntityDictionary, annotatedFactList);

            foreach (var ruleInstance in annotatedFactList)
            {
                var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

                LogInstance.Log($"debugStr = {debugStr}");

                context.GlobalCGStorage.Append(ruleInstance);

                //var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);
                //indexedRuleInstance.FillIndexedDataAsStorage();
                //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
            }

            {
                var queryPackage = CreateAnnotatedQuery(globalEntityDictionary);

                //var queryPassiveListStorage = new PassiveListGCStorage(context, queryPackage.AllRuleInstances);
                var queryStorage = new QueryCGStorage(globalEntityDictionary, queryPackage);

                var query = queryPackage.MainRuleInstance;

                {
                    var debugStr = DebugHelperForRuleInstance.ToString(query);

                    LogInstance.Log($"debugStr = {debugStr}");
                }

                //var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);
                //indexedQuery.FillIndexedDataAsStorage();

                var searcher = new LogicalSearcher(globalEntityDictionary);

                var searchOptions = new LogicalSearchOptions();
                //var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions.Storage = context.GlobalCGStorage;
                //globalStorageOptions.MaxDeph = null;
                //globalStorageOptions.UseFacts = true;
                //globalStorageOptions.UseAdditionalInstances = true;
                //globalStorageOptions.UseProductions = true;
                //globalStorageOptions.Priority = 1;

                //var globalStorageOptions_1 = new SettingsOfStorageForSearchingInThisSession();
                //globalStorageOptions_1.Storage = queryStorage;
                //globalStorageOptions_1.MaxDeph = null;
                //globalStorageOptions_1.UseFacts = false;
                //globalStorageOptions_1.UseAdditionalInstances = true;
                //globalStorageOptions_1.UseProductions = false;
                //globalStorageOptions_1.Priority = 1;

                //var dataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions, globalStorageOptions_1 };
                //var consolidatedCGStorage = new ConsolidatedCGStorage(context, dataSourcesSettings);

                //searchOptions.DataSource = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };
                //searchOptions.DataSource = consolidatedCGStorage;
                searchOptions.DataSource = context.GlobalCGStorage;

                //searchOptions.QueryExpression = indexedQuery;
                searchOptions.QuerySource = queryStorage;

                var searchResult = searcher.Run(searchOptions);

                LogInstance.Log($"searchResult = {searchResult}");

                var targetSearchResultItemsList = searchResult.Items;

                foreach (var targetSearchResultItem in targetSearchResultItemsList)
                {
                    var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

                    //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                    {
                        var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                        LogInstance.Log($"debugStr (zzz) = {debugStr}");
                    }
                }
            }

            LogInstance.Log("End");
        }

        private static void AddSmokeFact(RuleInstance smokeFact, BaseRealStorage storage)
        {
            storage.Append(smokeFact);

            var indexedSmokeFact = ConvertorToIndexed.ConvertRuleInstance(smokeFact);
            //indexedSmokeFact.FillIndexedDataAsStorage();
            //storage.NSetIndexedRuleInstanceToIndexData(indexedSmokeFact);
            var debugStr = DebugHelperForRuleInstance.ToString(smokeFact);
            LogInstance.Log($"debugStr = {debugStr}");
        }

        private static RuleInstancePackage CreateAnnotatedQuery(IEntityDictionary globalEntityDictionary)
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
            name = "state";
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

            return result;
        }

        private static RuleInstancePackage CreateAnnotaredFact(IEntityDictionary globalEntityDictionary)
        {
            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var annotationInstance = new RuleInstance();
            //result.Add(annotationInstance);
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
            name = "state";
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

            var entityConditionInstance = new RuleInstance();
            allRuleInstancesList.Add(entityConditionInstance);

            entityConditionInstance.Kind = KindOfRuleInstance.EntityCondition;
            name = NamesHelper.CreateEntityName();
            entityConditionInstance.Name = name;
            entityConditionInstance.Key = globalEntityDictionary.GetKey(name);

            var partOfEntityCondition = new RulePart();
            partOfEntityCondition.Parent = entityConditionInstance;
            entityConditionInstance.Part_1 = partOfEntityCondition;
            partOfEntityCondition.IsActive = true;

            relation = new RelationExpressionNode();
            partOfEntityCondition.Expression = relation;
            name = "determiner";
            relation.Name = name;
            relation.Key = globalEntityDictionary.GetKey(name);
            relation.Params = new List<BaseExpressionNode>();

            var concept = new ConceptExpressionNode();
            name = "dog";
            concept.Name = name;
            concept.Key = globalEntityDictionary.GetKey(name);
            relation.Params.Add(concept);

            concept = new ConceptExpressionNode();
            name = "the";
            concept.Name = name;
            concept.Key = globalEntityDictionary.GetKey(name);
            relation.Params.Add(concept);

            var ruleInstance = new RuleInstance();
            result.MainRuleInstance = ruleInstance;
            allRuleInstancesList.Add(ruleInstance);

            ruleInstance.Kind = KindOfRuleInstance.Fact;
            name = NamesHelper.CreateEntityName();
            ruleInstance.Name = name;
            ruleInstance.Key = globalEntityDictionary.GetKey(name);

            var partOfRuleInstance = new RulePart();
            partOfRuleInstance.Parent = ruleInstance;
            ruleInstance.Part_1 = partOfRuleInstance;
            partOfRuleInstance.IsActive = true;

            relation = new RelationExpressionNode();
            partOfRuleInstance.Expression = relation;
            name = "know";
            relation.Name = name;
            relation.Key = globalEntityDictionary.GetKey(name);
            relation.Params = new List<BaseExpressionNode>();
            relation.Annotations = new List<LogicalAnnotation>();

            var annotation = new LogicalAnnotation();
            relation.Annotations.Add(annotation);
            annotation.RuleInstanceKey = annotationInstance.Key;

            concept = new ConceptExpressionNode();
            name = "I";
            concept.Name = name;
            concept.Key = globalEntityDictionary.GetKey(name);
            relation.Params.Add(concept);

            var entityConditionVarName = "#@R";
            var entityConditionVarKey = globalEntityDictionary.GetKey(entityConditionVarName);

            var entityCondition = new EntityConditionExpressionNode();
            relation.Params.Add(entityCondition);
            entityCondition.Name = entityConditionInstance.Name;
            entityCondition.Key = entityConditionInstance.Key;
            entityCondition.VariableName = entityConditionVarName;
            entityCondition.VariableKey = entityConditionVarKey;

            var entitiesConditions = new EntitiesConditions();
            ruleInstance.EntitiesConditions = entitiesConditions;
            entitiesConditions.Items = new List<EntityConditionItem>();

            var entityCondition_1 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_1);
            entityCondition_1.Name = entityConditionInstance.Name;
            entityCondition_1.Key = entityConditionInstance.Key;
            entityCondition_1.VariableName = entityConditionVarName;
            entityCondition_1.VariableKey = entityConditionVarKey;

            return result;
        }

        private static void TSTATNParsing()
        {
            LogInstance.Log("Begin");

            var text = "I know the dog.";
            NTSTATNParsing(text);

            //var text = "I'll skip";
            //NTSTATNParsing(text);
            //var text = "- Sorry Mrs Hudson, I'll skip the tea.";
            //NTSTATNParsing(text);

            //text = "Kill the dog!";
            //NTSTATNParsing(text);

            //text = "This is a green forest 12.";
            //NTSTATNParsing(text);

            LogInstance.Log("End");
        }

        private static void NTSTATNParsing(string text)
        {
            LogInstance.Log($"text = {text}");

            var wordsDict = new WordsDict();
            var extendedLexer = new ATNExtendedLexer(text, wordsDict);

            IList<ATNExtendedToken> сlusterOfExtendTokens = null;

            while((сlusterOfExtendTokens = extendedLexer.GetСlusterOfExtendedTokens()) != null)
            {
                LogInstance.Log($"сlusterOfExtendTokens.Count = {сlusterOfExtendTokens.Count}");
                foreach(var extendToken in сlusterOfExtendTokens)
                {
                    LogInstance.Log($"extendToken = {extendToken}");
                }
            }

            //var lexer = new ATNLexer(text);

            //ATNToken token = null;

            //while((token = lexer.GetToken()) != null)
            //{
            //    LogInstance.Log($"token = {token}");
            //}

            LogInstance.Log("End");
        }

        private static void TSTWordNet()
        {
            var modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/WordNet/dict/");

            LogInstance.Log($"modelsPath = {modelsPath}");

            var wordNetEngine = new DataFileEngine(modelsPath);

            var lemma = "liked";

            var partsOfSpeechesList = wordNetEngine.GetPartsOfSpeech(lemma);
            LogInstance.Log($"partsOfSpeechesList.Length = {partsOfSpeechesList.Length}");
            foreach(var partsOfSpeech in partsOfSpeechesList)
            {
                LogInstance.Log($"partsOfSpeech = {partsOfSpeech}");
                var indexWord = wordNetEngine.GetIndexWord(lemma, partsOfSpeech);
                LogInstance.Log($"indexWord.PartOfSpeech = {indexWord.PartOfSpeech} indexWord.SynsetOffsets.Length = {indexWord.SynsetOffsets.Length} indexWord.TagSenseCount = {indexWord.TagSenseCount} indexWord.SenseCount = {indexWord.SenseCount}");
                var baseFormsList = wordNetEngine.GetBaseForms(lemma, partsOfSpeech);
                LogInstance.Log($"baseFormsList.Length = {baseFormsList.Length}");
                foreach(var baseForm in baseFormsList)
                {
                    LogInstance.Log($"baseForm = {baseForm}");
                }
                LogInstance.Log($"indexWord.RelationTypes.Length = {indexWord.RelationTypes.Length}");
                foreach (var relationType in indexWord.RelationTypes)
                {
                    LogInstance.Log($"relationType = {relationType}");
                }
                var sinsetsList = wordNetEngine.GetSynsets(lemma, partsOfSpeech);
                foreach(var sinset in sinsetsList)
                {
                    LogInstance.Log($"sinset = {sinset}");
                }
                var relationTypesList = wordNetEngine.GetRelationTypes(lemma, partsOfSpeech);
                foreach (var relationType in relationTypesList)
                {
                    LogInstance.Log($"relationType.Name = {relationType.Name}");
                }
            }
            var externalSinsetsList = wordNetEngine.GetSynsets(lemma);
            foreach (var sinset in externalSinsetsList)
            {
                LogInstance.Log($"sinset = {sinset}");
            }
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Models/EnglishPOS.nbin");// "path/to/EnglishPOS.nbin";
            var tagDictDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Models/Parser/tagdict"); //"path/to/tagdict/directory";
            var posTagger = new EnglishMaximumEntropyPosTagger(modelPath, tagDictDir);
            var tokens = new string[]{ lemma };
            var pos = posTagger.Tag(tokens);
            foreach (var p in pos)
            {
                LogInstance.Log($"p = {p}");
            }
            LogInstance.Log("End");
        }

        private static object EnglishMaximumEntropyPosTagger(string modelPath, object tagdictDir)
        {
            throw new NotImplementedException();
        }

        private static void TSTTextCGParserRunner()
        {
            var textCGParserRunner = new TextCGParserRunner();
            textCGParserRunner.CreateATNStateTree();
        }

        private static void TSTParseNLText()
        {
            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            //var path = AppDomain.CurrentDomain.BaseDirectory;

            //LogInstance.Log($"path = {path}");

            //var relativePath = "Resources/Models/";

            //var modelPath = Path.Combine(path, relativePath);

            //LogInstance.Log($"modelPath = {modelPath}");

            //mParser = new EnglishTreebankParser(modelPath);

            //var sentence = "- Sorry Mrs. Hudson, I'll skip the tea. Kill the dog! This is a green forest.";

            //ParseSentence(sentence);

            //sentence = "Kill the dog!";

            //ParseSentence(sentence);

            //sentence = "This is a green forest.";

            //ParseSentence(sentence);

            //sentence = "The third story arc centers on the longstanding brotherhood charged with defending the realm against the ancient threats of the fierce peoples and legendary creatures that lie far north, and an impending winter that threatens the realm.";

            //ParseSentence(sentence);

            var wordsDict = new WordsDict();

            var cgParserOptions = new CGParserOptions();
            cgParserOptions.WordsDict = wordsDict;
            cgParserOptions.BasePath = AppDomain.CurrentDomain.BaseDirectory;

            var parser = new CGParser(cgParserOptions);

            //var paragraph = "Mr. & Mrs. Smith is a 2005 American romantic comedy action film. The film stars Brad Pitt and Angelina Jolie as a bored upper-middle class married couple. They are surprised to learn that they are both assassins hired by competing agencies to kill each other.";

            //var result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            //var paragraph = "- Sorry Mrs. Hudson, I'll skip the tea. Kill the dog! This is a green forest. Kill the dog!";

            //var result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            var paragraph = "I know the dog.";

            var result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            var items = result.Items;

            foreach(var graph in items)
            {
                var dotStr = DotConverter.ConvertToString(graph);

                LogInstance.Log($"dotStr = {dotStr}");

                var internalCG = ConvertorCGToInternal.Convert(graph, globalEntityDictionary);

                //LogInstance.Log($"internalCG = {internalCG}");

                dotStr = DotConverter.ConvertToString(internalCG);

                LogInstance.Log($"dotStr (2) = {dotStr}");

                var ruleInstancesList = ConvertorInternalCGToPersistLogicalData.ConvertConceptualGraph(internalCG, globalEntityDictionary);

                LogInstance.Log($"ruleInstancesList.Count = {ruleInstancesList.Count}");
                foreach(var ruleInstance in ruleInstancesList)
                {
                    //LogInstance.Log($"ruleInstance = {ruleInstance}");

                    {
                        var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

                        LogInstance.Log($"debugStr = {debugStr}");
                    }

                ////    var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);
                ////    //indexedRuleInstance.FillIndexedDataAsStorage();

                ////    LogInstance.Log($"indexedRuleInstance = {indexedRuleInstance}");

                ////    context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);
                }
            }

            //var contentOfStorageDbgStr = context.GlobalCGStorage.GetContentAsDbgStr();

            //LogInstance.Log($"contentOfStorageDbgStr = {contentOfStorageDbgStr}");

            ////return;

            //var query = CreateSimpleQueryForMySentence(globalEntityDictionary);

            //LogInstance.Log($"query = {query}");

            //{
            //    var debugStr = DebugHelperForRuleInstance.ToString(query);

            //    LogInstance.Log($"debugStr = {debugStr}");
            //}

            //var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);
            ////indexedQuery.FillIndexedDataAsStorage();

            //var searcher = new LogicalSearcher(context);

            //var searchOptions = new LogicalSearchOptions();
            //var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
            //globalStorageOptions.Storage = context.GlobalCGStorage;
            //globalStorageOptions.MaxDeph = null;
            //globalStorageOptions.UseFacts = true;
            //globalStorageOptions.UseAdditionalInstances = true;
            //globalStorageOptions.UseProductions = true;
            //globalStorageOptions.Priority = 1;

            //searchOptions.DataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };

            //searchOptions.QueryExpression = indexedQuery;

            //var searchResult = searcher.Run(searchOptions);

            //LogInstance.Log($"searchResult = {searchResult}");

            //var targetSearchResultItemsList = searchResult.Items;

            //foreach (var targetSearchResultItem in targetSearchResultItemsList)
            //{
            //    var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

            //    //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

            //    {
            //        var debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

            //        LogInstance.Log($"debugStr = {debugStr}");
            //    }
            //}

            //var paragraph = "The dog likes the man.";

            //var result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            //paragraph = "Does not the dog love man?";

            //result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            //paragraph = "We analyse what our clients may need, develop a new product, produce a sample, improve it and sell it.";
            //result = parser.Run(paragraph);
            //LogInstance.Log($"result = {result}");

            LogInstance.Log("End");
        }

        private static RuleInstance CreateSimpleQueryForMySentence(IEntityDictionary globalEntityDictionary)
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

            var relationName = "know";
            var relationKey = globalEntityDictionary.GetKey(relationName);
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

            //konw(?X,?Y)

            return ruleInstance;
        }

        //private static EnglishTreebankParser mParser;

        private static void ParseSentence(string sentence)
        {
            LogInstance.Log($"sentence = '{sentence}'");

            //var path = Directory.GetCurrentDirectory();

            //LogInstance.Log($"path = {path}");

            //var relativePath = "Resources/Models/";

            //var modelPath = Path.Combine(path, relativePath);

            //LogInstance.Log($"modelPath = {modelPath}");

            //var parser = new EnglishTreebankParser(modelPath);
            //var node = parser.DoParse(sentence);
            //var node = mParser.DoParse(sentence);
            //var dbgStr = OpenNLPParseNodeHelper.ToString(node);
            //LogInstance.Log($"dbgStr = {dbgStr}");

            LogInstance.Log("End");
        }

        private static void TSTRuleInstance()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            LogInstance.Log($"globalEntityDictionary.Name = {globalEntityDictionary.Name}");

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var commonPersistLogicalData = new CommonPersistLogicalData();
            commonPersistLogicalData.DictionaryName = globalEntityDictionary.Name;
            commonPersistLogicalData.RuleInstancesList = new List<RuleInstance>();

            var commonPersistIndexedLogicalData = new CommonPersistIndexedLogicalData();

            commonPersistIndexedLogicalData.IndexedRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();

            //var exampleRuleInstance = CreateFirstRuleInstance(globalEntityDictionary);

            //LogInstance.Log($"ruleInstance = {exampleRuleInstance}");

            //var debugStr = DebugHelperForRuleInstance.ToString(exampleRuleInstance);

            //LogInstance.Log($"debugStr = {debugStr}");

            //var indexedExampleRuleInstance = ConvertorToIndexed.ConvertRuleInstance(exampleRuleInstance);

            //LogInstance.Log($"indexedExampleRuleInstance = {indexedExampleRuleInstance}");

            //var ruleInstance = CreateFirstRuleInstance(globalEntityDictionary);
            var ruleInstance = CreateSimpleRule(globalEntityDictionary);

            //LogInstance.Log($"ruleInstance = {ruleInstance}");

            var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(ruleInstance);

            //var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);

            var factInstance = CreateSimpleFact(globalEntityDictionary);
            commonPersistLogicalData.RuleInstancesList.Add(factInstance);

            //LogInstance.Log($"factInstance = {factInstance}");

            debugStr = DebugHelperForRuleInstance.ToString(factInstance);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(factInstance);

            //var indexedFactInstance = ConvertorToIndexed.ConvertRuleInstance(factInstance);

            //commonPersistIndexedLogicalData.IndexedRuleInstancesDict[indexedFactInstance.Key] = indexedFactInstance;

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFactInstance);

            //LogInstance.Log($"indexedFactInstance = {indexedFactInstance}");

            var fact_2 = CreateSimpleFact_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_2);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(fact_2);

            //var indexedFact_2 = ConvertorToIndexed.ConvertRuleInstance(fact_2);

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_2);

            var fact_2_2 = CreateSimpleFact_2_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_2_2);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(fact_2_2);

            //var indexedFact_2_2 = ConvertorToIndexed.ConvertRuleInstance(fact_2_2);

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_2_2);

            var fact_3 = CreateSimpleFact_3(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_3);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(fact_3);

            //var indexedFact_3 = ConvertorToIndexed.ConvertRuleInstance(fact_3);

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_3);

            var fact_3_2 = CreateSimpleFact_3_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_3_2);

            LogInstance.Log($"debugStr = {debugStr}");

            context.GlobalCGStorage.Append(fact_3_2);

            //var indexedFact_3_2 = ConvertorToIndexed.ConvertRuleInstance(fact_3_2);

            //context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_3_2);

            var queryPackage = CreateSimpleQuery(globalEntityDictionary);
            var query = queryPackage.MainRuleInstance;
            //LogInstance.Log($"query = {query}");

            debugStr = DebugHelperForRuleInstance.ToString(query);

            var queryStorage = new QueryCGStorage(globalEntityDictionary, queryPackage);

            LogInstance.Log($"debugStr = {debugStr}");

            //var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

            var searcher = new LogicalSearcher(globalEntityDictionary);

            var searchOptions = new LogicalSearchOptions();
            //var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
            //globalStorageOptions.Storage = context.GlobalCGStorage;
            //globalStorageOptions.MaxDeph = null;
            //globalStorageOptions.UseFacts = true;
            //globalStorageOptions.UseProductions = true;
            //globalStorageOptions.UseAdditionalInstances = true;
            //globalStorageOptions.Priority = 1;

            //var globalStorageOptions_1 = new SettingsOfStorageForSearchingInThisSession();
            //globalStorageOptions_1.Storage = queryStorage;
            //globalStorageOptions_1.MaxDeph = null;
            //globalStorageOptions_1.UseFacts = false;
            //globalStorageOptions_1.UseAdditionalInstances = true;
            //globalStorageOptions_1.UseProductions = false;
            //globalStorageOptions_1.Priority = 1;

            //var dataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions, globalStorageOptions_1 };
            //var consolidatedCGStorage = new ConsolidatedCGStorage(context, dataSourcesSettings);

            ////searchOptions.DataSource = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };
            //searchOptions.DataSource = consolidatedCGStorage;
            searchOptions.DataSource = context.GlobalCGStorage;

            //searchOptions.QueryExpression = indexedQuery;
            searchOptions.QuerySource = queryStorage;

            var searchResult = searcher.Run(searchOptions);

            //LogInstance.Log($"searchResult = {searchResult}");

            var targetSearchResultItemsList = searchResult.Items;

            foreach(var targetSearchResultItem in targetSearchResultItemsList)
            {
                var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

                //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                LogInstance.Log($"debugStr = {debugStr}");
            }

            LogInstance.Log("End");
        }

        private static RuleInstance CreateSimpleRule(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Rule;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            var rulePart_2 = new RulePart();
            rulePart_2.Parent = ruleInstance;
            ruleInstance.Part_2 = rulePart_2;

            rulePart_1.NextPart = rulePart_2;
            rulePart_2.NextPart = rulePart_1;

            rulePart_1.IsActive = true;
            rulePart_2.IsActive = true;

            var expr_1_1 = new RelationExpressionNode();
            rulePart_1.Expression = expr_1_1;
            expr_1_1.Params = new List<BaseExpressionNode>();
            expr_1_1.Name = "son";
            expr_1_1.Key = globalEntityDictionary.GetKey(expr_1_1.Name);

            var param_1_1_1 = new VarExpressionNode();
            expr_1_1.Params.Add(param_1_1_1);
            param_1_1_1.Name = "@X";
            param_1_1_1.Key = globalEntityDictionary.GetKey(param_1_1_1.Name);

            var param_1_1_2 = new VarExpressionNode();
            expr_1_1.Params.Add(param_1_1_2);
            param_1_1_2.Name = "@Y";
            param_1_1_2.Key = globalEntityDictionary.GetKey(param_1_1_2.Name);

            var expr_2_1 = new OperatorAndExpressionNode();
            rulePart_2.Expression = expr_2_1;

            var expr_2_2 = new RelationExpressionNode();
            expr_2_1.Left = expr_2_2;
            expr_2_2.Params = new List<BaseExpressionNode>();
            expr_2_2.Name = "parent";
            expr_2_2.Key = globalEntityDictionary.GetKey(expr_2_2.Name);

            var param_2_2_1 = new VarExpressionNode();
            expr_2_2.Params.Add(param_2_2_1);
            param_2_2_1.Name = "@Y";
            param_2_2_1.Key = globalEntityDictionary.GetKey(param_2_2_1.Name);

            var param_2_2_2 = new VarExpressionNode();
            expr_2_2.Params.Add(param_2_2_2);
            param_2_2_2.Name = "@X";
            param_2_2_2.Key = globalEntityDictionary.GetKey(param_2_2_2.Name);

            var expr_2_3 = new RelationExpressionNode();
            expr_2_1.Right = expr_2_3;
            expr_2_3.Params = new List<BaseExpressionNode>();
            expr_2_3.Name = "male";
            expr_2_3.Key = globalEntityDictionary.GetKey(expr_2_3.Name);

            var param_2_3_1 = new VarExpressionNode();
            expr_2_3.Params.Add(param_2_3_1);
            param_2_3_1.Name = "@X";
            param_2_3_1.Key = globalEntityDictionary.GetKey(param_2_3_1.Name);

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
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

            var relationName = "son";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Piter";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Tom";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //son(#Piter,#Tom)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
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

            var relationName = "parent";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Tom";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#John";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //parent(#Tom, #John)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_2_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
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

            var relationName = "parent";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#George";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Bob";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //parent(#George, #Bob)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_3(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
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

            var relationName = "male";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#John";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            //male(#John)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_3_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
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

            var relationName = "male";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Bob";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            //male(#Bob)

            return ruleInstance;
        }

        private static RuleInstancePackage CreateSimpleQuery(IEntityDictionary globalEntityDictionary)
        {
            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

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

            var relationName = "son";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "?X";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Tom";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //son(Piter,$X1)

            return result;
        }

        private static RuleInstance CreateFirstRuleInstance(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Name = "#1";
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var belongToEntityExpression = new EntityRefExpressionNode();
            ruleInstance.BelongToEntity = belongToEntityExpression;
            belongToEntityExpression.Name = "cat";
            belongToEntityExpression.Key = globalEntityDictionary.GetKey(belongToEntityExpression.Name);

            var entitiesConditions = new EntitiesConditions();
            ruleInstance.EntitiesConditions = entitiesConditions;
            entitiesConditions.Items = new List<EntityConditionItem>();

            var entityCondition_1 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_1);
            entityCondition_1.Name = "#123";
            entityCondition_1.Key = globalEntityDictionary.GetKey(entityCondition_1.Name);
            entityCondition_1.VariableName = "#@R";
            entityCondition_1.VariableKey = globalEntityDictionary.GetKey(entityCondition_1.VariableName);

            var entityCondition_2 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_2);
            entityCondition_2.Name = "#124";
            entityCondition_2.Key = globalEntityDictionary.GetKey(entityCondition_2.Name);
            entityCondition_2.VariableName = "#@T";
            entityCondition_2.VariableKey = globalEntityDictionary.GetKey(entityCondition_2.VariableName);

            var variablesQuantification = new VariablesQuantificationPart();
            ruleInstance.VariablesQuantification = variablesQuantification;
            variablesQuantification.Items = new List<VarExpressionNode>();

            var varQuant_1 = new VarExpressionNode();
            varQuant_1.Quantifier = KindOfQuantifier.Universal;
            varQuant_1.Name = "@X";
            varQuant_1.Key = globalEntityDictionary.GetKey(varQuant_1.Name);
            variablesQuantification.Items.Add(varQuant_1);

            var varQuant_2 = new VarExpressionNode();
            varQuant_2.Quantifier = KindOfQuantifier.Universal;
            varQuant_2.Name = "@Z";
            varQuant_2.Key = globalEntityDictionary.GetKey(varQuant_2.Name);
            variablesQuantification.Items.Add(varQuant_2);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            var rulePart_2 = new RulePart();
            rulePart_2.Parent = ruleInstance;
            ruleInstance.Part_2 = rulePart_2;

            rulePart_1.NextPart = rulePart_2;
            rulePart_2.NextPart = rulePart_1;

            rulePart_1.IsActive = true;
            rulePart_2.IsActive = true;

            var variablesQuantification_1 = new VariablesQuantificationPart();
            rulePart_1.VariablesQuantification = variablesQuantification_1;
            variablesQuantification_1.Items = new List<VarExpressionNode>();

            var varQuant_3 = new VarExpressionNode();
            varQuant_3.Quantifier = KindOfQuantifier.Existential;
            varQuant_3.Name = "@X";
            varQuant_3.Key = globalEntityDictionary.GetKey(varQuant_3.Name);
            variablesQuantification_1.Items.Add(varQuant_3);

            var expr_1 = new OperatorNotExpressionNode();
            rulePart_1.Expression = expr_1;
            var expr_2 = new OperatorAndExpressionNode();
            expr_1.Left = expr_2;
            var expr3 = new RelationExpressionNode();
            expr_2.Left = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "isa";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#2";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new ConceptExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "dog";
            param_2.Key = globalEntityDictionary.GetKey(param_1.Name);

            var relation2 = new RelationExpressionNode();
            expr_2.Right = relation2;
            relation2.Params = new List<BaseExpressionNode>();
            relation2.Name = "smile";
            relation2.Key = globalEntityDictionary.GetKey(relation2.Name);

            var param_3 = new ConceptExpressionNode();
            relation2.Params.Add(param_3);
            param_3.Name = "tree";
            param_3.Key = globalEntityDictionary.GetKey(param_3.Name);

            var relationForPart_2 = new RelationExpressionNode();
            rulePart_2.Expression = relationForPart_2;
            relationForPart_2.Params = new List<BaseExpressionNode>();
            relationForPart_2.Name = "slow";
            relationForPart_2.Key = globalEntityDictionary.GetKey(relationForPart_2.Name);

            var param_4 = new ConceptExpressionNode();
            relationForPart_2.Params.Add(param_4);
            param_4.Name = "car";
            param_4.Key = globalEntityDictionary.GetKey(param_4.Name);

            var notContradictPart = new NotContradictPart();
            ruleInstance.NotContradict = notContradictPart;
            notContradictPart.Parent = ruleInstance;

            var study_hard_relation = new RelationExpressionNode();
            notContradictPart.Expression = study_hard_relation;
            study_hard_relation.Params = new List<BaseExpressionNode>();

            study_hard_relation.Name = "study_hard";
            study_hard_relation.Key = globalEntityDictionary.GetKey(study_hard_relation.Name);

            var param_5 = new VarExpressionNode();
            study_hard_relation.Params.Add(param_5);

            param_5.Name = "@X";
            param_5.Key = globalEntityDictionary.GetKey(param_5.Name);

            var cF = new CertaintyFactorFuzzyModality();
            ruleInstance.CertaintyFactor = cF;
            cF.Parent = ruleInstance;

            var cFExpression = new ValueExpressionNode();
            cF.Expression = cFExpression;
            cFExpression.Value = 0.5f;

            cF.Annotations = new List<LogicalAnnotation>();

            var annotationForCF_1 = new LogicalAnnotation();
            cF.Annotations.Add(annotationForCF_1);
            //annotationForCF_1.Name = "#annotation_1";
            //annotationForCF_1.Key = globalEntityDictionary.GetKey(annotationForCF_1.Name);

            var annotationForCF_2 = new LogicalAnnotation();
            cF.Annotations.Add(annotationForCF_2);
            //annotationForCF_2.Name = "#annotation_2";
            //annotationForCF_2.Key = globalEntityDictionary.GetKey(annotationForCF_2.Name);

            return ruleInstance;
        }

        [MethodForLoggingSupport]
        private static string GetClassFullName()
        {
            var className = string.Empty;
            Type declaringType = null;
            //var framesToSkip = 2;
            var framesToSkip = 0;

            while (true)
            {
                var frame = new StackFrame(framesToSkip, false);

                var method = frame.GetMethod();

                var attribute = method?.GetCustomAttribute<MethodForLoggingSupportAttribute>();

                declaringType = method?.DeclaringType;

                LogInstance.Log($"method?.Name = {method?.Name} framesToSkip = {framesToSkip}");
                LogInstance.Log($"declaringType?.FullName = {declaringType?.FullName} framesToSkip = {framesToSkip}");
                LogInstance.Log($"(attribute == null) = {attribute == null}");

                if (declaringType == null)
                {
                    break;
                }

                if (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            }

            return className;
        }

        private static void TSTEntityLogging()
        {
            var tmpName = GetClassFullName();

            LogInstance.Log($"tmpName = {tmpName}");

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();

            LogInstance.Log($"tmpCallInfo = {tmpCallInfo}");

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");

            entityLogger.Log("1 :)");

            entityLogger.Enabled = true;

            entityLogger.Log("2 :)");
        }

        private static void TSTConceptualGraph_2()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var graph = new ConceptualGraph();
            graph.Name = "#1";

            LogInstance.Log($"graph = {graph}");

            var graph_2 = new ConceptualGraph();
            graph_2.Name = "#2";

            LogInstance.Log($"graph_2 = {graph_2}");

            var concept = new ConceptCGNode();    
            concept.Name = "dog";

            LogInstance.Log($"concept = {concept}");

            concept.Parent = graph;

            var relation = new RelationCGNode();
            relation.Name = "color";
            relation.Parent = graph;

            var concept_2 = new ConceptCGNode();
            concept_2.Name = "black";
            concept_2.Parent = graph;

            relation.AddInputNode(concept);
            concept_2.AddInputNode(relation);

            LogInstance.Log($"concept = {concept}");
            LogInstance.Log($"relation = {relation}");
            LogInstance.Log($"concept_2 = {concept_2}");
            LogInstance.Log($"graph = {graph}");
            LogInstance.Log($"graph_2 = {graph_2}");

            var dotStr = DotConverter.ConvertToString(graph);

            LogInstance.Log($"dotStr = {dotStr}");

            LogInstance.Log("End");
        }

        private static void TSTConceptualGraphs()
        {
            LogInstance.Log("Begin");

            var parser = new TSTConceptualGraphParser();
            var globalStorage = new TSTGlobalLogicalStorage();

            LogInstance.Log($"globalStorage = {globalStorage}");

            var nlText = "Go to far waypoint.";

            LogInstance.Log($"TSTConceptualGraphs nlText = {nlText}");

            //I get a conceptual graph by some text of natural language.
            var graph = parser.Parse(nlText);

            LogInstance.Log($"graph = {graph}");

            //I get a query by the conceptual graph.
            //The query is a special storage which contains this conceptual graph.
            //We can get information about this conceptual graph by making queries to the storage.
            //The global storage is as parent for the storage.
            //So read-queries can get information what is related with the storage but it contains only in global storage.
            var queryStorage = globalStorage.Query(graph);

            LogInstance.Log($"queryStorage = {queryStorage}");

            //I get a conceptual graph from storage.
            //In this case it is a query-storage.
            //But all of kinds of storages can return a conceptual graph by this way.
            //If the storage contains one graph it is that graph.
            //It the storage contains two or more graphs it is undetermined graph of contained in ths storage.
            //Check kind of storage before using this method.
            var conceptualGraphFromQueryStorage = queryStorage.GetConceptualGraph();

            LogInstance.Log($"conceptualGraphFromQueryStorage = {conceptualGraphFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as gnu clay sentence (my own format).
            var gnuClaySentenceFromQueryStorage = queryStorage.GetGnuClaySentence();

            LogInstance.Log($"gnuClaySentenceFromQueryStorage = {gnuClaySentenceFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as sdandard predicate sentence.
            var predicateSentenceFromQueryStorage = queryStorage.GetPredicateSentence();

            LogInstance.Log($"predicateSentenceFromQueryStorage = {predicateSentenceFromQueryStorage}");

            //Add all information from `queryStorage` to `globalStorage`.
            //This information remains in `queryStorage`.
            //I will have duplicating of this information in both storages.
            globalStorage.Accept(queryStorage);

            //I can add directly a conceptual graph, gnu clay sentence or sdandard predicate sentence to any storage.
            globalStorage.Accept(predicateSentenceFromQueryStorage);

            //I create an empty storage which is based on `globalStorage` as its parent.
            var fork_1 = globalStorage.Fork();

            LogInstance.Log($"fork_1 = {fork_1}");

            //I create an empty storage which is based on `fork_1` as its parent.
            var fork_2 = fork_1.Fork();

            LogInstance.Log($"fork_2 = {fork_2}");

            //I create an empty storage which is based on `queryStorage` as its parent.
            var fork_3 = queryStorage.Fork();

            LogInstance.Log($"fork_3 = {fork_3}");

            LogInstance.Log("End");
        }

        private static void TSTRange()
        {
            LogInstance.Log("Begin");

            var list = ListHelper.GetRange(0, 90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach(var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(90, 0, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(-90, 0, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(90, 90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(10, 90, 0);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(0, -90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }

            LogInstance.Log("End");
        }

        private static void TSTLexer()
        {
        }

        private static void TSTLogicalAST()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var context = new ContextOfCGStorage(globalEntityDictionary);
            
            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var systemPropertiesDictionary = new SystemPropertiesDictionary(globalEntityDictionary);

            var npcHostContext = new StubOfNPCHostContext(entityLogger);

            var storageOfSpecialEntities = new StorageOfSpecialEntities();
            storageOfSpecialEntities.SelfEntityId = npcHostContext.SelfEntityId;
            var visionObjectsStorage = new VisionObjectsStorage(entityLogger, globalEntityDictionary, npcHostContext, systemPropertiesDictionary, storageOfSpecialEntities);
        }

        private static Dictionary<int, CancellationToken> mCancelationTokenDict = new Dictionary<int, CancellationToken>();

        private static void TSTCancelTask_2()
        {
            LogInstance.Log("Begin");

            var cs = new CancellationTokenSource();
            var token = cs.Token;
            var token2 = token;

            var tmpTask = new Task(() =>
            {
                try
                {
                    mCancelationTokenDict[Task.CurrentId.Value] = token;

                    LogInstance.Log("Task start");
                    LogInstance.Log($"Task.CurrentId = {Task.CurrentId}");

                    DoWork();
                }
                catch(OperationCanceledException)
                {
                    LogInstance.Log("catch(OperationCanceledException)");
                }
                catch(Exception e)
                {
                    LogInstance.Error($"Task e = {e}");
                }
                finally
                {
                    mCancelationTokenDict.Remove(Task.CurrentId.Value);
                    LogInstance.Log($"finally");
                }
            }, token);

            tmpTask.Start();

            LogInstance.Log("started");
            LogInstance.Log($"tmpTask.Id = {tmpTask.Id}");

            Thread.Sleep(1000);

            LogInstance.Log($"mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");

            cs.Cancel();

            LogInstance.Log("Canceled");

            cs.Cancel();

            LogInstance.Log("Canceled twice");

            Thread.Sleep(1000);

            LogInstance.Log($"after mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");
            LogInstance.Log("End");
        }

        private static void DoWork()
        {
            var token = mCancelationTokenDict[Task.CurrentId.Value];

            var n = 0;

            while (true)
            {
                LogInstance.Log($"n = {n}");
                n++;

                token.ThrowIfCancellationRequested();
            }
        }

        private static void TSTCancelTask()
        {
            LogInstance.Log("Begin");

            Thread tmpThread = null;

            var tmpTask = new Task(() => {
                tmpThread = Thread.CurrentThread;

                var n = 0;

                while(true)
                {
                    LogInstance.Log($"Task n = {n}");
                    n++;
                }
            });

            tmpTask.Start();

            LogInstance.Log("started");

            Thread.Sleep(1000);

            tmpThread.Abort();

            LogInstance.Log("aborted");

            Thread.Sleep(1000);

            LogInstance.Log("End");
        }

        private static void TSTMyNPCContext()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var stubOfHumanoidBodyController = new StubOfNPCHostContext(entityLogger, globalEntityDictionary);

            var context = new MyNPCContext(entityLogger, globalEntityDictionary, stubOfHumanoidBodyController);
            context.Bootstrap();

            Thread.Sleep(1000);

            var command = new NPCCommand();
            command.Name = "key press";
            command.Params.Add("key", "k");

            context.Send(command);

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var queryStr = "!((name=helen|name=ann)&class=girl)";

            var logicalObject = context.GetLogicalObject(queryStr);

            var entitiesIdList = logicalObject.CurrentEntitiesIdList;

            LogInstance.Log($"entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            var logicalObject_2 = context.GetLogicalObject(queryStr);

            var entitiesIdList_2 = logicalObject_2.CurrentEntitiesIdList;

            LogInstance.Log($"entitiesIdList_2.Count = {entitiesIdList_2.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            LogInstance.Log($"resultOfcomparsing = {resultOfcomparsing}");

            var name = logicalObject["name"];

            LogInstance.Log($"name = {name}");

            logicalObject["name"] = 12;

            name = logicalObject["name"];

            LogInstance.Log($"name (2) = {name}");

            resultOfcomparsing = context.SelfLogicalObject == logicalObject;

            LogInstance.Log($"resultOfcomparsing (2) = {resultOfcomparsing}");

            var visibleItems = context.VisibleObjects;
            LogInstance.Log($"visibleItems.Count = {visibleItems.Count}");
            foreach (var visibleItem in visibleItems)
            {
                LogInstance.Log($"visibleItem = {visibleItem}");
                var posOfVisibleItem = visibleItem["global position"];
                LogInstance.Log($"posOfVisibleItem = {posOfVisibleItem}");
            }

            var pos = context.SelfLogicalObject["global position"];

            LogInstance.Log($"pos = {pos}");

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        private static void TSTStorageOfNPCProcesses()
        {
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TmpConcreteNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            LogInstance.Log($"result = {result}");

            var command = new NPCCommand();
            command.Name = "test";

            LogInstance.Log($"command = {command}");

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            LogInstance.Log($"internalCommand = {internalCommand}");

            var process = storage.GetProcess(internalCommand);

            LogInstance.Log($"(process == null) = {process == null}");

            process = storage.GetProcess(internalCommand);

            LogInstance.Log($"(process == null) (2) = {process == null}");

            process.RunAsync();

            LogInstance.Log("-----------------------------------------------");

            //type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            //result = storage.AddTypeOfProcess(type);

            //LogInstance.Log($"result = {result}");

            //command = new NPCCommand();
            //command.Name = "SomeName";

            //LogInstance.Log($"command = {command}");

            //internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            //LogInstance.Log($"internalCommand = {internalCommand}");

            //process = storage.GetProcess(internalCommand);

            //LogInstance.Log($"(process == null) = {process == null}");

            //process = storage.GetProcess(internalCommand);

            //LogInstance.Log($"(process == null) (2) = {process == null}");

            Thread.Sleep(10000);

            LogInstance.Log("End");
        }

        private static void TSTActivatorOfNPCProcessEntryPointInfo()
        {
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);
            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(string));

            LogInstance.Log($"rank = {rank}");

            LogInstance.Log($"typeof(int?).FullName = {typeof(int?).FullName}");
            LogInstance.Log($"System.Nullable = {typeof(int?).FullName.StartsWith("System.Nullable")}");
            LogInstance.Log($"typeof(int?).IsClass = {typeof(int?).IsClass}");
            LogInstance.Log($"typeof(string).IsClass = {typeof(string).IsClass}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), typeof(int));

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(string), null);

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), null);

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int), null);

            LogInstance.Log($"rank = {rank}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var arg1Key = globalEntityDictionary.GetKey("someArgument");
            var arg2Key = globalEntityDictionary.GetKey("secondArgument");

            var paramsDict = new Dictionary<ulong, object>() { { arg1Key, true }, { arg2Key, 12 } };
            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            LogInstance.Log($"result.Count = {result.Count}");
            foreach(var tmpItem in result)
            {
                LogInstance.Log($"tmpItem = {tmpItem}");
            }

            type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            paramsDict = new Dictionary<ulong, object>() { { 1ul, true }, { 2ul, 12 } };
            result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            LogInstance.Log($"result.Count = {result.Count}");
            foreach (var tmpItem in result)
            {
                LogInstance.Log($"tmpItem = {tmpItem}");
            }
        }

        private static void CreateContextAndProcessesCase1()
        {
            LogInstance.Log("Begin");

            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var tmpContext = new TmpConcreteNPCContext(entityLogger, globalEntityDictionary, npcProcessInfoCache);

            var command = new NPCCommand();
            command.Name = "SomeName";
            command.InitiatingProcessId = 1;

            var process = tmpContext.Send(command);

            process.Task?.Wait();

            //Thread.Sleep(10000);

            //try
            //{
            //    npcProcessInfoCache.Set(null);
            //}
            //catch(Exception e)
            //{
            //    LogInstance.Log($"e = {e}");
            //}

            //try
            //{
            //    npcProcessInfoCache.Get(null);
            //}
            //catch (Exception e)
            //{
            //    LogInstance.Log($"e = {e}");
            //}

            LogInstance.Log("End");
        }

        private static void CreateInfoOfConcreteProcess()
        {
            LogInstance.Log("Begin");

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            //var type = typeof(TmpConcreteNPCProcess);
            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);

            LogInstance.Log($"type.FullName = {type.FullName}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            var method_1 = npcProcessInfo.EntryPointsInfoList.Single(p => p.ParametersMap.Count == 0);

            LogInstance.Log($"method_1 = {method_1}");

            var method_2 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.ContainsValue(typeof(int)) && p.ParametersMap.ContainsValue(typeof(bool)));
            LogInstance.Log($"method_2 = {method_2}");

            var method_3 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.Values.Count(x => x == typeof(int)) == 2);
            LogInstance.Log($"method_3 = {method_3}");

            //type = typeof(Program);
            //LogInstance.Log($"type.FullName = {type.FullName}");

            //npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            //LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            type = typeof(TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess);
            LogInstance.Log($"type.FullName = {type.FullName}");

            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            LogInstance.Log("End");
        }
    }
}
