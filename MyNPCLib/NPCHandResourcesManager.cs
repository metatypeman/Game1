using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public enum KindOfHand
    {
        Right,
        Left
    }

    public class NPCHandResourcesManager: INPCResourcesManager
    {
        public NPCHandResourcesManager(IEntityLogger entityLogger, IIdFactory idFactory, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, KindOfHand kindOfHand, INPCContext context)
        {
            mEntityLogger = entityLogger;
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mContext = context;

            if(npcHostContext != null)
            {
                switch (kindOfHand)
                {
                    case KindOfHand.Right:
                        mNPCHandHost = npcHostContext.RightHandHost;
                        break;

                    case KindOfHand.Left:
                        mNPCHandHost = npcHostContext.LeftHandHost;
                        break;
                }
            }
        }

        #region private members
        private IEntityLogger mEntityLogger;
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private INPCHandHost mNPCHandHost;
        private INPCContext mContext;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        private readonly object mDataLockObj = new object();
        private List<ulong> mListOfProcessedId = new List<ulong>();
        private readonly Dictionary<ulong, ProxyForNPCResourceProcess> mProcessesDict = new Dictionary<ulong, ProxyForNPCResourceProcess>();
        #endregion

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        public void Bootstrap()
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                if (mState == StateOfNPCContext.Working)
                {
                    return;
                }

                mState = StateOfNPCContext.Working;
            }
        }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(mEntityLogger, id, mContext);
            process.LocalPriority = command.Priority;

            var cs = new CancellationTokenSource();

            process.CancellationToken = cs;

            var token = cs.Token;

            NPCProcessHelpers.RegProcess(mContext, process, NPCProcessStartupMode.NewInstance, command.KindOfLinkingToInitiator, command.InitiatingProcessId, true);

            var task = new Task(() => {
                NExecute(command, process);
            }, token);

            process.Task = task;

            var taskId = task.Id;

            mContext.RegCancellationToken(taskId, token);

            task.Start();

            return process;
        }

        private void NExecute(INPCCommand command, ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //Log($"Begin command = {command}");
#endif

            try
            {
                var processId = process.Id;

                var resolution = CreateResolution(command, processId);

#if DEBUG
                //Log($"resolution = {resolution}");
#endif

                var kindOfResolution = resolution.KindOfResult;

#if DEBUG
                //Log($"kindOfResolution = {kindOfResolution}");
#endif

                switch (kindOfResolution)
                {
                    case NPCResourcesResolutionKind.Allow:
                    case NPCResourcesResolutionKind.AllowAdd:
                        ProcessAllow(command, processId, process, kindOfResolution);
                        break;

                    case NPCResourcesResolutionKind.Forbiden:
                        {
#if DEBUG
                            //Log($"case NPCResourcesResolutionKind.Forbiden:");
#endif

                            var kindOfResolutionOfContext = mContext.ApproveNPCResourceProcessExecute(resolution);

#if DEBUG
                            //Log($"kindOfResolutionOfContext = {kindOfResolutionOfContext}");
#endif

                            switch (kindOfResolutionOfContext)
                            {
                                case NPCResourcesResolutionKind.Allow:
                                case NPCResourcesResolutionKind.AllowAdd:
                                    ProcessAllow(command, processId, process, kindOfResolutionOfContext);
                                    break;

                                case NPCResourcesResolutionKind.Forbiden:
                                    ProcessForbiden(process);
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(kindOfResolutionOfContext), kindOfResolutionOfContext, null);
                            }
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kindOfResolution), kindOfResolution, null);
                }
            }
            catch (OperationCanceledException)
            {
#if DEBUG
                Error("catch(OperationCanceledException)");
#endif
            }
            catch (Exception e)
            {
                process.State = StateOfNPCProcess.Faulted;

#if DEBUG
                Error($"End e = {e}");
#endif
            }
            finally
            {
                var taskId = Task.CurrentId;

                mContext.UnRegCancellationToken(taskId.Value);
            }

#if DEBUG
            //Log($"End command = {command}");
