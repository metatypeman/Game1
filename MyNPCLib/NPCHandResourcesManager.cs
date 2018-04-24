﻿using System;
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
        public NPCHandResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, KindOfHand kindOfHand, INPCContext context)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mContext = context;

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

        #region private members
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
            LogInstance.Log($"NPCHandResourcesManager Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(id, mContext);
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
            LogInstance.Log($"NPCHandResourcesManager Begin NExecute command = {command}");
#endif

            try
            {
                var processId = process.Id;

                var resolution = CreateResolution(command, processId);

#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager NExecute resolution = {resolution}");
#endif

                var kindOfResolution = resolution.KindOfResult;

#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager NExecute kindOfResolution = {kindOfResolution}");
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
                            LogInstance.Log($"NPCHandResourcesManager NExecute case NPCResourcesResolutionKind.Forbiden:");
#endif

                            var kindOfResolutionOfContext = mContext.ApproveNPCResourceProcessExecute(resolution);

#if DEBUG
                            LogInstance.Log($"NPCHandResourcesManager NExecute kindOfResolutionOfContext = {kindOfResolutionOfContext}");
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
                LogInstance.Log("NPCHandResourcesManager NExecute catch(OperationCanceledException)");
#endif
            }
            catch (Exception e)
            {
                process.State = StateOfNPCProcess.Faulted;

#if DEBUG
                LogInstance.Log($"End NPCHandResourcesManager NExecute e = {e}");
#endif
            }
            finally
            {
                var taskId = Task.CurrentId;

                mContext.UnRegCancellationToken(taskId.Value);
            }

#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager End NExecute command = {command}");
#endif
        }

        private NPCHandResourcesResolution CreateResolution(INPCCommand command, ulong processId)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager CreateResolution command = {command}");
            LogInstance.Log($"NPCHandResourcesManager CreateResolutionprocessId = {processId}");
            DumpProcesses();
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
                LogInstance.Log("NPCHandResourcesManager CreateResolution NEXT");
                LogInstance.Log("End NPCHandResourcesManager CreateResolution");
#endif
                return result;
            }
        }

        private void ProcessAllow(INPCCommand command, ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager ProcessAllow command = {command}");
            LogInstance.Log($"NPCHandResourcesManager ProcessAllow processId = {processId}");
#endif

            RegProcessId(processId, process, resolutionKind);

#if DEBUG
            LogInstance.Log("NPCHandResourcesManager ProcessAllow before mNPCHandHost.Send");
#endif

            var processOfHost = mNPCHandHost.Send(command);

#if DEBUG
            LogInstance.Log("NPCHandResourcesManager ProcessAllow after mNPCHandHost.Send");
            LogInstance.Log($"NPCHandResourcesManager ProcessAllow processOfHost.State = {processOfHost.State}");
#endif

            processOfHost.OnStateChanged += (INPCProcess sender, StateOfNPCProcess state) => {
#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager ProcessAllow processOfHost.OnStateChanged sender.Id = {sender.Id} state = {state}");
#endif

                process.State = state;
            };

            //throw new NotImplementedException();
        }

        private void RegProcessId(ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager RegProcessId processId = {processId} process = {process} resolutionKind = {resolutionKind}");
#endif

            lock (mDataLockObj)
            {
#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager RegProcessId processId = {processId} NEXT");
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
                        LogInstance.Log($"NPCHandResourcesManager RegProcessId displacedProcessId = {displacedProcessId}");
#endif

                        RemoveProcessId(displacedProcessId);

                        //var displacedTask = mProcessesDict[displacedProcessId];
                        //displacedTask.State = StateOfNPCProcess.Canceled;
                        //mProcessesDict.Remove(displacedProcessId);
                    }
                }

#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager RegProcessId before mProcessesDict.Count = {mProcessesDict.Count}");
#endif

                mProcessesDict[processId] = process;

#if DEBUG
                LogInstance.Log($"NPCHandResourcesManager RegProcessId after mProcessesDict.Count = {mProcessesDict.Count}");
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
            //LogInstance.Log($"NPCHandResourcesManager ProcessForbiden npcMeshTask = {npcMeshTask}");
#endif

            process.State = StateOfNPCProcess.Canceled;
        }

#if DEBUG
        private void DumpProcesses()
        {
            lock (mDataLockObj)
            {
                LogInstance.Log("NPCHandResourcesManager DumpProcesses Begin mListOfProcessedId");
                foreach (var item in mListOfProcessedId)
                {
                    LogInstance.Log($"NPCHandResourcesManager DumpProcesses item = {item}");
                }
                LogInstance.Log("NPCHandResourcesManager DumpProcesses End mListOfProcessedId");
                LogInstance.Log("NPCHandResourcesManager DumpProcesses Begin mProcessesDict");
                foreach (var kvpItem in mProcessesDict)
                {
                    var productId = kvpItem.Key;
                    var task = kvpItem.Value;

                    LogInstance.Log($"NPCHandResourcesManager DumpProcesses productId = {productId} task = {task}");
                }
                LogInstance.Log("NPCHandResourcesManager DumpProcesses End mProcessesDict");
            }
        }
#endif

        public object Get(string propertyName)
        {
#if DEBUG
            LogInstance.Log($"NPCHandResourcesManager Get propertyName = {propertyName}");
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
            LogInstance.Log($"NPCHandResourcesManager UnRegProcess processId = {processId}");
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
            LogInstance.Log("NPCHandResourcesManager Dispose");
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
