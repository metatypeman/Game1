using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class NPCProcessHelpers
    {
        public static void RegProcess(INPCContext context, INPCProcess npcProcess, NPCProcessStartupMode startupMode, KindOfLinkingToInitiator kindOfLinkingToInitiator, ulong parentProcessId, bool isProxy)
        {
#if DEBUG
            //LogInstance.Log($"NPCProcessHelpers RegProcess npcProcess.Id = {npcProcess.Id}");
            //LogInstance.Log($"NPCProcessHelpers RegProcess startupMode = {startupMode}");
            //LogInstance.Log($"NPCProcessHelpers RegProcess kindOfLinkingToInitiator = {kindOfLinkingToInitiator}");
            //LogInstance.Log($"NPCProcessHelpers RegProcess parentProcessId = {parentProcessId}");
            //LogInstance.Log($"NPCProcessHelpers RegProcess isProxy = {isProxy}");
#endif

            var targetStartupMode = NPCProcessStartupMode.NewInstance;

            switch(startupMode)
            {
                case NPCProcessStartupMode.NewInstance:
                    if(parentProcessId == 0ul)
                    {
                        targetStartupMode = NPCProcessStartupMode.NewStandaloneInstance;
                        break;
                    }
                    targetStartupMode = NPCProcessStartupMode.NewInstance;
                    break;

                case NPCProcessStartupMode.NewStandaloneInstance:
                    targetStartupMode = NPCProcessStartupMode.NewStandaloneInstance;
                    break;

                case NPCProcessStartupMode.Singleton:
                    if(isProxy)
                    {
                        targetStartupMode = NPCProcessStartupMode.NewStandaloneInstance;
                        break;
                    }
                    targetStartupMode = NPCProcessStartupMode.Singleton;
                    break;
            }

#if DEBUG
            //LogInstance.Log($"NPCProcessHelpers RegProcess targetStartupMode = {targetStartupMode}");
#endif

            switch (targetStartupMode)
            {
                case NPCProcessStartupMode.NewInstance:
                    context.RegProcess(npcProcess, parentProcessId);
                    break;

                case NPCProcessStartupMode.NewStandaloneInstance:
                    context.RegProcess(npcProcess, 0ul);
                    break;

                case NPCProcessStartupMode.Singleton:
                    context.RegProcess(npcProcess, 0ul);
                    break;
            }
        }

        public static void UnRegProcess(INPCContext context, INPCProcess npcProcess)
        {
            var startupMode = npcProcess.StartupMode;

            switch (startupMode)
            {
                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:
                    context.UnRegProcess(npcProcess);
                    break;

                case NPCProcessStartupMode.Singleton:
                    break;
            }
        }
    }
}
