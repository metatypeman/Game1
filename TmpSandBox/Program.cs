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
            //TSTMyNPCContext();
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

            var passiveLogicalObject = new PassiveLogicalObject(globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject.EntityId, passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";

            //indexingStorage.PutPropertyValue(12, namePropertyId, "helen");

            var conditionNode = new ConditionOfQueryASTNode();
            conditionNode.PropertyId = namePropertyId;
            conditionNode.Value = "helen";

            NLog.LogManager.GetCurrentClassLogger().Info($"TSTLogicalAST conditionNode = {conditionNode}");

            var entities
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
