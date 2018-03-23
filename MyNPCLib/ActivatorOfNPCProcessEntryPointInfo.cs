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

        public List<RankedNPCProcessEntryPointInfo> GetRankedEntryPoints(NPCProcessInfo npcProcessInfo, INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints npcProcessInfo = {npcProcessInfo}");
            LogInstance.Log($"ActivatorOfNPCProcessEntryPointInfo GetRankedEntryPoints command = {command}");
#endif
            throw new NotImplementedException();
        }
    }
}
