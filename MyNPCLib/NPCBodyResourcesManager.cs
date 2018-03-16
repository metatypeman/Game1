using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCResourcesManager
    {
        public NPCBodyResourcesManager(IIdFactory idFactory)
        {
            mIdFactory = idFactory;
        }

#region private members
        private IIdFactory mIdFactory;
#endregion

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Send command = {command}");
#endif

            return null;
        }
    }
}
