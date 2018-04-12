﻿using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class StubOfNPCBodyHost: INPCBodyHost
    {
        private StatesOfHumanoidBodyController mStates = new StatesOfHumanoidBodyController();

        public StatesOfHumanoidBodyController States
        {
            get
            {
                return mStates;
            }
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        private object mLockObj = new object();
        private HumanoidTaskOfExecuting mTargetStateForExecuting;

        public HumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"ExecuteAsync targetState = {targetState}");
#endif

            lock (mLockObj)
            {
                if (mTargetStateForExecuting != null)
                {
                    mTargetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Canceled;
                }

                var targetStateForExecuting = new HumanoidTaskOfExecuting();
                targetStateForExecuting.ProcessedState = targetState;

                mTargetStateForExecuting = targetStateForExecuting;

#if DEBUG
                targetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Executed;//tmp
                //NLog.LogManager.GetCurrentClassLogger().Info($"ExecuteAsync mTargetStateQueue.Count = {mTargetStateQueue.Count}");
#endif

                return targetStateForExecuting;
            }
        }
    }

    public class StubOfNPCHandHost: INPCHandHost
    {
        public INPCProcess Send(INPCCommand command)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($" = {}");
        }

        public object Get(string propertyName)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($" = {}");
        }
    }

    public class StubOfNPCHostContext: INPCHostContext
    {
        public StubOfNPCHostContext()
        {
            mBodyHost = new StubOfNPCBodyHost();
            mRightHandHost = new StubOfNPCHandHost();
            mLeftHandHost = new StubOfNPCHandHost();
        }

        private StubOfNPCBodyHost mBodyHost;
        private StubOfNPCHandHost mRightHandHost;
        private StubOfNPCHandHost mLeftHandHost;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
    }
}
