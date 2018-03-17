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

            var tmpContext = new TmpConcreteNPCContext();

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateContextAndProcessesCase1");
        }

        private static void CreateInfoOfConcreteProcess()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin CreateInfoOfConcreteProcess");

            var type = typeof(TmpConcreteNPCProcess);

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            var atrrribute = type.GetCustomAttribute<NPCProcessStartupModeAttribute>();

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess atrrribute.StartupMode = {atrrribute.StartupMode}");

            var attribute2 = type.GetCustomAttribute<ObsoleteAttribute>();

            NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess (attribute2 == null) = {attribute2 == null}");

            var mainMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach(var method in mainMethods)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess method.Name = {method.Name}");

                var parametersList = method.GetParameters();

                foreach(var parameter in parametersList)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"CreateInfoOfConcreteProcess parameter.Name = {parameter.Name} parameter.ParameterType.FullName = {parameter.ParameterType.FullName}");
                }
            }

            var t = new Class1();
            t.PrintType(type);

            NLog.LogManager.GetCurrentClassLogger().Info("End CreateInfoOfConcreteProcess");
        }
    }
}
