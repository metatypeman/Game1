using MyNPCLib;
using System;

namespace TmpSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var logProxy = new LogProxyForNLog();
            LogInstance.SetLogProxy(logProxy);

            CreateContextAndProcessesCase1();
        }

        private static void CreateContextAndProcessesCase1()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateContextAndProcessesCase1");

            var idFactory = new IdFactory();

            var i = idFactory.GetNewId();

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 i = {i}");

            i = idFactory.GetNewId();

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateContextAndProcessesCase1 i = {i}");
        }
    }
}
