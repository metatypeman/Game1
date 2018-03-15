using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCHandResourcesManager: INPCResourcesManager
    {
        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Send command = {command}");
#endif

            return null;
        }
    }
}
