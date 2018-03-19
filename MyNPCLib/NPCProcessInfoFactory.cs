using System;
using System.Collections.Generic;
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

            var mainMethods = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in mainMethods)
            {
                var entryPointInfo = new NPCProcessEntryPointInfo();
                result.EntryPointsInfoList.Add(entryPointInfo);

                var parametersList = method.GetParameters();

                foreach (var parameter in parametersList)
                {
                    var parameterType = parameter.ParameterType;
                    var parameterName = parameter.Name;

                    entryPointInfo.ParametersMap[parameterName] = parameterType;
                    entryPointInfo.IndexedParametersMap[mEntityDictionary.GetKey(parameterName)] = parameterType;
                }
            }

            return result;
        }
    }
}
