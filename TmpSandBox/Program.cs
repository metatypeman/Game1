using MyNPCLib;
using System;
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

            CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
        }

        private void TryGetInfoAboutPreviouslyAddedTypeByKey_IfPreviouslyWasAddedTwoDifferentTypesOfProcesesWithTheSameName_OnDisposed_GotElementIsNotActiveException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var type_2 = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            Assert.NotEqual(type, type_2);

            storage.AddTypeOfProcess(type_2);

            var npcProcessInfoByType = storage.GetNPCProcessInfo(type);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfoByType, globalEntityDictionary, type);

            var npcProcessInfoByType_2 = storage.GetNPCProcessInfo(type_2);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfoByType_2, globalEntityDictionary, type);

            Assert.NotEqual(npcProcessInfoByType, npcProcessInfoByType_2);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() =>
            {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(key);
            });
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

            type = typeof(Program);
            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess npcProcessInfo = {npcProcessInfo}");

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateInfoOfConcreteProcess");
        }
    }
}
