using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private ActivatorOfNPCProcessEntryPointInfo mActivator = new ActivatorOfNPCProcessEntryPointInfo();
        private List<BaseCommonNPCProcess> mListOfProxes = new List<BaseCommonNPCProcess>();
        private object mListOfProxesLockObj = new object();
        #endregion

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
            LogInstance.Log($"BaseNPCProcess Execute command = {command}");
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
            LogInstance.Log($"BaseNPCProcess ExecuteAsChild command = {command}");
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
            LogInstance.Log($"BaseNPCProcess ExecuteBody command = {command}");
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
            LogInstance.Log($"BaseNPCProcess ExecuteDefaultHand command = {command}");
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
            LogInstance.Log($"BaseNPCProcess GetDefaultHandProperty propertyName = {propertyName}");
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
            LogInstance.Log($"BaseNPCProcess GetDefaultHandProperty<T> propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.DefaultHand.Get(propertyName);
            }
            catch
            {
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteRightHand(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteRightHand command = {command}");
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
            LogInstance.Log($"BaseNPCProcess GetRightHandProperty propertyName = {propertyName}");
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
            LogInstance.Log($"BaseNPCProcess GetRightHandProperty<T> propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.RightHand.Get(propertyName);
            }
            catch
            {
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess ExecuteLeftHand(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteLeftHand command = {command}");
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
            LogInstance.Log($"BaseNPCProcess GetLeftHandProperty propertyName = {propertyName}");
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
            LogInstance.Log($"BaseNPCProcess GetLeftHandProperty<T> propertyName = {propertyName}");
#endif
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            T result = default(T);

            try
            {
                result = (T)Context.LeftHand.Get(propertyName);
            }
            catch
            {
            }

            cancelationToken?.ThrowIfCancellationRequested();

            return result;
        }

        public INPCProcess RunAsync(NPCInternalCommand command, NPCProcessEntryPointInfo entryPointInfo)
        {
#if DEBUG
            //LogInstance.Log($"Begin BaseNPCProcess RunAsync command = {command}");
            //LogInstance.Log($"Begin BaseNPCProcess RunAsync LocalPriority = {LocalPriority}");
            //LogInstance.Log($"Begin BaseNPCProcess RunAsync entryPointInfo = {entryPointInfo}");
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
                    proxy = new ProxyForNPCAbstractProcess(mId, Context);
                    proxy.StartupMode = StartupMode;
#if DEBUG

                    LogInstance.Log($"End BaseNPCProcess RunAsync (1) proxy.StartupMode = {proxy.StartupMode}");
#endif
                    break;

                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:              
                    proxy = this;
#if DEBUG
                    LogInstance.Log($"End BaseNPCProcess RunAsync (2) proxy.StartupMode = {proxy.StartupMode}");
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
            //LogInstance.Log($"End BaseNPCProcess RunAsync command = {command}");
            //LogInstance.Log($"End BaseNPCProcess RunAsync proxy.LocalPriority = {proxy.LocalPriority}");
#endif

            return proxy;
        }

        private void NRun(NPCProcessEntryPointInfo entryPointInfo, NPCInternalCommand command, BaseCommonNPCProcess proxy, CancellationToken cancellationToken)
        {
#if DEBUG
            //LogInstance.Log($"Begin BaseNPCProcess NRun command = {command}");
#endif
            cancellationToken.ThrowIfCancellationRequested();

            var startupMode = Info.StartupMode;

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

                NPCProcessHelpers.RegProcess(Context, proxy, startupMode, command.KindOfLinkingToInitiator, command.InitiatingProcessId, true);

                cancellationToken.ThrowIfCancellationRequested();

                proxy.State = StateOfNPCProcess.Running;

                mActivator.CallEntryPoint(this, entryPointInfo, command.Params);

                proxy.State = StateOfNPCProcess.RanToCompletion;
            }
            catch (OperationCanceledException)
            {
#if DEBUG
                LogInstance.Log("BaseNPCProcess NRun catch(OperationCanceledException)");
#endif
            }
            catch (Exception e)
            {
                proxy.State = StateOfNPCProcess.Faulted;

#if DEBUG
                LogInstance.Log($"End BaseNPCProcess NRun e = {e}");
#endif
            }
            finally
            {
                var taskId = Task.CurrentId;

                Context.UnRegCancellationToken(taskId.Value);
            }

            if (proxy != this)
            {
                lock (mListOfProxesLockObj)
                {
                    mListOfProxes.Remove(proxy);
                }
            }

#if DEBUG
            //LogInstance.Log($"End BaseNPCProcess NRun command = {command}");
#endif
        }

        public void Wait(params INPCProcess[] processes)
        {
            var cancelationToken = GetCancellationToken();
            cancelationToken?.ThrowIfCancellationRequested();

            var tasksArray = processes.Where(p => p.Task != null).Select(p => p.Task).ToArray();

            cancelationToken?.ThrowIfCancellationRequested();

            Task.WaitAll(tasksArray);

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
            LogInstance.Log("BaseNPCProcess AddChildComponent");
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
            LogInstance.Log("BaseNPCProcess RemoveChildComponent");
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
            var trigger = new BaseTrigger(predicate, timeout);
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
            //LogInstance.Log("BaseNPCProcess Dispose");
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
