using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestedNPCBodyHost : INPCBodyHost
    {
        public TestedNPCBodyHost(IInternalHumanoidHostContext intenalHostContext, IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mInternalHumanoidHostContext = intenalHostContext;
            mInternalBodyHumanoidHost = internalBodyHumanoidHost;

            mInternalBodyHumanoidHost.OnReady += MInternalBodyHumanoidHost_OnReady;
            mInternalBodyHumanoidHost.OnDie += MInternalBodyHumanoidHost_OnDie;

            mInternalBodyHumanoidHost.SetInternalHumanoidHostContext(intenalHostContext);
            mStates = new ProxyForStatesOfHumanoidBodyHost(mInternalBodyHumanoidHost);
            mInternalBodyHumanoidHost.OnHumanoidStatesChanged += InternalOnHumanoidStatesChanged;
        }

        private void MInternalBodyHumanoidHost_OnReady()
        {
#if DEBUG
            LogInstance.Log("TestedNPCBodyHost MInternalBodyHumanoidHost_OnReady");
#endif
            Task.Run(() => {
                try
                {
                    OnReady?.Invoke();
                }catch(Exception e)
                {
#if DEBUG
                    LogInstance.Log($"TestedNPCBodyHost MInternalBodyHumanoidHost_OnReady e = {e}");
#endif
                }
            });
        }

        public event Action OnDie;

        private void MInternalBodyHumanoidHost_OnDie()
        {
            Task.Run(() => {
                try
                {
                    OnDie?.Invoke();
                }
                catch(Exception e)
                {
#if DEBUG
                    LogInstance.Log($"TestedNPCBodyHost MInternalBodyHumanoidHost_OnDie e = {e}");
#endif
                }            
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

            Task.Run(() => {
                try
                {
                    OnHumanoidStatesChanged?.Invoke(result);
                }
                catch (Exception e)
                {
#if DEBUG
                    LogInstance.Log($"TestedNPCBodyHost InternalOnHumanoidStatesChanged e = {e}");
#endif
                }
            });
                
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        public void Execute(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            LogInstance.Log($"TestedNPCBodyHost Execute targetState = {targetState}");
#endif

            var internalTargetStateOfHumanoidBody = InternalTargetStateOfHumanoidControllerConverter.ConvertToInternal(targetState);

#if DEBUG
            LogInstance.Log($"TestedNPCBodyHost ExecuteAsync internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
#endif

            mInternalBodyHumanoidHost.Execute(internalTargetStateOfHumanoidBody);

#if DEBUG
            LogInstance.Log($"TestedNPCBodyHost End ExecuteAsync internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
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

        public bool IsReady => mInternalBodyHumanoidHost.IsReady;
        public event Action OnReady;
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
            LogInstance.Log($"Begin TestedNPCHandHost Send command = {command}");

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
#if DEBUG
            LogInstance.Log($"TestedNPCHandHost Get propertyName = {propertyName}");
#endif
            if (mInternalHumanoidHostContext.RightHandThing != null)
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
            mInternalBodyHumanoidHost = internalBodyHumanoidHost;
            mInternalHumanoidHostContext = new InternalHumanoidHostContext();

            mBodyHost = new TestedNPCBodyHost(mInternalHumanoidHostContext, internalBodyHumanoidHost);
            mBodyHost.OnReady += MBodyHost_OnReady;

            mRightHandHost = new TestedNPCHandHost(mInternalHumanoidHostContext);
            mLeftHandHost = new TestedNPCHandHost(mInternalHumanoidHostContext);
        }

        private void MBodyHost_OnReady()
        {
#if DEBUG
            LogInstance.Log("TestedNPCHostContext MBodyHost_OnReady");
#endif

            Task.Run(() => {
                try
                {
                    OnReady?.Invoke();
                }catch(Exception e)
                {
#if DEBUG
                    LogInstance.Log($"TestedNPCHostContext MBodyHost_OnReady e = {e}");
#endif
                }
            });       
        }

        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;
        private InternalHumanoidHostContext mInternalHumanoidHostContext;
        private TestedNPCBodyHost mBodyHost;
        private TestedNPCHandHost mRightHandHost;
        private TestedNPCHandHost mLeftHandHost;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
        public ILogicalStorage HostLogicalStorage => mInternalBodyHumanoidHost.HostLogicalStorage;
        public ulong SelfEntityId => mInternalBodyHumanoidHost.SelfEntityId;
        public bool IsReady => mBodyHost.IsReady;
        public event Action OnReady;

        public IList<IHostVisionObject> VisibleObjects => mInternalBodyHumanoidHost.VisibleObjects;
        public Vector3 GlobalPosition => mInternalBodyHumanoidHost.GlobalPosition;
    }
}