#endif
        }

        private NPCHandResourcesResolution CreateResolution(INPCCommand command, ulong processId)
        {
#if DEBUG
            //Log($"command = {command}");
            //Log($"processId = {processId}");
            //DumpProcesses();
#endif

            lock (mDataLockObj)
            {
                var result = new NPCHandResourcesResolution();
                result.TargetProcessId = processId;
                result.Command = command;

                var theSame = true;

                if(mListOfProcessedId.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if(!mListOfProcessedId.Contains(processId))
                    {
                        theSame = false;
                        result.KindOfResult = NPCResourcesResolutionKind.Forbiden;
                        result.CurrentProcessesId = mListOfProcessedId.ToList();
                    }
                }

                if (result.KindOfResult == NPCResourcesResolutionKind.Unknow)
                {
                    if (theSame)
                    {
                        result.KindOfResult = NPCResourcesResolutionKind.AllowAdd;
                    }
                    else
                    {
                        result.KindOfResult = NPCResourcesResolutionKind.Allow;
                    }
                }

#if DEBUG
                //Log("NEXT");
                //Log("End");
#endif
                return result;
            }
        }

        private void ProcessAllow(INPCCommand command, ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            //Log($"command = {command}");
            //Log($"processId = {processId}");
#endif

            RegProcessId(processId, process, resolutionKind);

#if DEBUG
            //Log("before mNPCHandHost.Send");
#endif

            var processOfHost = mNPCHandHost.Send(command);

#if DEBUG
            //Log("after mNPCHandHost.Send");
            //Log($"processOfHost.State = {processOfHost.State}");
#endif

            processOfHost.OnStateChanged += (INPCProcess sender, StateOfNPCProcess state) => {
#if DEBUG
                //Log($"processOfHost.OnStateChanged sender.Id = {sender.Id} state = {state}");
#endif

                process.State = state;
            };

            //throw new NotImplementedException();
        }

        private void RegProcessId(ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            //Log($"processId = {processId} process = {process} resolutionKind = {resolutionKind}");
#endif

            lock (mDataLockObj)
            {
#if DEBUG
                //Log($"processId = {processId} NEXT");
#endif

                var displacedProcessesIdList = new List<ulong>();

                switch (resolutionKind)
                {
                    case NPCResourcesResolutionKind.Allow:
                        displacedProcessesIdList.AddRange(mListOfProcessedId);
                        mListOfProcessedId.Clear();
                        break;

                    case NPCResourcesResolutionKind.AllowAdd:
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                }

                if (!mListOfProcessedId.Contains(processId))
                {
                    mListOfProcessedId.Add(processId);
                }

                if (displacedProcessesIdList.Count > 0)
                {
                    displacedProcessesIdList = displacedProcessesIdList.Distinct().ToList();

                    foreach (var displacedProcessId in displacedProcessesIdList)
                    {
#if DEBUG
                        //Log($"displacedProcessId = {displacedProcessId}");
#endif

                        RemoveProcessId(displacedProcessId);

                        //var displacedTask = mProcessesDict[displacedProcessId];
                        //displacedTask.State = StateOfNPCProcess.Canceled;
                        //mProcessesDict.Remove(displacedProcessId);
                    }
                }

#if DEBUG
                //Log($"before mProcessesDict.Count = {mProcessesDict.Count}");
#endif

                mProcessesDict[processId] = process;

#if DEBUG
                //Log($"after mProcessesDict.Count = {mProcessesDict.Count}");
#endif
            }
        }

        private void RemoveProcessId(ulong processId)
        {
            if (mListOfProcessedId.Contains(processId))
            {
                mListOfProcessedId.Remove(processId);
            }

            if (mProcessesDict.ContainsKey(processId))
            {
                var displacedTask = mProcessesDict[processId];
                displacedTask.State = StateOfNPCProcess.Canceled;
                mProcessesDict.Remove(processId);
            }
        }

        private void ProcessForbiden(ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //Log($"npcMeshTask = {npcMeshTask}");
#endif

            process.State = StateOfNPCProcess.Canceled;
        }

#if DEBUG
        private void DumpProcesses()
        {
            lock (mDataLockObj)
            {
                Log("Begin mListOfProcessedId");
                foreach (var item in mListOfProcessedId)
                {
                    Log($"item = {item}");
                }
                Log("End mListOfProcessedId");
                Log("Begin mProcessesDict");
                foreach (var kvpItem in mProcessesDict)
                {
                    var productId = kvpItem.Key;
                    var task = kvpItem.Value;

                    Log($"productId = {productId} task = {task}");
                }
                Log("End mProcessesDict");
            }
        }
#endif

        public object Get(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            return mNPCHandHost.Get(propertyName);
        }

        public void UnRegProcess(ulong processId)
        {
#if DEBUG
            //Log($"processId = {processId}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            //throw new NotImplementedException();
        }

        public void Dispose()
        {
#if DEBUG
            //Log("Begin");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCContext.Destroyed;
            }
        }
    }
}
