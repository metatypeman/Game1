using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessInfoFactory
    {
        public NPCProcessInfoFactory(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
        }

        #region private members
        private IEntityDictionary mEntityDictionary;
        #endregion

        public NPCProcessInfo CreateInfo(Type type)
        {
            var result = new NPCProcessInfo();

            var typeInfo = type.GetTypeInfo();

            result.Type = type;
            result.TypeInfo = typeInfo;

            var startupModeAttribute = typeInfo.GetCustomAttribute<NPCProcessStartupModeAttribute>();

            if(startupModeAttribute != null)
            {
                result.StartupMode = startupModeAttribute.StartupMode;
            }

            var nameAttribute = typeInfo.GetCustomAttribute<NPCProcessNameAttribute>();

            if(nameAttribute == null)
            {
                result.Name = type.FullName;
            }
            else
            {
                var name = nameAttribute.Name;

                if(string.IsNullOrWhiteSpace(name))
                {
                    result.Name = type.FullName;
                }
                else
                {
                    result.Name = name;
                }
            }

            result.Key = mEntityDictionary.GetKey(result.Name);

            var methodsList = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
            methodsList.AddRange(typeInfo.GetMethods().ToList());

            foreach (var method in methodsList)
            {
                if(method.Name.ToLower() != "main")
                {
                    continue;
                }

                var entryPointInfo = new NPCProcessEntryPointInfo();
                result.EntryPointsInfoList.Add(entryPointInfo);
                entryPointInfo.MethodInfo = method;
                var parametersList = method.GetParameters();

#if DEBUG
                LogInstance.Log($"CreateInfoOfConcreteProcess method.Name = {method.Name}");
#endif
                foreach (var parameter in parametersList)
                {
                    var parameterType = parameter.ParameterType;
                    var parameterName = parameter.Name;

#if DEBUG
                    LogInstance.Log($"CreateInfoOfConcreteProcess parameter.Name = {parameter.Name} parameter.ParameterType.FullName = {parameter.ParameterType.FullName}");
#endif

                    entryPointInfo.ParametersMap[parameterName] = parameterType;
                    entryPointInfo.IndexedParametersMap[mEntityDictionary.GetKey(parameterName)] = parameterType;
                }
            }

            return result;
        }
    }
}
