using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class NPCProcessHelpers
    {
        public static void RegProcess(INPCContext context, INPCProcess npcProcess, NPCProcessStartupMode startupMode, KindOfLinkingToInitiator kindOfLinkingToInitiator)
        {
#if DEBUG
            LogInstance.Log($"NPCProcessHelpers RegProcess npcProcess.Id = {npcProcess.Id}");
            LogInstance.Log($"NPCProcessHelpers RegProcess startupMode = {startupMode}");
            LogInstance.Log($"NPCProcessHelpers RegProcess kindOfLinkingToInitiator = {kindOfLinkingToInitiator}");
#endif

            d

            /*
                            switch (startupMode)
                {
                    case NPCProcessStartupMode.NewInstance:
                        Context.RegProcess(this, command.InitiatingProcessId);
                        break;

                    case NPCProcessStartupMode.NewStandaloneInstance:
                        Context.RegProcess(this, 0ul);
                        break;

                    case NPCProcessStartupMode.Singleton:
                        lock(mIsFirstCallLockObj)
                        {
                            if(!mIsFirstCall)
                            {
                                mIsFirstCall = true;
                                Context.RegProcess(this, 0ul);
                            }
                        }
                        break;
                } 
            */
        }

        public static void UnRegProcess(INPCContext context, INPCProcess npcProcess)
        {
            /*            switch (startupMode)
            {
                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:
                    Context.UnRegProcess(this);
                    break;

                case NPCProcessStartupMode.Singleton:
                    break;
            }*/
            f
        }
    }
}
