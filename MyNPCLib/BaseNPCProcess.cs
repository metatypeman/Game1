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
        public override Task Task
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        #region private members
        private ulong mId;
        private ActivatorOfNPCProcessEntryPointInfo mActivator = new ActivatorOfNPCProcessEntryPointInfo();
        private List<ProxyForNPCAbstractProcess> mListOfProxes = new List<ProxyForNPCAbstractProcess>();
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
            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Context.Send(command);
        }

        public INPCProcess ExecuteAsChild(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteAsChild command = {command}");
#endif
            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.KindOfLinkingToInitiator = KindOfLinkingToInitiator.Child;
            command.InitiatingProcessId = Id;

            return Context.Send(command);
        }

        public INPCProcess ExecuteBody(HumanoidBodyCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteBody command = {command}");
#endif

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            return Context.Body.Send(command);
        }

        public INPCProcess ExecuteDefaultHand(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteDefaultHand command = {command}");
#endif

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            return Context.DefaultHand.Send(command);
        }

        public object GetDefaultHandProperty(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetDefaultHandProperty propertyName = {propertyName}");
#endif

            return Context.DefaultHand.Get(propertyName);
        }

        public T GetDefaultHandProperty<T>(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetDefaultHandProperty<T> propertyName = {propertyName}");
#endif

            try
            {
                return (T)Context.DefaultHand.Get(propertyName);
            }
            catch
            {
            }

            return default(T);
        }

        public INPCProcess ExecuteRightHand(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteRightHand command = {command}");
#endif

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            return Context.RightHand.Send(command);
        }

        public object GetRightHandProperty(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetRightHandProperty propertyName = {propertyName}");
#endif

            return Context.RightHand.Get(propertyName);
        }

        public T GetRightHandProperty<T>(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetRightHandProperty<T> propertyName = {propertyName}");
#endif

            try
            {
                return (T)Context.RightHand.Get(propertyName);
            }
            catch
            {
            }

            return default(T);
        }

        public INPCProcess ExecuteLeftHand(NPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess ExecuteLeftHand command = {command}");
#endif

            StateChecker();

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.InitiatingProcessId = Id;

            return Context.LeftHand.Send(command);
        }

        public object GetLeftHandProperty(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetLeftHandProperty propertyName = {propertyName}");
#endif

            return Context.LeftHand.Get(propertyName);
        }

        public T GetLeftHandProperty<T>(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCProcess GetLeftHandProperty<T> propertyName = {propertyName}");
#endif
            try
            {
                return (T)Context.LeftHand.Get(propertyName);
            }
            catch
            {
            }

            return default(T);
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

            var cs = new CancellationTokenSource();
            var token = cs.Token;

            var proxy = new ProxyForNPCAbstractProcess(mId, Context);
            proxy.LocalPriority = command.Priority;

            var task = new Task(() => {
                NRun(entryPointInfo, command, proxy);
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

        private void NRun(NPCProcessEntryPointInfo entryPointInfo, NPCInternalCommand command, ProxyForNPCAbstractProcess proxy)
        {
#if DEBUG
            //LogInstance.Log($"Begin BaseNPCProcess NRun command = {command}");
#endif

            var startupMode = Info.StartupMode;

            try
            {
                lock(mListOfProxesLockObj)
                {
                    mListOfProxes.Add(proxy);
                }

                NPCProcessHelpers.RegProcess(Context, proxy, startupMode, command.KindOfLinkingToInitiator, command.InitiatingProcessId, true);

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

            lock (mListOfProxesLockObj)
            {
                mListOfProxes.Remove(proxy);
            }

#if DEBUG
            //LogInstance.Log($"End BaseNPCProcess NRun command = {command}");
#endif
        }

        public void Wait(params INPCProcess[] processes)
        {
            var tasksArray = processes.Where(p => p.Task != null).Select(p => p.Task).ToArray();

            Task.WaitAll(tasksArray);
        }

        public void Wait()
        {
            while(true)
            {
                lock (StateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        break;
                    }
                }

                Thread.Sleep(1000);
            }
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
