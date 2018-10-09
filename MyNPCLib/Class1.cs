using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class Class1
    {
        public void Run()
        {
            Task.Run(() => {
                try
                {

                }
                catch (Exception e)
                {
#if DEBUG
                    LogInstance.Error($"e = {e}");
#endif
                }
            });
        }

        public void Tst()
        {
            Thread.Sleep(10);
            LogInstance.Log("Tst");
        }

        public void PrintType(Type type)
        {
            LogInstance.Log($"type.FullName = {type.FullName}");

            var typeInfo = type.GetTypeInfo();

            //var customAttributes = typeInfo.GetCustomAttribute<>

            var atrrribute = typeInfo.GetCustomAttribute<NPCProcessStartupModeAttribute>();

            LogInstance.Log($"atrrribute.StartupMode = {atrrribute.StartupMode}");

            var attribute2 = typeInfo.GetCustomAttribute<ObsoleteAttribute>();

            LogInstance.Log($"(attribute2 == null) = {attribute2 == null}");

            var mainMethods = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in mainMethods)
            {
                LogInstance.Log($"method.Name = {method.Name}");

                var parametersList = method.GetParameters();

                foreach (var parameter in parametersList)
                {
                    LogInstance.Log($"parameter.Name = {parameter.Name} parameter.ParameterType.FullName = {parameter.ParameterType.FullName}");
                }
            }

            var instance = Activator.CreateInstance(type);

            var targetMethod = mainMethods[0];
            targetMethod.Invoke(instance, null);

            targetMethod = mainMethods[1];
            targetMethod.Invoke(instance, new object[] { 12 });
        }

        public IList<int> GetItems()
        {
            LogInstance.Log("Begin");

            return new List<int>();
        }

        public void TSTCancelTask()
        {
            LogInstance.Log("Begin");

            Thread tmpThread = null;

            var tmpTask = new Task(() => {
                tmpThread = Thread.CurrentThread;

                var n = 0;

                while (true)
                {
                    LogInstance.Log($"Task n = {n}");
                    n++;
                }
            });

            tmpTask.Start();

            LogInstance.Log("started");

            Thread.Sleep(1000);

            //tmpThread.

            LogInstance.Log("aborted");

            Thread.Sleep(1000);

            LogInstance.Log("End");
        }

        public Vector3 GetVector()
        {
            return new Vector3(1, 2, 3);
        }
    }
}
