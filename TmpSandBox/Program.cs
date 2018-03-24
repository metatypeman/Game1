using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TmpSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var logProxy = new LogProxyForNLog();
            LogInstance.SetLogProxy(logProxy);

            TSTActivatorOfNPCProcessEntryPointInfo();
            //CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
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
            //var tmpContext = new TmpConcreteNPCContext(globalEntityDictionary, npcProcessInfoCache);

            try
            {
                npcProcessInfoCache.Set(null);
            }
            catch(Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 e = {e}");
            }

            try
            {
                npcProcessInfoCache.Get(null);
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 e = {e}");
            }

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
