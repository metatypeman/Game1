using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCBodyResourcesManager
    {
        public NPCBodyResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary, INPCHostContext npcHostContext, INPCContext context)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mNPCBodyHost = npcHostContext.BodyHost;
            mNPCBodyHost.OnHumanoidStatesChanged += OnHumanoidStatesChanged;
            mContext = context;
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private INPCBodyHost mNPCBodyHost;
        private INPCContext mContext;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        #endregion

        private void OnHumanoidStatesChanged(List<HumanoidStateKind> changedStates)
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }
            }

            Task.Run(() => {
#if DEBUG
                //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged Begin changedStates");
                //foreach (var changedState in changedStates)
                //{
                //    LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged changedState = {changedState}");
                //}
                //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged End changedStates");
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
                    //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged displacedProcessesIdList.Count = {displacedProcessesIdList.Count}");
                    //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged before mTasksDict.Count = {mTasksDict.Count}");
#endif

                    foreach (var displacedProcessId in displacedProcessesIdList)
                    {
#if DEBUG
                        //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged displacedProcessId = {displacedProcessId}");
#endif

                        var targetTask = mProcessesDict[displacedProcessId];
                        mProcessesDict.Remove(displacedProcessId);
                        targetTask.State = StateOfNPCProcess.RanToCompletion;
                    }

#if DEBUG
                    //LogInstance.Log($"NPCBodyResourcesManager OnHumanoidStatesChanged after mTasksDict.Count = {mTasksDict.Count}");
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
            LogInstance.Log($"NPCBodyResourcesManager Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    return new NotValidResourceNPCProcess(mContext);
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(id, mContext);

            var task = new Task(() => {
                NExecute(command, process);
            });

            process.Task = task;

            task.Start();

            return process;
        }

        private void NExecute(IHumanoidBodyCommand command, ProxyForNPCResourceProcess process)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Begin NExecute command = {command}");
#endif
            var processId = command.InitiatingProcessId;

            var targetState = CreateTargetState(command);

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager NExecute targetState = {targetState}");
#endif
            var resolution = CreateResolution(mNPCBodyHost.States, targetState, processId);

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager NExecute resolution = {resolution}");
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
                        //LogInstance.Log($"NPCBodyResourcesManager Execute kindOfResolutionOfContext = {kindOfResolutionOfContext}");
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

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager End NExecute command = {command}");
#endif
        }

        private TargetStateOfHumanoidBody CreateTargetState(IHumanoidBodyCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager CreateTargetState command = {command}");
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
                        result.InstanceOfThingId = targetCommand.InstanceId;
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
            LogInstance.Log($"NPCBodyResourcesManager CreateTargetState result = {result}");
#endif

            return result;
        }

        private NPCBodyResourcesResolution CreateResolution(IStatesOfHumanoidBodyHost sourceState, TargetStateOfHumanoidBody targetState, ulong processId)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager CreateResolution sourceState = {sourceState}");
            LogInstance.Log($"NPCBodyResourcesManager CreateResolution targetState = {targetState}");
            LogInstance.Log($"NPCBodyResourcesManager CreateResolution processId = {processId}");
            DumpProcesses();
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
                    LogInstance.Log($"NPCBodyResourcesManager CreateResolution targetHandsState = {targetHandsState}");
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
                LogInstance.Log("NPCBodyResourcesManager CreateResolution NEXT");
                LogInstance.Log("End NPCBodyResourcesManager CreateResolution");
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
                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses Begin {nameof(mHState)}");
                foreach (var item in mHState)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses {nameof(item)} = {item}");
                }
                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses End {nameof(mHState)}");

                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses Begin {nameof(mTargetPosition)}");
                foreach (var item in mTargetPosition)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses {nameof(item)} = {item}");
                }
                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses End {nameof(mTargetPosition)}");

                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses Begin {nameof(mVState)}");
                foreach (var item in mVState)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses {nameof(item)} = {item}");
                }
                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses End {nameof(mVState)}");

                LogInstance.Log($"NPCBodyResourcesManager DumpProcesses Begin {nameof(mHandsState)}");
                foreach (var item in mHandsState)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses item = {item}");
                }
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses End mHandsState");

                LogInstance.Log("NPCBodyResourcesManager DumpProcesses Begin mHandsActionState");
                foreach (var item in mHandsActionState)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses item = {item}");
                }
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses End mHandsActionState");

                LogInstance.Log("NPCBodyResourcesManager DumpProcesses Begin mHeadState");
                foreach (var item in mHeadState)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses item = {item}");
                }
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses End mHeadState");

                LogInstance.Log("NPCBodyResourcesManager DumpProcesses Begin mTargetHeadPosition");
                foreach (var item in mTargetHeadPosition)
                {
                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses item = {item}");
                }
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses End mTargetHeadPosition");
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses Begin mProcessesDict");
                foreach (var kvpItem in mProcessesDict)
                {
                    var productId = kvpItem.Key;
                    var task = kvpItem.Value;

                    LogInstance.Log($"NPCBodyResourcesManager DumpProcesses productId = {productId} task = {task}");
                }
                LogInstance.Log("NPCBodyResourcesManager DumpProcesses End mProcessesDict");
            }
        }
#endif

        private void ProcessAllow(TargetStateOfHumanoidBody targetState, ulong processId, ProxyForNPCResourceProcess process, NPCResourcesResolutionKind resolutionKind)
        {
#if DEBUG
            //LogInstance.Log($"NPCBodyResourcesManager ProcessAllow targetState = {targetState}");
            //LogInstance.Log($"NPCBodyResourcesManager ProcessAllow processId = {processId}");
#endif

            RegProcessId(targetState, processId, process, resolutionKind);

#if DEBUG
            //LogInstance.Log("NPCBodyResourcesManager ProcessAllow before mNPCBodyHost.ExecuteAsync");
#endif

            var targetStateForExecuting = mNPCBodyHost.ExecuteAsync(targetState);

            while (targetStateForExecuting.State == StateOfHumanoidTaskOfExecuting.Created)
            {
            }

#if DEBUG
            //LogInstance.Log("NPCBodyResourcesManager ProcessAllow after mNPCBodyHost.ExecuteAsync");
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
                        //LogInstance.Log($"NPCBodyResourcesManager CreateTargetState displacedProcessId = {displacedProcessId}");
#endif

                        RemoveProcessId(displacedProcessId);

                        var displacedTask = mProcessesDict[displacedProcessId];
                        displacedTask.State = StateOfNPCProcess.Canceled;
                        mProcessesDict.Remove(displacedProcessId);
                    }
                }

#if DEBUG
                //LogInstance.Log($"NPCBodyResourcesManager CreateTargetState before mTasksDict.Count = {mTasksDict.Count}");
#endif

                mProcessesDict[processId] = process;

#if DEBUG
                //LogInstance.Log($"NPCBodyResourcesManager CreateTargetState after mTasksDict.Count = {mTasksDict.Count}");
#endif
            }
        }

        private void ProcessForbiden(ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //LogInstance.Log($"NPCBodyResourcesManager ProcessForbiden npcMeshTask = {npcMeshTask}");
#endif

            process.State = StateOfNPCProcess.Canceled;
        }

        public void UnRegProcess(ulong processId)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager UnRegProcess processId = {processId}");
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
                mProcessesDict.Remove(processId);
            }
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("NPCBodyResourcesManager Dispose");
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
