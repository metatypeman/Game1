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

            var tmpContext = new TmpConcreteNPCContext();

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateContextAndProcessesCase1");
        }
    }
}
