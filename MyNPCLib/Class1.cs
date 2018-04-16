using System;
using System.Collections;
using System.Collections.Generic;
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

            });
        }

        public void Tst()
        {
            Thread.Sleep(10);
            LogInstance.Log("Tst");
        }

        public void PrintType(Type type)
        {
            LogInstance.Log($"CreateInfoOfConcreteProcess type.FullName = {type.FullName}");

            var typeInfo = type.GetTypeInfo();

            //var customAttributes = typeInfo.GetCustomAttribute<>

            var atrrribute = typeInfo.GetCustomAttribute<NPCProcessStartupModeAttribute>();

            LogInstance.Log($"CreateInfoOfConcreteProcess atrrribute.StartupMode = {atrrribute.StartupMode}");

            var attribute2 = typeInfo.GetCustomAttribute<ObsoleteAttribute>();

            LogInstance.Log($"CreateInfoOfConcreteProcess (attribute2 == null) = {attribute2 == null}");

            var mainMethods = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in mainMethods)
            {
                LogInstance.Log($"CreateInfoOfConcreteProcess method.Name = {method.Name}");

                var parametersList = method.GetParameters();

                foreach (var parameter in parametersList)
                {
                    LogInstance.Log($"CreateInfoOfConcreteProcess parameter.Name = {parameter.Name} parameter.ParameterType.FullName = {parameter.ParameterType.FullName}");
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
            LogInstance.Log("GetItems");

            return new List<int>();
        }
    }
}
