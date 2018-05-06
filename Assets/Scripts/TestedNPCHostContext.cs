using MyNPCLib;
using MyNPCLib.Logical;
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

        private void InternalOnHumanoidStatesChanged(IList<HumanoidStateKind> changedStates)
        {
            var result = new List<HumanoidStateKind>();
            
            foreach(var initItem in changedStates)
            {
                result.Add(initItem);
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

            mInternalBodyHumanoidHost.Execute(internalTargetStateOfHumanoidBody);

#if DEBUG
            Debug.Log($"TestedNPCBodyHost End ExecuteAsync internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
#endif
        }
        
        public void CallInMainUI(Action function)
        {
            mInternalBodyHumanoidHost.CallInMainUI(function);
        }
        
        public TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            return mInternalBodyHumanoidHost.CallInMainUI(function);
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
            mHostLogicalStorage = internalBodyHumanoidHost.HostLogicalStorage;

#if DEBUG
            LogInstance.Log($"TestedNPCHostContext (mHostLogicalStorage == null) = {mHostLogicalStorage == null}");
#endif

            mSelfEntityId = internalBodyHumanoidHost.SelfEntityId;
        }

        private InternalHumanoidHostContext mInternalHumanoidHostContext;
        private TestedNPCBodyHost mBodyHost;
        private TestedNPCHandHost mRightHandHost;
        private TestedNPCHandHost mLeftHandHost;
        private ILogicalStorage mHostLogicalStorage;
        private ulong mSelfEntityId;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
        public ILogicalStorage HostLogicalStorage => mHostLogicalStorage;
        public ulong SelfEntityId => mSelfEntityId;
    }
}
