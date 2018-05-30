using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessInfoFactory
    {
        public NPCProcessInfoFactory(IEntityLogger entityLogger, IEntityDictionary entityDictionary)
        {
            mEntityLogger = entityLogger;
            mEntityDictionary = entityDictionary;
            mTypeInfoOfBaseNPCProcess = typeof(BaseNPCProcess).GetTypeInfo();
        }

        #region private members
        private IEntityLogger mEntityLogger;
        private IEntityDictionary mEntityDictionary;
        private TypeInfo mTypeInfoOfBaseNPCProcess;
        #endregion

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        public NPCProcessInfo CreateInfo(Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(!mTypeInfoOfBaseNPCProcess.IsAssignableFrom(type))
            {
                throw new TypeIsNotNPCProcessException(type);
            }

            var typeInfo = type.GetTypeInfo();

            var result = new NPCProcessInfo();

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
                //Log($"method.Name = {method.Name}");
#endif
                foreach (var parameter in parametersList)
                {
                    var parameterType = parameter.ParameterType;
                    var parameterName = parameter.Name;
                    var defaultValue = parameter.DefaultValue;
                    var parameterKey = mEntityDictionary.GetKey(parameterName);
#if DEBUG
                    //Log($"parameter.Name = {parameter.Name} parameter.ParameterType.FullName = {parameter.ParameterType.FullName} parameterKey = {parameterKey} defaultValue = {defaultValue} defaultValue.GetType().FullName = {defaultValue.GetType().FullName}");
#endif

                    entryPointInfo.ParametersMap[parameterName] = parameterType;
                    entryPointInfo.IndexedParametersMap[parameterKey] = parameterType;

                    if(defaultValue != DBNull.Value)
                    {
                        entryPointInfo.DefaultValuesMap[parameterName] = defaultValue;
                        entryPointInfo.IndexedDefaultValuesMap[parameterKey] = defaultValue;
                    }
                }
            }

            return result;
        }
    }
}
