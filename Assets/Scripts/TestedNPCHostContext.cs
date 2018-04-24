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
        public TestedNPCBodyHost(IInternalHumanoidHostContext intenalHostContext, IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mInternalHumanoidHostContext = intenalHostContext;
            mInternalBodyHumanoidHost = internalBodyHumanoidHost;

            mInternalBodyHumanoidHost.OnDie += MInternalBodyHumanoidHost_OnDie;

            mInternalBodyHumanoidHost.SetInternalHumanoidHostContext(intenalHostContext);
            mStates = new ProxyForStatesOfHumanoidBodyHost(mInternalBodyHumanoidHost);
            mInternalBodyHumanoidHost.OnHumanoidStatesChanged += InternalOnHumanoidStatesChanged;
        }

        public event Action OnDie;

        private void MInternalBodyHumanoidHost_OnDie()
        {
            Task.Run(() => {
                OnDie?.Invoke();
            });
        }

        private IInternalHumanoidHostContext mInternalHumanoidHostContext;
        private IStatesOfHumanoidBodyHost mStates;
        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;

        public IStatesOfHumanoidBodyHost States
        {
            get
            {
                return mStates;
            }
        }

        private void InternalOnHumanoidStatesChanged(IList<InternalHumanoidStateKind> changedStates)
        {
            var result = new List<HumanoidStateKind>();
            
            foreach(var initItem in changedStates)
            {
                result.Add(InternalStatesConverter.HumanoidStateKindFromInternal(initItem));
            }

            OnHumanoidStatesChanged?.Invoke(result);
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        public void Execute(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            Debug.Log($"TestedNPCBodyHost Execute targetState = {targetState}");
#endif

            var internalTargetStateOfHumanoidBody = InternalTargetStateOfHumanoidControllerConverter.ConvertToInternal(targetState);

#if DEBUG
            Debug.Log($"TestedNPCBodyHost ExecuteAsync internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
#endif

            //var targetStateForExecuting = new HumanoidTaskOfExecuting();
            //targetStateForExecuting.ProcessedState = targetState;

            mInternalBodyHumanoidHost.Execute(internalTargetStateOfHumanoidBody);

            //var internalHumanoidTaskOfExecuting = mInternalBodyHumanoidHost.ExecuteAsync(internalTargetStateOfHumanoidBody);

            //while (internalHumanoidTaskOfExecuting.State == InternalStateOfHumanoidTaskOfExecuting.Created)
            //{
            //}

#if DEBUG
            Debug.Log($"TestedNPCBodyHost End ExecuteAsync internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
#endif

            //targetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Executed;
            //return targetStateForExecuting;
        }
    }

    public class TestedNPCHandHost : INPCHandHost
    {
        public TestedNPCHandHost(IInternalHumanoidHostContext intenalHostContext)
        {
            mInternalHumanoidHostContext = intenalHostContext;
        }

        private IInternalHumanoidHostContext mInternalHumanoidHostContext;

        public INPCProcess Send(INPCCommand command)
        {
            Debug.Log($"Begin TestedNPCHandHost Send command = {command}");

            if(mInternalHumanoidHostContext.RightHandThing != null)
            {
                return mInternalHumanoidHostContext.RightHandThing.Send(command);
            }

            var process = new NPCThingProcess();
            process.State = StateOfNPCProcess.Faulted;
            return process;

            //var process = new NPCThingProcess();
            //process.State = StateOfNPCProcess.Running;

            //Task.Run(() =>
            //{
            //    Debug.Log($"Begin TestedNPCHandHost Send Task.Run command = {command}");

            //    process.State = StateOfNPCProcess.Running;

            //    Thread.Sleep(1000);

            //    Debug.Log($"End TestedNPCHandHost Send Task.Run command = {command}");
            //});

            //Debug.Log($"End TestedNPCHandHost Send command = {command}");

            //return process;
        }

        public object Get(string propertyName)
        {
            Debug.Log($"TestedNPCHandHost Get propertyName = {propertyName}");

            if(mInternalHumanoidHostContext.RightHandThing != null)
            {
                return mInternalHumanoidHostContext.RightHandThing.Get(propertyName);
            }

            return null;
        }
    }

    public class TestedNPCHostContext: INPCHostContext
    {
        public TestedNPCHostContext(IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mInternalHumanoidHostContext = new InternalHumanoidHostContext();

            mBodyHost = new TestedNPCBodyHost(mInternalHumanoidHostContext, internalBodyHumanoidHost);
            mRightHandHost = new TestedNPCHandHost(mInternalHumanoidHostContext);
            mLeftHandHost = new TestedNPCHandHost(mInternalHumanoidHostContext);
        }

        private InternalHumanoidHostContext mInternalHumanoidHostContext;
        private TestedNPCBodyHost mBodyHost;
        private TestedNPCHandHost mRightHandHost;
        private TestedNPCHandHost mLeftHandHost;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
    }
}
