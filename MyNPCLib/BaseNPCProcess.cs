using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseNPCProcess : BaseCommonNPCProcess
    {
        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;

        #region private members
        private ulong mId;
        private ActivatorOfNPCProcessEntryPointInfo mActivator = new ActivatorOfNPCProcessEntryPointInfo(null);
        private List<BaseCommonNPCProcess> mListOfProxes = new List<BaseCommonNPCProcess>();
        private object mListOfProxesLockObj = new object();
        #endregion

        protected override void OnSetEntityLogger()
        {
            base.OnSetEntityLogger();
            mActivator.EntityLogger = EntityLogger;
        }

        public override StateOfNPCProcess State
        {
            get
            {
                lock(StateLockObj)
                {
                    return mState;
                }
            }

            set
            {
                lock (StateLockObj)
                {
                    NState = value;
                }
            }
        }

        private StateOfNPCProcess NState
        {
            set
            {
                if (!StateTransitionChecker(mState, value))
                {
                    return;
                }

                mState = value;

                EmitChangingOfState(mState);
            }
        }

        public override ulong Id
        {
            get
            {
                return mId;
            }

            set
            {
                StateChecker();

                mId = value;
            }
        }

        private NPCProcessInfo mNPCProcessInfo;

        public NPCProcessInfo Info
        {
            get
            {
                return mNPCProcessInfo;
            }

            set
            {
                mNPCProcessInfo = value;
            }
        }

        protected INPCProcess Execute(NPCCommand command)
        {
#if DEBUG
           //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteAsChild(NPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.KindOfLinkingToInitiator = KindOfLinkingToInitiator.Child;
            command.InitiatingProcessId = Id;

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteBody(HumanoidBodyCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.Body.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteDefaultHand(NPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.DefaultHand.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public object GetDefaultHandProperty(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.DefaultHand.Get(propertyName);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public T GetDefaultHandProperty<T>(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.DefaultHand.Get(propertyName);
            }
            catch(Exception e)
            {
#if DEBUG
                Error($"propertyName = {propertyName} e = {e}");
#endif
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteRightHand(NPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.RightHand.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public object GetRightHandProperty(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.RightHand.Get(propertyName);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public T GetRightHandProperty<T>(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.RightHand.Get(propertyName);
            }
            catch(Exception e)
            {
#if DEBUG
                Error($"propertyName = {propertyName} e = {e}");
#endif
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteLeftHand(NPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.LeftHand.Send(command);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public object GetLeftHandProperty(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.LeftHand.Get(propertyName);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public T GetLeftHandProperty<T>(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.LeftHand.Get(propertyName);
            }
            catch(Exception e)
            {
#if DEBUG
                LogInstance.Error($"propertyName = {propertyName} e = {e}");
#endif
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess RunAsync(NPCInternalCommand command, NPCProcessEntryPointInfo entryPointInfo)
        {
#if DEBUG
            //Log($"Begin command = {command}");
            //Log($"Begin LocalPriority = {LocalPriority}");
            //Log($"Begin entryPointInfo = {entryPointInfo}");
#endif

            StateChecker();

            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if(entryPointInfo == null)
            {
                throw new ArgumentNullException(nameof(entryPointInfo));
            }

            BaseCommonNPCProcess proxy = null;

            switch (StartupMode)
            {
                case NPCProcessStartupMode.Singleton:
                    proxy = new ProxyForNPCAbstractProcess(EntityLogger, mId, Context);
                    proxy.StartupMode = StartupMode;
#if DEBUG

                    //Log($"End (1) proxy.StartupMode = {proxy.StartupMode} command = {command}");
#endif
                    break;

                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:              
                    proxy = this;
#if DEBUG
                    //Log($"End (2) proxy.StartupMode = {proxy.StartupMode} command = {command}");
#endif
                    break;
            }
        
            proxy.LocalPriority = command.Priority;

            var cs = new CancellationTokenSource();

            proxy.CancellationToken = cs;

            var token = cs.Token;

            var task = new Task(() => {
                NRun(entryPointInfo, command, proxy, token);
            }, token);

            proxy.Task = task;

            var taskId = task.Id;

            Context.RegCancellationToken(taskId, token);

            task.Start();

#if DEBUG
            //Log($"End command = {command}");
            //Log($"End proxy.LocalPriority = {proxy.LocalPriority}");
#endif

            return proxy;
        }

        private void NRun(NPCProcessEntryPointInfo entryPointInfo, NPCInternalCommand command, BaseCommonNPCProcess proxy, CancellationToken cancellationToken)
        {
#if DEBUG
            //Log($"Begin proxy.Id = {proxy.Id} proxy.State = {proxy.State} command = {command}");
#endif
            cancellationToken.ThrowIfCancellationRequested();

            var startupMode = Info.StartupMode;

#if DEBUG
            //Log($"proxy.Id = {proxy.Id} proxy.State = {proxy.State} startupMode = {startupMode}");
#endif

            try
            {
                if(proxy != this)
                {
                    lock (mListOfProxesLockObj)
                    {
                        mListOfProxes.Add(proxy);
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();

#if DEBUG
                //Log($"proxy.Id = {proxy.Id} proxy.State = {proxy.State} Step 1");
#endif

                NPCProcessHelpers.RegProcess(Context, proxy, startupMode, command.KindOfLinkingToInitiator, command.InitiatingProcessId, true);

#if DEBUG
                //Log($"proxy.Id = {proxy.Id} proxy.State = {proxy.State} Step 2");
#endif

                cancellationToken.ThrowIfCancellationRequested();

                proxy.State = StateOfNPCProcess.Running;

#if DEBUG
                //Log($"proxy.Id = {proxy.Id} proxy.State = {proxy.State} Step 3");
#endif

                mActivator.CallEntryPoint(this, entryPointInfo, command.Params);

#if DEBUG
                //Log($"proxy.Id = {proxy.Id} proxy.State = {proxy.State} Step 4");
#endif

                proxy.State = StateOfNPCProcess.RanToCompletion;
            }
            //catch (TargetInvocationException)
            //{
            //}
            catch (OperationCanceledException)
            {
#if DEBUG
                //Error("catch(OperationCanceledException)");
#endif
            }
            catch (Exception e)
            {
                proxy.State = StateOfNPCProcess.Faulted;

#if DEBUG
                Error($"End proxy.Id = {proxy.Id} e = {e}");
#endif
            }
            finally
            {
#if DEBUG
                //Log($"Begin finally proxy.Id = {proxy.Id} proxy.State = {proxy.State}");
#endif

                var taskId = Task.CurrentId;

                Context.UnRegCancellationToken(taskId.Value);

#if DEBUG
                //Log($"End finally proxy.Id = {proxy.Id} proxy.State = {proxy.State}");
#endif
            }

            if (proxy != this)
            {
                lock (mListOfProxesLockObj)
                {
                    mListOfProxes.Remove(proxy);
                }
            }

#if DEBUG
            //Log($"End proxy.Id = {proxy.Id} proxy.State = {proxy.State} command = {command}");
#endif
        }

        public void Wait(params INPCProcess[] processes)
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            //var tasksArray = processes.Where(p => p.Task != null).Select(p => p.Task).ToArray();

            //cancelationToken?.ThrowIfCancellationRequested();

            //Task.WaitAll(tasksArray);

            //cancelationToken?.ThrowIfCancellationRequested();

            var tasksList = processes.ToList();

            while (true)
            {
#if DEBUG
                //var sb = new StringBuilder();
                //sb.Append(tasksList.Count);
                //foreach (var tmpT in tasksList)
                //{
                //    sb.Append($"Id = {tmpT.Id}; State = {tmpT.State}");
                //}
                //Log($" ------- {sb.ToString()}");
#endif

                lock (StateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        break;
                    }
                }

                if(!tasksList.Any(p => p.State == StateOfNPCProcess.Created || p.State == StateOfNPCProcess.Running))
                {
                    break;
                }

                cancelationToken?.ThrowIfCancellationRequested();

                Thread.Sleep(10);

#if DEBUG
                //Log("!!!!!!!!");
#endif
            }

            cancelationToken?.ThrowIfCancellationRequested();
        }

        public void Wait()
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            while (true)
            {
                lock (StateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        break;
                    }
                }

                cancelationToken?.ThrowIfCancellationRequested();

                Thread.Sleep(1000);
            }
        }

        public void Wait(int millisecondsTimeout)
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            Thread.Sleep(millisecondsTimeout);

            cancelationToken?.ThrowIfCancellationRequested();
        }

        protected bool InfinityCondition
        {
            get
            {
                lock (StateLockObj)
                {
                    if (mState != StateOfNPCProcess.Running)
                    {
                        return false;
                    }
                }

                TryAsCancel();
                return true;
            }
        }

        protected void CallInMainUI(Action function)
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            Context.CallInMainUI(function);

            cancelationToken?.ThrowIfCancellationRequested();
        }

        protected TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            var result = Context.CallInMainUI(function);

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public void AddChildComponent(IChildComponentOfNPCProcess component)
        {
#if DEBUG
            //Log("Begin");
#endif

            lock (StateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }
            }

            lock (mChildrenComponentsLockObj)
            {
                if(mChildrenComponentsList.Contains(component))
                {
                    return;
                }

                mChildrenComponentsList.Add(component);
            }
        }

        public void RemoveChildComponent(IChildComponentOfNPCProcess component)
        {
#if DEBUG
            //Log("Begin");
#endif
            lock (StateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }
            }

            lock (mChildrenComponentsLockObj)
            {
                if(!mChildrenComponentsList.Contains(component))
                {
                    return;
                }

                mChildrenComponentsList.Remove(component);
            }
        }

        private readonly object mChildrenComponentsLockObj = new object();
        private List<IChildComponentOfNPCProcess> mChildrenComponentsList = new List<IChildComponentOfNPCProcess>();

        public ITrigger CreateTrigger(PredicateOfTrigger predicate, int timeout = 1000)
        {
            var trigger = new BaseTrigger(EntityLogger, predicate, timeout);
            AddChildComponent(trigger);
            trigger.Start();
            return trigger;
        }

        public BaseAbstractLogicalObject GetLogicalObject(string query)
        {
            return Context.GetLogicalObject(query);
        }

        public override void Dispose()
        {
#if DEBUG
            //Log("Begin");
#endif
            lock (StateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                NState = StateOfNPCProcess.Destroyed;
            }

            lock (mListOfProxesLockObj)
            {
                foreach(var proxy in mListOfProxes)
                {
                    proxy.State = StateOfNPCProcess.Destroyed;
                }
            }

            Context.UnRegProcess(this);

            lock (mChildrenComponentsLockObj)
            {
                foreach(var childItem in mChildrenComponentsList)
                {
                    childItem.Dispose();
                }

                mChildrenComponentsList.Clear();
            }
        }
    }
}
