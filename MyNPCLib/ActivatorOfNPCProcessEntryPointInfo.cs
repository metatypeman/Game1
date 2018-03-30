using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class ActivatorOfNPCProcessEntryPointInfo
    {
        public float GetRankByTypesOfParameters(Type typeOfArgument, Type typeOfParameter)
        {
#if DEBUG
            //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints typeOfArgument = {typeOfArgument?.FullName}");
            //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints typeOfParameter = {typeOfParameter?.FullName}");
#endif

            if(typeOfArgument == null)
            {
                throw new ArgumentNullException(nameof(typeOfArgument));
            }

            var typeInfoOfArgument = typeOfArgument.GetTypeInfo();

            if (typeOfParameter == null)
            {
                if(typeInfoOfArgument.IsClass)
                {
                    return 0.5f;
                }

                if(typeInfoOfArgument.IsGenericType)
                {
                    if(typeInfoOfArgument.FullName.StartsWith("System.Nullable"))
                    {
                        return 0.5f;
                    }
                }

                return 0f;
            }

            if (typeOfArgument == typeOfParameter)
            {
                return 1f;
            }

            if(typeInfoOfArgument.IsAssignableFrom(typeOfParameter))
            {
                return 0.5f;
            }

            return 0f;
        }

        public List<RankedNPCProcessEntryPointInfo> GetRankedEntryPoints(NPCProcessInfo npcProcessInfo, Dictionary<ulong, object> paramsOfCommand)
        {
#if DEBUG
            //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints npcProcessInfo = {npcProcessInfo}");
            //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints paramsOfCommand = {paramsOfCommand?.Count}");
#endif

            if (npcProcessInfo == null)
            {
                throw new ArgumentNullException(nameof(npcProcessInfo));
            }

            if (paramsOfCommand == null)
            {
                throw new ArgumentNullException(nameof(paramsOfCommand));
            }

            var entryPointsList = npcProcessInfo.EntryPointsInfoList;
            
            if(entryPointsList == null)
            {
                throw new ArgumentNullException("npcProcessInfo.EntryPointsInfoList");
            }

            var result = new List<RankedNPCProcessEntryPointInfo>();

            if(entryPointsList.Count == 0)
            {
                return result;
            }

            foreach(var entryPoint in entryPointsList)
            {
#if DEBUG
                //LogInstance.Log("ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints ------------------------------------------------------------------------");
                //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints entryPoint = {entryPoint}");
#endif

                var indexedParametersMap = entryPoint.IndexedParametersMap;
                var indexedDefaultValuesMap = entryPoint.IndexedDefaultValuesMap;

                var needContinue = false;

                var rank = 1f;

                var foundParameters = 0;

                foreach(var argumentKVPItem in indexedParametersMap)
                {
#if DEBUG
                    //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints argumentKVPItem.Key = {argumentKVPItem.Key} argumentKVPItem.Value = {argumentKVPItem.Value}");
#endif

                    var key = argumentKVPItem.Key;
                    var argumentType = argumentKVPItem.Value;

                    Type paramType = null;

                    if(paramsOfCommand.ContainsKey(key))
                    {
                        var paramValue = paramsOfCommand[key];

                        if(paramValue != null)
                        {
                            paramType = paramValue.GetType();
                        }
                    }
                    else
                    {
                        if (indexedDefaultValuesMap.ContainsKey(key))
                        {
                            var paramValue = indexedDefaultValuesMap[key];

                            if (paramValue != null)
                            {
                                paramType = paramValue.GetType();
                            }
                        }
                        else
                        {
                            needContinue = true;
                            break;
                        }
                    }
                    
#if DEBUG
                    //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints paramType = {paramType?.FullName}");
#endif

                    var currentRank = GetRankByTypesOfParameters(argumentType, paramType);

#if DEBUG
                    //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints currentRank = {currentRank}");
#endif

                    if(currentRank == 0f)
                    {
                        needContinue = true;
                        break;
                    }

                    foundParameters++;
                    rank *= currentRank;
                }

                if (needContinue)
                {
                    continue;
                }

                if (indexedParametersMap.Count != foundParameters)
                {
                    continue;
                }

                if(paramsOfCommand.Count > foundParameters)
                {
                    continue;
                }
#if DEBUG
                //LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints NEXT rank = {rank} indexedParametersMap.Count = {indexedParametersMap.Count} foundParameters = {foundParameters}");
#endif

                var item = new RankedNPCProcessEntryPointInfo();
                item.Rank = rank;
                item.EntryPoint = entryPoint;

                result.Add(item);
            }

            if(result.Count == 0)
            {
                return result;
            }

            return result.OrderByDescending(p => p.Rank).ToList();
        }

        public RankedNPCProcessEntryPointInfo GetTopEntryPoint(NPCProcessInfo npcProcessInfo, Dictionary<ulong, object> paramsOfCommand)
        {
            var rankedEntryPointsList = GetRankedEntryPoints(npcProcessInfo, paramsOfCommand);

            return rankedEntryPointsList.FirstOrDefault();
        }

        public void CallEntryPoint(BaseNPCProcess npcProcess, NPCProcessEntryPointInfo entryPoint, Dictionary<ulong, object> paramsOfCommand)
        {
            if(npcProcess == null)
            {
                throw new ArgumentNullException(nameof(npcProcess));
            }

            if(entryPoint == null)
            {
                throw new ArgumentNullException(nameof(entryPoint));
            }

            if(paramsOfCommand == null)
            {
                throw new ArgumentNullException(nameof(paramsOfCommand));
            }

            object[] args = null;

            if (paramsOfCommand.Count > 0)
            {
                var argsList = new List<object>();

                var indexedParametersMap = entryPoint.IndexedParametersMap;
                var indexedDefaultValuesMap = entryPoint.IndexedDefaultValuesMap;

                foreach (var argumentKVPItem in indexedParametersMap)
                {
                    var key = argumentKVPItem.Key;

                    if (paramsOfCommand.ContainsKey(key))
                    {
                        var paramValue = paramsOfCommand[key];

                        if (paramValue != null)
                        {
                            argsList.Add(paramValue);
                        }
                    }
                    else
                    {
                        if (indexedDefaultValuesMap.ContainsKey(key))
                        {
                            var paramValue = indexedDefaultValuesMap[key];

                            if (paramValue != null)
                            {
                                argsList.Add(paramValue);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                }

                args = argsList.ToArray();
            }

            var targetMethod = entryPoint.MethodInfo;
            targetMethod.Invoke(npcProcess, args);
        }
    }
}
