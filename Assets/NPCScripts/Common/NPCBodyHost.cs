//using Assets.Scripts;
//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Common
//{
//    public class NPCBodyHost : INPCBodyHost
//    {
//        public NPCBodyHost(IEntityLogger entityLogger, IInternalHumanoidHostContext intenalHostContext, IInternalBodyHumanoidHost internalBodyHumanoidHost)
//        {
//            mEntityLogger = entityLogger;
//            mInternalHumanoidHostContext = intenalHostContext;
//            mInternalBodyHumanoidHost = internalBodyHumanoidHost;

//            mInternalBodyHumanoidHost.OnReady += MInternalBodyHumanoidHost_OnReady;
//            mInternalBodyHumanoidHost.OnDie += MInternalBodyHumanoidHost_OnDie;

//            mInternalBodyHumanoidHost.SetInternalHumanoidHostContext(intenalHostContext);
//            mStates = new ProxyForStatesOfHumanoidBodyHost(mInternalBodyHumanoidHost);
//            mInternalBodyHumanoidHost.OnHumanoidStatesChanged += InternalOnHumanoidStatesChanged;
//        }

//        private IEntityLogger mEntityLogger;

//        [MethodForLoggingSupport]
//        protected void Log(string message)
//        {
//            mEntityLogger?.Log(message);
//        }

//        [MethodForLoggingSupport]
//        protected void Error(string message)
//        {
//            mEntityLogger?.Error(message);
//        }

//        [MethodForLoggingSupport]
//        protected void Warning(string message)
//        {
//            mEntityLogger?.Warning(message);
//        }

//        private void MInternalBodyHumanoidHost_OnReady()
//        {
//#if DEBUG
//            //Log("Begin");
//#endif
//            Task.Run(() => {
//                try
//                {
//                    mOnReady?.Invoke();
//                }
//                catch (Exception e)
//                {
//#if DEBUG
//                    Error($"e = {e}");
//#endif
//                }
//            });
//        }

//        public event Action OnDie;

//        private void MInternalBodyHumanoidHost_OnDie()
//        {
//            Task.Run(() => {
//                try
//                {
//                    OnDie?.Invoke();
//                }
//                catch (Exception e)
//                {
//#if DEBUG
//                    Error($"e = {e}");
//#endif
//                }
//            });
//        }

//        private IInternalHumanoidHostContext mInternalHumanoidHostContext;
//        private IStatesOfHumanoidBodyHost mStates;
//        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;

//        public IStatesOfHumanoidBodyHost States
//        {
//            get
//            {
//                return mStates;
//            }
//        }

//        private void InternalOnHumanoidStatesChanged(IList<HumanoidStateKind> changedStates)
//        {
//            var result = new List<HumanoidStateKind>();

//            foreach (var initItem in changedStates)
//            {
//                result.Add(initItem);
//            }

//            Task.Run(() => {
//                try
//                {
//                    OnHumanoidStatesChanged?.Invoke(result);
//                }
//                catch (Exception e)
//                {
//#if DEBUG
//                    Error($"e = {e}");
//#endif
//                }
//            });

//        }

//        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

//        public void Execute(TargetStateOfHumanoidBody targetState)
//        {
//#if DEBUG
//            //Log($"targetState = {targetState}");
//#endif

//            var internalTargetStateOfHumanoidBody = InternalTargetStateOfHumanoidControllerConverter.ConvertToInternal(targetState);

//#if DEBUG
//            //Log($"internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
//#endif

//            mInternalBodyHumanoidHost.Execute(internalTargetStateOfHumanoidBody);

//#if DEBUG
//            //Log($"End internalTargetStateOfHumanoidBody = {internalTargetStateOfHumanoidBody}");
//#endif
//        }

//        public void CallInMainUI(Action function)
//        {
//            mInternalBodyHumanoidHost.CallInMainUI(function);
//        }

//        public TResult CallInMainUI<TResult>(Func<TResult> function)
//        {
//            return mInternalBodyHumanoidHost.CallInMainUI(function);
//        }

//        public bool IsReady => mInternalBodyHumanoidHost.IsReady;
//        private event Action mOnReady;
//        public event Action OnReady
//        {
//            add
//            {
//                mOnReady += value;
//                if (mInternalBodyHumanoidHost.IsReady)
//                {
//                    Task.Run(() => {
//                        try
//                        {
//                            value();
//                        }
//                        catch (Exception e)
//                        {
//#if DEBUG
//                            Error($"e = {e}");
//#endif
//                        }
//                    });
//                }
//            }

//            remove
//            {
//                mOnReady -= value;
//            }
//        }
//    }
//}
