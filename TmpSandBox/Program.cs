using MyNPCLib;
using System;
using System.Reflection;

namespace TmpSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var logProxy = new LogProxyForNLog();
            LogInstance.SetLogProxy(logProxy);

            //CreateContextAndProcessesCase1();
            CreateInfoOfConcreteProcess();
        }

        private static void CreateContextAndProcessesCase1()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateContextAndProcessesCase1");

            var globalEntityDictionary = new EntityDictionary();
            var tmpContext = new TmpConcreteNPCContext(globalEntityDictionary);

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateContextAndProcessesCase1");
        }

        private static void CreateInfoOfConcreteProcess()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateInfoOfConcreteProcess");

            var type = typeof(TmpConcreteNPCProcess);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(globalEntityDictionary);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess npcProcessInfo = {npcProcessInfo}");

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateInfoOfConcreteProcess");
        }
    }
}
