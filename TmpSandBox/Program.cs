using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TmpSandBox.NPCBehaviour;
using TmpSandBox.TstSmartObj;

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

            TSTLogicalAST();
            //TSTLogicalObject();
            //TSTCancelTask_2();
            //TSTCancelTask();
            TSTMyNPCContext();
            //TSTStorageOfNPCProcesses();
            //TSTActivatorOfNPCProcessEntryPointInfo();
            //CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
        }

        private static void TSTLogicalAST()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTLogicalAST");

            var globalEntityDictionary = new EntityDictionary();

            var indexingStorage = new LogicalIndexStorage();

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var passiveLogicalObject = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject.EntityId, passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";
            passiveLogicalObject[classPropertyId] = "girl";

            var passiveLogicalObject_2 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_2.EntityId, passiveLogicalObject_2);

            passiveLogicalObject_2[namePropertyId] = "ann";
            passiveLogicalObject_2[classPropertyId] = "girl";

            var passiveLogicalObject_3 = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_3.EntityId, passiveLogicalObject_3);

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

            var queryStr = "!((name=helen|name=ann)&class=girl)";
            var logicalObject = new MyNPCLib.Logical.LogicalObject(queryStr, globalEntityDictionary, indexingStorage);

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

            var logicalObject_2 = new MyNPCLib.Logical.LogicalObject(queryStr, globalEntityDictionary, indexingStorage);

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

        private static void TSTLogicalObject()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTLogicalObject");

            var logicalContext = new LogicalContext();

            var logicalObject = logicalContext.Get("name = helen");

            var visibleObj = new TestedVisibleItem();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalObject (logicalObject == visibleObj) = {logicalObject == visibleObj}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalObject (visibleObj == logicalObject) = {visibleObj == logicalObject}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalObject logicalObject['a'] = {logicalObject["a"]}");
            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalObject logicalObject['a', 16] = {logicalObject["a", 16]}");

            var getProperty = logicalObject.GetProperty("name");

            var c = new C();
            var b = new B();

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalObject (b == c) = {b == c}");
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

            var stubOfHumanoidBodyController = new StubOfNPCHostContext();

            var context = new MyNPCContext(stubOfHumanoidBodyController);
            context.Bootstrap();

            Thread.Sleep(1000);

            var command = new NPCCommand();
            command.Name = "key press";
            command.Params.Add("key", "k");

            context.Send(command);

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

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST entitiesIdList_2.Count = {entitiesIdList_2.Count}");
            foreach (var entityId in entitiesIdList)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST resultOfcomparsing = {resultOfcomparsing}");

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
            var tmpContext = new TmpConcreteNPCContext(globalEntityDictionary, npcProcessInfoCache);

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
