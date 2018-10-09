using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCBodyResourcesManager
    {
        public NPCBodyResourcesManager(IEntityLogger entityLogger, IIdFactory idFactory, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, INPCContext context)
        {
            mEntityLogger = entityLogger;
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            if(npcHostContext != null)
            {
                mNPCBodyHost = npcHostContext.BodyHost;
                mNPCBodyHost.OnHumanoidStatesChanged += OnHumanoidStatesChanged;
            }

            mContext = context;
        }

        #region private members
        private IEntityLogger mEntityLogger;
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private INPCBodyHost mNPCBodyHost;
        private INPCContext mContext;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
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

        private void OnHumanoidStatesChanged(IList<HumanoidStateKind> changedStates)
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }
            }

            Task.Run(() => {
                try
                {
#if DEBUG
                    //Log($"Begin changedStates");
                    //foreach (var changedState in changedStates)
                    //{
                    //    Log($"changedState = {changedState}");
                    //}
                    //Log($"End changedStates");
#endif

                    lock (mDataLockObj)
                    {
                        var displacedProcessesIdList = new List<ulong>();

                        foreach (var changedState in changedStates)
                        {
                            switch (changedState)
                            {
                                case HumanoidStateKind.HState:
                                    displacedProcessesIdList.AddRange(mHState);
                                    mHState.Clear();
                                    break;

                                case HumanoidStateKind.TargetPosition:
                                    displacedProcessesIdList.AddRange(mTargetPosition);
                                    mTargetPosition.Clear();
                                    break;

                                case HumanoidStateKind.VState:
                                    displacedProcessesIdList.AddRange(mVState);
                                    mVState.Clear();
                                    break;

                                case HumanoidStateKind.HandsState:
                                    displacedProcessesIdList.AddRange(mHandsState);
                                    mHandsState.Clear();
                                    break;

                                case HumanoidStateKind.HandsActionState:
                                    displacedProcessesIdList.AddRange(mHandsActionState);
                                    mHandsActionState.Clear();
                                    break;

                                case HumanoidStateKind.HeadState:
                                    displacedProcessesIdList.AddRange(mHeadState);
                                    mHeadState.Clear();
                                    break;

                                case HumanoidStateKind.TargetHeadPosition:
                                    displacedProcessesIdList.AddRange(mTargetHeadPosition);
                                    mTargetHeadPosition.Clear();
                                    break;

                                case HumanoidStateKind.ThingsCommand:
                                    displacedProcessesIdList.AddRange(mHandsState);
                                    mHandsState.Clear();
                                    displacedProcessesIdList.AddRange(mHandsActionState);
                                    mHandsActionState.Clear();
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(changedState), changedState, null);
                            }
                        }

                        displacedProcessesIdList = displacedProcessesIdList.Distinct().ToList();

#if DEBUG
                        //Log($"displacedProcessesIdList.Count = {displacedProcessesIdList.Count}");
                        //Log($"before mTasksDict.Count = {mTasksDict.Count}");
#endif

                        foreach (var displacedProcessId in displacedProcessesIdList)
                        {
#if DEBUG
                            //Log($"displacedProcessId = {displacedProcessId}");
#endif

                            var targetTask = mProcessesDict[displacedProcessId];
                            mProcessesDict.Remove(displacedProcessId);
                            targetTask.State = StateOfNPCProcess.RanToCompletion;
                        }

#if DEBUG
                        //Log($"after mTasksDict.Count = {mTasksDict.Count}");
#endif
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
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

        public INPCProcess Send(IHumanoidBodyCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    return new NotValidResourceNPCProcess(mEntityLogger, mContext);
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(mEntityLogger, id, mContext);

            var cs = new CancellationTokenSource();

            process.CancellationToken = cs;

            var token = cs.Token;

            var task = new Task(() => {
                NExecute(command, process);
            }, token);

            process.Task = task;

            process.Task = task;

            var taskId = task.Id;

            mContext.RegCancellationToken(taskId, token);

            task.Start();

            return process;
        }

        private void NExecute(IHumanoidBodyCommand command, ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //Log($"Begin command = {command}");
#endif
            try
            {
                var processId = command.InitiatingProcessId;

                var targetState = CreateTargetState(command);

#if DEBUG
                //Log($"targetState = {targetState}");
#endif
                var resolution = CreateResolution(mNPCBodyHost.States, targetState, processId);

#if DEBUG
                //Log($"resolution = {resolution}");
#endif

                var kindOfResolution = resolution.KindOfResult;

                switch (kindOfResolution)
                {
                    case NPCResourcesResolutionKind.Allow:
                    case NPCResourcesResolutionKind.AllowAdd:
                        ProcessAllow(targetState, processId, process, kindOfResolution);
                        break;

                    case NPCResourcesResolutionKind.Forbiden:
                        {
                            var kindOfResolutionOfContext = mContext.ApproveNPCResourceProcessExecute(resolution);

#if UNITY_EDITOR
                        //Log($"kindOfResolutionOfContext = {kindOfResolutionOfContext}");
#endif

                            switch (kindOfResolutionOfContext)
                            {
                                case NPCResourcesResolutionKind.Allow:
                                case NPCResourcesResolutionKind.AllowAdd:
                                    ProcessAllow(targetState, processId, process, kindOfResolutionOfContext);
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

        private TargetStateOfHumanoidBody CreateTargetState(IHumanoidBodyCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif

            var result = new TargetStateOfHumanoidBody();

            var kind = command.Kind;

            switch (kind)
            {
                case HumanoidBodyCommandKind.HState:
                    {
                        var targetCommand = command as IHumanoidHStateCommand;

                        var targetHState = targetCommand.State;
                        var targetPosition = targetCommand.TargetPosition;

                        result.HState = targetCommand.State;
                        result.TargetPosition = targetCommand.TargetPosition;

                        switch (targetHState)
                        {
                            case HumanoidHState.Stop:
                                result.TargetPosition = null;
                                break;

                            case HumanoidHState.Walk:
                            case HumanoidHState.Run:
                                if (targetPosition == null)
                                {
                                    result.HState = HumanoidHState.Stop;
                                }
                                break;
                        }
                    }
                    break;

                case HumanoidBodyCommandKind.VState:
                    {
                        var targetCommand = command as IHumanoidVStateCommand;

                        result.VState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HandsState:
                    {
                        var targetCommand = command as IHumanoidHandsStateCommand;

                        result.HandsState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HandsActionState:
                    {
                        var targetCommand = command as IHumanoidHandsActionStateCommand;

                        result.HandsActionState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HeadState:
                    {
                        var targetCommand = command as IHumanoidHeadStateCommand;

                        result.HeadState = targetCommand.State;
                        result.TargetHeadPosition = targetCommand.TargetPosition;
                    };
                    break;

                case HumanoidBodyCommandKind.Things:
                    {
                        var targetCommand = command as IHumanoidThingsCommand;

                        result.KindOfThingsCommand = targetCommand.State;

                        var thing = targetCommand.Thing;

                        if(thing != null)
                        {
                            result.EntityIdOfThing = thing.CurrentEntityId;
                        }                        
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            if (result.HandsState.HasValue)
            {
                var targeHandsState = result.HandsState.Value;

                switch (targeHandsState)
                {
                    case HumanoidHandsState.FreeHands:
                        result.HandsActionState = HumanoidHandsActionState.Empty;
                        break;
                }
            }

#if DEBUG
            //Log($"result = {result}");
#endif

            return result;
        }

        private NPCBodyResourcesResolution CreateResolution(IStatesOfHumanoidBodyHost sourceState, TargetStateOfHumanoidBody targetState, ulong processId)
        {
#if DEBUG
            //Log($"sourceState = {sourceState}");
            //Log($"targetState = {targetState}");
            //Log($"processId = {processId}");
            //DumpProcesses();
#endif

            lock(mDataLockObj)
            {
                var result = new NPCBodyResourcesResolution();
                result.TargetProcessId = processId;
                result.TargetState = targetState;

                var theSame = true;

                var currentStates = sourceState;

                if (targetState.HState.HasValue)
                {
                    var targetHState = targetState.HState.Value;

                    if (mHState.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mHState.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByHStateInfo();
                            result.DisagreementByHState = disagreement;
                            disagreement.CurrentProcessesId = mHState.ToList();
                            disagreement.CurrentValue = currentStates.HState;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetHState;
                        }
                    }
                }

                if (targetState.TargetPosition != null)
                {
                    var targetPosition = targetState.TargetPosition;

                    if (mTargetPosition.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mTargetPosition.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByTargetPositionInfo();
                            result.DisagreementByTargetPosition = disagreement;
                            disagreement.CurrentProcessesId = mTargetPosition.ToList();
                            disagreement.CurrentValue = currentStates.TargetPosition;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetPosition;
                        }
                    }
                }

                if (targetState.VState.HasValue)
                {
                    var targetVState = targetState.VState.Value;

                    if (mVState.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mVState.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByVStateInfo();
                            result.DisagreementByVState = disagreement;
                            disagreement.CurrentProcessesId = mVState.ToList();
                            disagreement.CurrentValue = currentStates.VState;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetVState;
                        }
                    }
                }

                if (targetState.HandsState.HasValue || targetState.KindOfThingsCommand.HasValue)
                {
                    var targetHandsState = HumanoidHandsState.FreeHands;

                    if (targetState.HandsState.HasValue)
                    {
                        targetHandsState = targetState.HandsState.Value;
                    }
                    else
                    {
                        var kindOfThingsCommand = targetState.KindOfThingsCommand.Value;

                        switch (kindOfThingsCommand)
                        {
                            case KindOfHumanoidThingsCommand.Take:
                                targetHandsState = HumanoidHandsState.HasRifle;
                                break;

                            case KindOfHumanoidThingsCommand.PutToBagpack:
                            case KindOfHumanoidThingsCommand.ThrowOutToSurface:
                                targetHandsState = HumanoidHandsState.FreeHands;
                                break;
                        }
                    }

#if DEBUG
                    //Log($"targetHandsState = {targetHandsState}");
#endif

                    if (mHandsState.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mHandsState.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByHandsStateInfo();
                            result.DisagreementByHandsState = disagreement;
                            disagreement.CurrentProcessesId = mHandsState.ToList();
                            disagreement.CurrentValue = currentStates.HandsState;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetHandsState;
                        }
                    }
                }

                if (targetState.HandsActionState.HasValue)
                {
                    var targetHandsActionState = targetState.HandsActionState.Value;

                    if (mHandsActionState.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mHandsActionState.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByHandsActionStateInfo();
                            result.DisagreementByHandsActionState = disagreement;
                            disagreement.CurrentProcessesId = mHandsActionState.ToList();
                            disagreement.CurrentValue = currentStates.HandsActionState;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetHandsActionState;
                        }
                    }
                }

                if (targetState.HeadState.HasValue)
                {
                    var targetHeadState = targetState.HeadState.Value;

                    if (mHeadState.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mHeadState.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByHeadStateInfo();
                            result.DisagreementByHeadState = disagreement;
                            disagreement.CurrentProcessesId = mHeadState.ToList();
                            disagreement.CurrentValue = currentStates.HeadState;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetHeadState;
                        }
                    }
                }

                if (targetState.TargetHeadPosition != null)
                {
                    var targetHeadPosition = targetState.TargetHeadPosition;

                    if (mTargetHeadPosition.Count == 0)
                    {
                        theSame = false;
                    }
                    else
                    {
                        if (!mTargetHeadPosition.Contains(processId))
                        {
                            theSame = false;
                            result.KindOfResult = NPCResourcesResolutionKind.Forbiden;

                            var disagreement = new DisagreementByTargetHeadPositionInfo();
                            result.DisagreementByTargetHeadPosition = disagreement;
                            disagreement.CurrentProcessesId = mTargetHeadPosition.ToList();
                            disagreement.CurrentValue = currentStates.TargetHeadPosition;
                            disagreement.TargetProcessId = processId;
                            disagreement.TargetValue = targetHeadPosition;
                        }
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

        private readonly object mDataLockObj = new object();

        private readonly List<ulong> mHState = new List<ulong>();
        private readonly List<ulong> mTargetPosition = new List<ulong>();
        private readonly List<ulong> mVState = new List<ulong>();
        private readonly List<ulong> mHandsState = new List<ulong>();
        private readonly List<ulong> mHandsActionState = new List<ulong>();
        private readonly List<ulong> mHeadState = new List<ulong>();
        private readonly List<ulong> mTargetHeadPosition = new List<ulong>();
        private readonly Dictionary<ulong, ProxyForNPCResourceProcess> mProcessesDict = new Dictionary<ulong, ProxyForNPCResourceProcess>();

#if DEBUG
        private void DumpProcesses()
        {
            lock (mDataLockObj)
            {
                Log($"Begin {nameof(mHState)}");
                foreach (var item in mHState)
                {
                    Log($"{nameof(item)} = {item}");
                }
                Log($"End {nameof(mHState)}");

                Log($"Begin {nameof(mTargetPosition)}");
                foreach (var item in mTargetPosition)
                {
                    Log($"{nameof(item)} = {item}");
                }
                Log($"End {nameof(mTargetPosition)}");

                Log($"Begin {nameof(mVState)}");
                foreach (var item in mVState)
                {
                    Log($"{nameof(item)} = {item}");
                }
                Log($"End {nameof(mVState)}");

                Log($"Begin {nameof(mHandsState)}");
                foreach (var item in mHandsState)
                {
                    Log($"item = {item}");
                }
                Log("End mHandsState");

                Log("Begin mHandsActionState");
                foreach (var item in mHandsActionState)
                {
                    Log($"item = {item}");
                }
                Log("End mHandsActionState");

                Log("Begin mHeadState");
                foreach (var item in mHeadState)
                {
                    Log($"item = {item}");
                }
                Log("End mHeadState");

                Log("Begin mTargetHeadPosition");
                foreach (var item in mTargetHeadPosition)
                {
                    Log($"item = {item}");
                }
                Log("End mTargetHeadPosition");
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

        private void ProcessAllow(TargetStateOfHumanoidBody targetState, ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            //Log($"targetState = {targetState}");
            //Log($"processId = {processId}");
#endif

            RegProcessId(targetState, processId, process, resolutionKind);

#if DEBUG
            //Log("before mNPCBodyHost.ExecuteAsync");
#endif

            mNPCBodyHost.Execute(targetState);

#if DEBUG
            //Log("after mNPCBodyHost.ExecuteAsync");
#endif

            process.State = StateOfNPCProcess.Running;
        }

        private void RegProcessId(TargetStateOfHumanoidBody targetState, ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
            lock (mDataLockObj)
            {
                var displacedProcessesIdList = new List<ulong>();

                if (targetState.HState.HasValue)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mHState);
                            mHState.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mHState.Contains(processId))
                    {
                        mHState.Add(processId);
                    }
                }
                else
                {
                    if (mHState.Contains(processId))
                    {
                        mHState.Remove(processId);
                    }
                }

                if (targetState.TargetPosition != null)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mTargetPosition);
                            mTargetPosition.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mTargetPosition.Contains(processId))
                    {
                        mTargetPosition.Add(processId);
                    }
                }
                else
                {
                    if (mTargetPosition.Contains(processId))
                    {
                        mTargetPosition.Remove(processId);
                    }
                }

                if (targetState.VState.HasValue)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mVState);
                            mVState.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mVState.Contains(processId))
                    {
                        mVState.Add(processId);
                    }
                }
                else
                {
                    if (mVState.Contains(processId))
                    {
                        mVState.Remove(processId);
                    }
                }

                if (targetState.HandsState.HasValue || targetState.KindOfThingsCommand.HasValue)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mHandsState);
                            mHandsState.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mHandsState.Contains(processId))
                    {
                        mHandsState.Add(processId);
                    }
                }
                else
                {
                    if (mHandsState.Contains(processId))
                    {
                        mHandsState.Remove(processId);
                    }
                }

                if (targetState.HandsActionState.HasValue)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mHandsActionState);
                            mHandsActionState.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mHandsActionState.Contains(processId))
                    {
                        mHandsActionState.Add(processId);
                    }
                }
                else
                {
                    if (mHandsActionState.Contains(processId))
                    {
                        mHandsActionState.Remove(processId);
                    }
                }

                if (targetState.HeadState.HasValue)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mHeadState);
                            mHeadState.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mHeadState.Contains(processId))
                    {
                        mHeadState.Add(processId);
                    }
                }
                else
                {
                    if (mHeadState.Contains(processId))
                    {
                        mHeadState.Remove(processId);
                    }
                }

                if (targetState.TargetHeadPosition != null)
                {
                    switch (resolutionKind)
                    {
                        case NPCResourcesResolutionKind.Allow:
                            displacedProcessesIdList.AddRange(mTargetHeadPosition);
                            mTargetHeadPosition.Clear();
                            break;

                        case NPCResourcesResolutionKind.AllowAdd:
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                    }

                    if (!mTargetHeadPosition.Contains(processId))
                    {
                        mTargetHeadPosition.Add(processId);
                    }
                }
                else
                {
                    if (mTargetHeadPosition.Contains(processId))
                    {
                        mTargetHeadPosition.Remove(processId);
                    }
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
                    }
                }

