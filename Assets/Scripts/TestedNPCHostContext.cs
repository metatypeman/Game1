using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestedNPCBodyHost : INPCBodyHost
    {
        public TestedNPCBodyHost(IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mInternalBodyHumanoidHost = internalBodyHumanoidHost;
            mStates = new ProxyForStatesOfHumanoidBodyHost(mInternalBodyHumanoidHost.States);
        }

        private IStatesOfHumanoidBodyHost mStates;
        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;

        public IStatesOfHumanoidBodyHost States
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
            Debug.Log($"ExecuteAsync targetState = {targetState}");
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

    public class TestedNPCHandHost : INPCHandHost
    {
        public INPCProcess Send(INPCCommand command)
        {
            Debug.Log($"Begin Send command = {command}");

            var process = new NPCThingProcess();
            process.State = StateOfNPCProcess.Running;

            Task.Run(() => {
                Debug.Log($"Begin Send Task.Run command = {command}");

                process.State = StateOfNPCProcess.Running;

                Thread.Sleep(1000);

                Debug.Log($"End Send Task.Run command = {command}");
            });

            Debug.Log($"End Send command = {command}");

            return process;
        }

        public object Get(string propertyName)
        {
            Debug.Log($"Get propertyName = {propertyName}");

            return "The Beatles!!!";
        }
    }

    public class TestedNPCHostContext: INPCHostContext
    {
        public TestedNPCHostContext(IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mBodyHost = new TestedNPCBodyHost(internalBodyHumanoidHost);
            mRightHandHost = new TestedNPCHandHost();
            mLeftHandHost = new TestedNPCHandHost();
        }

        private TestedNPCBodyHost mBodyHost;
        private TestedNPCHandHost mRightHandHost;
        private TestedNPCHandHost mLeftHandHost;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
    }
}
