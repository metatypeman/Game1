using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.Dot;
using MyNPCLib.Logical;
using MyNPCLib.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            TSTEntityLogging();
            //TSTConceptualGraph_2();
            //TSTConceptualGraphs();
            //TSTRange();
            //TSTLexer();
            //TSTLogicalAST();
            //TSTCancelTask_2();
            //TSTCancelTask();
            TSTMyNPCContext();
            //TSTStorageOfNPCProcesses();
            //TSTActivatorOfNPCProcessEntryPointInfo();
            //CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
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
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTConceptualGraphs_2");

            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            context.Init();

            var graph = new ConceptualGraph();
            graph.Name = "#1";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 graph = {graph}");

            var graph_2 = new ConceptualGraph();
            graph_2.Name = "#2";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 graph_2 = {graph_2}");

            var concept = new ConceptCGNode();    
            concept.Name = "dog";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 concept = {concept}");

            concept.Parent = graph;

            var relation = new RelationCGNode();
            relation.Name = "color";
            relation.Parent = graph;

            var concept_2 = new ConceptCGNode();
            concept_2.Name = "black";
            concept_2.Parent = graph;

            relation.AddInputNode(concept);
            concept_2.AddInputNode(relation);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 concept = {concept}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 relation = {relation}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 concept_2 = {concept_2}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 graph = {graph}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 graph_2 = {graph_2}");

            var dotStr = DotConverter.ConvertToString(graph);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs_2 dotStr = {dotStr}");

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTConceptualGraphs_2");
        }

        private static void TSTConceptualGraphs()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTConceptualGraphs");

            var parser = new TSTConceptualGraphParser();
            var globalStorage = new TSTGlobalLogicalStorage();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs globalStorage = {globalStorage}");

            var nlText = "Go to far waypoint.";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs nlText = {nlText}");

            //I get a conceptual graph by some text of natural language.
            var graph = parser.Parse(nlText);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs graph = {graph}");

            //I get a query by the conceptual graph.
            //The query is a special storage which contains this conceptual graph.
            //We can get information about this conceptual graph by making queries to the storage.
            //The global storage is as parent for the storage.
            //So read-queries can get information what is related with the storage but it contains only in global storage.
            var queryStorage = globalStorage.Query(graph);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs queryStorage = {queryStorage}");

            //I get a conceptual graph from storage.
            //In this case it is a query-storage.
            //But all of kinds of storages can return a conceptual graph by this way.
            //If the storage contains one graph it is that graph.
            //It the storage contains two or more graphs it is undetermined graph of contained in ths storage.
            //Check kind of storage before using this method.
            var conceptualGraphFromQueryStorage = queryStorage.GetConceptualGraph();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs conceptualGraphFromQueryStorage = {conceptualGraphFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as gnu clay sentence (my own format).
            var gnuClaySentenceFromQueryStorage = queryStorage.GetGnuClaySentence();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs gnuClaySentenceFromQueryStorage = {gnuClaySentenceFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as sdandard predicate sentence.
            var predicateSentenceFromQueryStorage = queryStorage.GetPredicateSentence();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs predicateSentenceFromQueryStorage = {predicateSentenceFromQueryStorage}");

            //Add all information from `queryStorage` to `globalStorage`.
            //This information remains in `queryStorage`.
            //I will have duplicating of this information in both storages.
            globalStorage.Accept(queryStorage);

            //I can add directly a conceptual graph, gnu clay sentence or sdandard predicate sentence to any storage.
            globalStorage.Accept(predicateSentenceFromQueryStorage);

            //I create an empty storage which is based on `globalStorage` as its parent.
            var fork_1 = globalStorage.Fork();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs fork_1 = {fork_1}");

            //I create an empty storage which is based on `fork_1` as its parent.
            var fork_2 = fork_1.Fork();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs fork_2 = {fork_2}");

            //I create an empty storage which is based on `queryStorage` as its parent.
            var fork_3 = queryStorage.Fork();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTConceptualGraphs fork_3 = {fork_3}");

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTConceptualGraphs");
        }

        private static void TSTRange()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTRange");

            var list = ListHelper.GetRange(0, 90, 5);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach(var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }
            list = ListHelper.GetRange(90, 0, 5);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach (var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }
            list = ListHelper.GetRange(-90, 0, 5);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach (var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }
            list = ListHelper.GetRange(90, 90, 5);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach (var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }
            list = ListHelper.GetRange(10, 90, 0);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach (var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }
            list = ListHelper.GetRange(0, -90, 5);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange list.Count = {list.Count}");
            foreach (var item in list)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTRange item = {item}");
            }

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTRange");
        }

        private static void TSTLexer()
        {
            var queryStr = "!((name='helen'|name='ann')&class='girl')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            //var lexer = new Lexer(queryStr);
            //Token token = null;
            //while ((token = lexer.GetToken()) != null)
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer token = {token}");
            //}
            var globalEntityDictionary = new EntityDictionary();
            var context = new ParserContext(queryStr, globalEntityDictionary);
            //Token token = null;
            //while ((token = context.GetToken()) != null)
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer token = {token}");
            //}
            //var parser = new LogicalExpressionParser(context);
            //parser.Run();
            var node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "!((name='helen'&name='ann')|class='girl')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "(name='helen'&name='ann')|class='girl'";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "class='girl'|(name='helen'&name='ann')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "class='girl'&(name='helen'&name='ann')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "class='girl'&!(name='helen'&name='ann')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "class='girl'|!(name='helen'&name='ann')";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "!class='girl'";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");

            queryStr = "class='girl'";
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLexer node = {node}");
        }

        private static void TSTLogicalAST()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTLogicalAST");

            var globalEntityDictionary = new EntityDictionary();

            var indexingStorage = new LogicalIndexStorage();

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var passiveLogicalObject = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";
            passiveLogicalObject[classPropertyId] = "girl";

            var passiveLogicalObject_2 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_2);

            passiveLogicalObject_2[namePropertyId] = "ann";
            passiveLogicalObject_2[classPropertyId] = "girl";

            var passiveLogicalObject_3 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_3);

            passiveLogicalObject_3[namePropertyId] = "Beatles";
            passiveLogicalObject_3[classPropertyId] = "band";

            //indexingStorage.PutPropertyValue(12, namePropertyId, "helen");

            var conditionNode = new ConditionOfQueryASTNode();
            conditionNode.PropertyId = namePropertyId;
            conditionNode.Value = "helen";

            var conditionNode_2 = new ConditionOfQueryASTNode();
            conditionNode_2.PropertyId = namePropertyId;
            conditionNode_2.Value = "ann";

            var orNode = new BinaryOperatorOfQueryASTNode();
            orNode.OperatorId = KindOfBinaryOperators.Or;
            orNode.Left = conditionNode;
            orNode.Right = conditionNode_2;

            var conditionNode_1 = new ConditionOfQueryASTNode();
            conditionNode_1.PropertyId = classPropertyId;
            conditionNode_1.Value = "girl";

            var andNode = new BinaryOperatorOfQueryASTNode();
            andNode.OperatorId = KindOfBinaryOperators.And;
            andNode.Left = orNode;
            andNode.Right = conditionNode_1;

            var notNode = new UnaryOperatorOfQueryASTNode();
            notNode.OperatorId = KindOfUnaryOperators.Not;
            notNode.Left = andNode;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST notNode = {notNode}");

            var queryCache = new QueriesCache(globalEntityDictionary);

            var systemPropertiesDictionary = new SystemPropertiesDictionary(globalEntityDictionary);

            var npcHostContext = new StubOfNPCHostContext();

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var storageOfSpecialEntities = new StorageOfSpecialEntities();
            storageOfSpecialEntities.SelfEntityId = npcHostContext.SelfEntityId;
            var visionObjectsStorage = new VisionObjectsStorage(entityLogger, globalEntityDictionary, npcHostContext, systemPropertiesDictionary, storageOfSpecialEntities);

            var queryStr = "!((name=helen|name=ann)&class=girl)";
            var logicalObject = new LogicalObject(entityLogger, queryStr, globalEntityDictionary, indexingStorage, queryCache, systemPropertiesDictionary, visionObjectsStorage);

            var entitiesIdList = logicalObject.CurrentEntitiesIdList;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST entityId = {entityId}");
            }

            passiveLogicalObject_2[classPropertyId] = "boy";

            Thread.Sleep(100);

            entitiesIdList = logicalObject.CurrentEntitiesIdList;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (2) entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (2) entityId = {entityId}");
            }

            var logicalObject_2 = new LogicalObject(entityLogger, queryStr, globalEntityDictionary, indexingStorage, queryCache, systemPropertiesDictionary, visionObjectsStorage);

            entitiesIdList = logicalObject.CurrentEntitiesIdList;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (3) entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (3) entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST resultOfcomparsing = {resultOfcomparsing}");

            resultOfcomparsing = logicalObject_2 == logicalObject;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (2) resultOfcomparsing = {resultOfcomparsing}");

            /*var list1 = new List<int>() { 1 };
            var list2 = new List<int>() { 1, 2 };

            var except1_2 = list1.Except(list2).ToList();
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST except1_2.Count = {except1_2.Count}");
            foreach (var entityId in except1_2)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (1_2) entityId = {entityId}");
            }

            var except2_1 = list2.Except(list1).ToList();
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST except2_1.Count = {except2_1.Count}");
            foreach (var entityId in except2_1)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST (2_1) entityId = {entityId}");
            }*/
        }

        private static Dictionary<int, CancellationToken> mCancelationTokenDict = new Dictionary<int, CancellationToken>();

        private static void TSTCancelTask_2()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTCancelTask_2");

            var cs = new CancellationTokenSource();
            var token = cs.Token;
            var token2 = token;

            var tmpTask = new Task(() =>
            {
                try
                {
                    mCancelationTokenDict[Task.CurrentId.Value] = token;

                    NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask_2 Task start");
                    NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask_2 Task.CurrentId = {Task.CurrentId}");

                    DoWork();
                }
                catch(OperationCanceledException)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask_2 catch(OperationCanceledException)");
                }
                catch(Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask_2 Task e = {e}");
                }
                finally
                {
                    mCancelationTokenDict.Remove(Task.CurrentId.Value);
                    NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask_2 finally");
                }
            }, token);

            tmpTask.Start();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask_2 started");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask_2 tmpTask.Id = {tmpTask.Id}");

            Thread.Sleep(1000);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask_2 mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");

            cs.Cancel();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask_2 Canceled");

            cs.Cancel();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask_2 Canceled twice");

            Thread.Sleep(1000);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask after mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");
            NLog.LogManager.GetCurrentClassLogger().Info("End TSTCancelTask_2");
        }

        private static void DoWork()
        {
            var token = mCancelationTokenDict[Task.CurrentId.Value];

            var n = 0;

            while (true)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"DoWork Task n = {n}");
                n++;

                token.ThrowIfCancellationRequested();
            }
        }

        private static void TSTCancelTask()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTCancelTask");

            Thread tmpThread = null;

            var tmpTask = new Task(() => {
                tmpThread = Thread.CurrentThread;

                var n = 0;

                while(true)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"TSTCancelTask Task n = {n}");
                    n++;
                }
            });

            tmpTask.Start();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask started");

            Thread.Sleep(1000);

            tmpThread.Abort();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTCancelTask aborted");

            Thread.Sleep(1000);

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTCancelTask");
        }

        private static void TSTMyNPCContext()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTMyNPCContext");

            var globalEntityDictionary = new EntityDictionary();
            var stubOfHumanoidBodyController = new StubOfNPCHostContext(globalEntityDictionary);

            var indexingStorage = stubOfHumanoidBodyController.LogicalIndexStorageImpl;

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var context = new MyNPCContext(entityLogger, globalEntityDictionary, stubOfHumanoidBodyController);
            context.Bootstrap();

            Thread.Sleep(1000);

            var command = new NPCCommand();
            command.Name = "key press";
            command.Params.Add("key", "k");

            context.Send(command);

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var passiveLogicalObject = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";
            passiveLogicalObject[classPropertyId] = "girl";

            var passiveLogicalObject_2 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_2);

            passiveLogicalObject_2[namePropertyId] = "ann";
            passiveLogicalObject_2[classPropertyId] = "girl";

            var passiveLogicalObject_3 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_3);

            passiveLogicalObject_3[namePropertyId] = "Beatles";
            passiveLogicalObject_3[classPropertyId] = "band";

            var queryStr = "!((name=helen|name=ann)&class=girl)";

            var logicalObject = context.GetLogicalObject(queryStr);

            var entitiesIdList = logicalObject.CurrentEntitiesIdList;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext entityId = {entityId}");
            }

            var logicalObject_2 = context.GetLogicalObject(queryStr);

            var entitiesIdList_2 = logicalObject_2.CurrentEntitiesIdList;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext entitiesIdList_2.Count = {entitiesIdList_2.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext resultOfcomparsing = {resultOfcomparsing}");

            var name = logicalObject["name"];

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext name = {name}");

            logicalObject["name"] = 12;

            name = logicalObject["name"];

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext name (2) = {name}");

            resultOfcomparsing = context.SelfLogicalObject == logicalObject;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext resultOfcomparsing (2) = {resultOfcomparsing}");

            var visibleItems = context.VisibleObjects;
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext visibleItems.Count = {visibleItems.Count}");
            foreach (var visibleItem in visibleItems)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext visibleItem = {visibleItem}");
                var posOfVisibleItem = visibleItem["global position"];
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext posOfVisibleItem = {posOfVisibleItem}");
            }

            var pos = context.SelfLogicalObject["global position"];

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTMyNPCContext pos = {pos}");

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        private static void TSTStorageOfNPCProcesses()
        {
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext();
            var storage = new StorageOfNPCProcesses(idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TmpConcreteNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses result = {result}");

            var command = new NPCCommand();
            command.Name = "test";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses command = {command}");

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses internalCommand = {internalCommand}");

            var process = storage.GetProcess(internalCommand);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses (process == null) = {process == null}");

            process = storage.GetProcess(internalCommand);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses (process == null) (2) = {process == null}");

            process.RunAsync();

            NLog.LogManager.GetCurrentClassLogger().Info("TSTStorageOfNPCProcesses -----------------------------------------------");

            //type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            //result = storage.AddTypeOfProcess(type);

            //NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses result = {result}");

            //command = new NPCCommand();
            //command.Name = "SomeName";

            //NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses command = {command}");

            //internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            //NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses internalCommand = {internalCommand}");

            //process = storage.GetProcess(internalCommand);

            //NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses (process == null) = {process == null}");

            //process = storage.GetProcess(internalCommand);

            //NLog.LogManager.GetCurrentClassLogger().Info($"TSTStorageOfNPCProcesses (process == null) (2) = {process == null}");

            Thread.Sleep(10000);

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTStorageOfNPCProcesses");
        }

        private static void TSTActivatorOfNPCProcessEntryPointInfo()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();
            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(string));

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo rank = {rank}");

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo typeof(int?).FullName = {typeof(int?).FullName}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo System.Nullable = {typeof(int?).FullName.StartsWith("System.Nullable")}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo typeof(int?).IsClass = {typeof(int?).IsClass}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo typeof(string).IsClass = {typeof(string).IsClass}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), typeof(int));

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(string), null);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), null);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int), null);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo rank = {rank}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var arg1Key = globalEntityDictionary.GetKey("someArgument");
            var arg2Key = globalEntityDictionary.GetKey("secondArgument");

            var paramsDict = new Dictionary<ulong, object>() { { arg1Key, true }, { arg2Key, 12 } };
            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo result.Count = {result.Count}");
            foreach(var tmpItem in result)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo tmpItem = {tmpItem}");
            }

            type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            paramsDict = new Dictionary<ulong, object>() { { 1ul, true }, { 2ul, 12 } };
            result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo result.Count = {result.Count}");
            foreach (var tmpItem in result)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTActivatorOfNPCProcessEntryPointInfo tmpItem = {tmpItem}");
            }
        }

        private static void CreateContextAndProcessesCase1()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateContextAndProcessesCase1");

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
            //    NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 e = {e}");
            //}

            //try
            //{
            //    npcProcessInfoCache.Get(null);
            //}
            //catch (Exception e)
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 e = {e}");
            //}

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateContextAndProcessesCase1");
        }

        private static void CreateInfoOfConcreteProcess()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateInfoOfConcreteProcess");

            //var type = typeof(TmpConcreteNPCProcess);
            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(globalEntityDictionary);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess npcProcessInfo = {npcProcessInfo}");

            var method_1 = npcProcessInfo.EntryPointsInfoList.Single(p => p.ParametersMap.Count == 0);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess method_1 = {method_1}");

            var method_2 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.ContainsValue(typeof(int)) && p.ParametersMap.ContainsValue(typeof(bool)));
            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess method_2 = {method_2}");

            var method_3 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.Values.Count(x => x == typeof(int)) == 2);
            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess method_3 = {method_3}");

            //type = typeof(Program);
            //NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            //npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            //NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess npcProcessInfo = {npcProcessInfo}");

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateInfoOfConcreteProcess");

            type = typeof(TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess);
            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess npcProcessInfo = {npcProcessInfo}");
        }
    }
}
