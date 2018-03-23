using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class ActivatorOfNPCProcessEntryPointInfo
    {
        public float GetRankByTypesOfParameters(Type typeOfArgument, Type typeOfParameter)
        {
#if DEBUG
            LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints typeOfArgument = {typeOfArgument?.FullName}");
            LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints typeOfParameter = {typeOfParameter?.FullName}");
#endif

            if(typeOfArgument == null)
            {
                throw new ArgumentNullException(nameof(typeOfArgument));
            }

            if(typeOfParameter == null)
            {
                throw new ArgumentNullException(nameof(typeOfParameter));
            }

            if(typeOfArgument == typeOfParameter)
            {
                return 1f;
            }

            if(typeOfArgument.GetTypeInfo().IsAssignableFrom(typeOfParameter))
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
                var indexedParametersMap = entryPoint.IndexedParametersMap;

                if (indexedParametersMap.Count != paramsOfCommand.Count)
                {
                    continue;
                }

                var fetchedCount = 0;

                foreach(var paramOfCommandKVPItem in paramsOfCommand)
                {
                    var key = paramOfCommandKVPItem.Key;

                    if(!indexedParametersMap.ContainsKey(key))
                    {
                        continue;
                    }

                    throw new NotImplementedException();
                }

                throw new NotImplementedException();
            }

            return result;
        }
    }
}
