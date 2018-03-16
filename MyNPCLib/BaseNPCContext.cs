using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContext: INPCContext
    {
        public BaseNPCContext()
        {
            mIdFactory = new IdFactory();
            mBodyResourcesManager = new NPCBodyResourcesManager(mIdFactory);
            mLeftHandResourcesManager = new NPCHandResourcesManager(mIdFactory);
            mRightHandResourcesManager = new NPCHandResourcesManager(mIdFactory);
        }

        #region private members
        private IdFactory mIdFactory;
        private NPCBodyResourcesManager mBodyResourcesManager;
        private NPCHandResourcesManager mLeftHandResourcesManager;
        private NPCHandResourcesManager mRightHandResourcesManager;
        #endregion

        public INPCResourcesManager Body => mBodyResourcesManager;
        public INPCResourcesManager DefaultHand => mRightHandResourcesManager;
        public INPCResourcesManager LeftHand => mLeftHandResourcesManager;
        public INPCResourcesManager RightHand => mRightHandResourcesManager;

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCContext Send command = {command}");
#endif

            return null;
        }
    }
}
