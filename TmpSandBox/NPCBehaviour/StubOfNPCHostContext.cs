﻿using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TmpSandBox.NPCBehaviour
{
    public class StubOfNPCBodyHost: INPCBodyHost
    {
        private StatesOfHumanoidBodyController mStates = new StatesOfHumanoidBodyController();

        public IStatesOfHumanoidBodyHost States
        {
            get
            {
                return mStates;
            }
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        public event Action OnDie;

        //private object mLockObj { get; set; } = new object();
        //private HumanoidTaskOfExecuting mTargetStateForExecuting { get; set; }

        public void Execute(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"ExecuteAsync targetState = {targetState}");
#endif

            Thread.Sleep(1000);
        }
    }

    public class StubOfNPCHandHost: INPCHandHost
    {
        public INPCProcess Send(INPCCommand command)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Beging Send command = {command}");

            var process = new NPCThingProcess();
            process.State = StateOfNPCProcess.Running;

            Task.Run(() => {
                NLog.LogManager.GetCurrentClassLogger().Info($"Beging Send Task.Run command = {command}");

                process.State = StateOfNPCProcess.Running;

                Thread.Sleep(1000);

                process.State = StateOfNPCProcess.RanToCompletion;

                NLog.LogManager.GetCurrentClassLogger().Info($"End Send Task.Run command = {command}");
            });

            NLog.LogManager.GetCurrentClassLogger().Info($"End Send command = {command}");

            return process;
        }

        public object Get(string propertyName)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Get propertyName = {propertyName}");

            return "The Beatles!!!";
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
