using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContext: INPCContext
    {
        public BaseNPCContext()
        {

        }

        #region private members
        private IdFactory mIdFactory = new IdFactory();
        #endregion

        public INPCResourcesManager Body { get; }
        public INPCResourcesManager DefaultHand { get; }
        public INPCResourcesManager LeftHand { get; }
        public INPCResourcesManager RightHand { get; }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCContext Send command = {command}");
#endif

            return null;
        }
    }
}