#if DEBUG
                //Log($"before mTasksDict.Count = {mTasksDict.Count}");
#endif

                mProcessesDict[processId] = process;

#if DEBUG
                //Log($"after mTasksDict.Count = {mTasksDict.Count}");
#endif
            }
        }

        private void ProcessForbiden(ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //Log($"npcMeshTask = {npcMeshTask}");
#endif

            process.State = StateOfNPCProcess.Canceled;
        }

        public void UnRegProcess(ulong processId)
        {
#if DEBUG
            //Log($"processId = {processId}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }
            }

            lock (mDataLockObj)
            {
                if (!mProcessesDict.ContainsKey(processId))
                {
                    return;
                }

                RemoveProcessId(processId);
            }
        }

        private void RemoveProcessId(ulong processId)
        {
            if (mHState.Contains(processId))
            {
                mHState.Remove(processId);
            }

            if (mTargetPosition.Contains(processId))
            {
                mTargetPosition.Remove(processId);
            }

            if (mVState.Contains(processId))
            {
                mVState.Remove(processId);
            }

            if (mHandsState.Contains(processId))
            {
                mHandsState.Remove(processId);
            }

            if (mHandsActionState.Contains(processId))
            {
                mHandsActionState.Remove(processId);
            }

            if (mHeadState.Contains(processId))
            {
                mHeadState.Remove(processId);
            }

            if (mTargetHeadPosition.Contains(processId))
            {
                mTargetHeadPosition.Remove(processId);
            }

            if(mProcessesDict.ContainsKey(processId))
            {
                var displacedTask = mProcessesDict[processId];
                displacedTask.State = StateOfNPCProcess.Canceled;
                mProcessesDict.Remove(processId);
            }
        }

        public void CallInMainUI(Action function)
        {
            mNPCBodyHost.CallInMainUI(function);
        }

        public TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            return mNPCBodyHost.CallInMainUI(function);
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
