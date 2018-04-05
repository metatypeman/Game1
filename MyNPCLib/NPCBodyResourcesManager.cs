using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCBodyResourcesManager
    {
        public NPCBodyResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary, IHumanoidBodyController humanoidBodyController, INPCContext context)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mHumanoidBodyController = humanoidBodyController;
            mHumanoidBodyController.OnHumanoidStatesChanged += OnHumanoidStatesChanged;
            mContext = context;
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private IHumanoidBodyController mHumanoidBodyController;
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
                //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged Begin changedStates");
                //foreach (var changedState in changedStates)
                //{
                //    Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged changedState = {changedState}");
                //}
                //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged End changedStates");
#endif

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
                //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged displacedProcessesIdList.Count = {displacedProcessesIdList.Count}");
                //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged before mTasksDict.Count = {mTasksDict.Count}");
#endif

                foreach (var displacedProcessId in displacedProcessesIdList)
                {
#if DEBUG
                    //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged displacedProcessId = {displacedProcessId}");
#endif

                    var targetTask = mTasksDict[displacedProcessId];
                    mTasksDict.Remove(displacedProcessId);
                    targetTask.State = StateOfNPCProcess.RanToCompletion;
                }

#if DEBUG
                //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged after mTasksDict.Count = {mTasksDict.Count}");
#endif
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
                    return new NotValidResourceNPCProcess();
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(id);

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
            var resolution = CreateResolution(mHumanoidBodyController.States, targetState, processId);

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager NExecute resolution = {resolution}");
#endif

            var kindOfResolution = resolution.Kind;

            switch (kindOfResolution)
            {
                case NPCMeshTaskResulutionKind.Allow:
                case NPCMeshTaskResulutionKind.AllowAdd:
                    ProcessAllow(targetState, processId, process, kindOfResolution);
                    break;

                case NPCMeshTaskResulutionKind.Forbiden:
                    {
                        var kindOfResolutionOfContext = mContext.ApproveNPCMeshTaskExecute(resolution);

#if UNITY_EDITOR
                        //Debug.Log($"NPCThreadSafeMeshController Execute kindOfResolutionOfContext = {kindOfResolutionOfContext}");
#endif

                        switch (kindOfResolutionOfContext)
                        {
                            case NPCMeshTaskResulutionKind.Allow:
                            case NPCMeshTaskResulutionKind.AllowAdd:
                                ProcessAllow(targetState, processId, process, kindOfResolutionOfContext);
                                break;

                            case NPCMeshTaskResulutionKind.Forbiden:
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
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState command = {command}");
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
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState result = {result}");
#endif

            return result;
        }

        private NPCResourcesResulution CreateResolution(StatesOfHumanoidBodyController sourceState, TargetStateOfHumanoidBody targetState, ulong processId)
        {
#if DEBUG
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState sourceState = {sourceState}");
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState targetState = {targetState}");
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState processId = {processId}");
            DumpProcesses();
#endif

            var result = new NPCResourcesResulution();
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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState targetHandsState = {targetHandsState}");
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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByTargetHeadPositionInfo();
                        result.DisagreementByTargetHeadPosition = disagreement;
                        disagreement.CurrentProcessesId = mTargetHeadPosition.ToList();
                        disagreement.CurrentValue = currentStates.TargetHeadPosition;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHeadPosition;
                    }
                }
            }

            if (result.Kind == NPCMeshTaskResulutionKind.Unknow)
            {
                if (theSame)
                {
                    result.Kind = NPCMeshTaskResulutionKind.AllowAdd;
                }
                else
                {
                    result.Kind = NPCMeshTaskResulutionKind.Allow;
                }
            }

#if DEBUG
            LogInstance.Log("NPCThreadSafeMeshController CreateTargetState NEXT");
            LogInstance.Log("End NPCThreadSafeMeshController CreateTargetState");
#endif
            return result;
        }

        private List<ulong> mHState = new List<ulong>();
        private List<ulong> mTargetPosition = new List<ulong>();
        private List<ulong> mVState = new List<ulong>();
        private List<ulong> mHandsState { get; set; } = new List<ulong>();
        private List<ulong> mHandsActionState = new List<ulong>();
        private List<ulong> mHeadState = new List<ulong>();
        private List<ulong> mTargetHeadPosition = new List<ulong>();
        private Dictionary<ulong, ProxyForNPCResourceProcess> mTasksDict = new Dictionary<ulong, ProxyForNPCResourceProcess>();

#if DEBUG
        private void DumpProcesses()
        {
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mHState");
            foreach(var item in mHState)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mHState");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mTargetPosition");
            foreach (var item in mTargetPosition)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mTargetPosition");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mVState");
            foreach (var item in mVState)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mVState");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mHandsState");
            foreach (var item in mHandsState)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mHandsState");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mHandsActionState");
            foreach (var item in mHandsActionState)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mHandsActionState");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mHeadState");
            foreach (var item in mHeadState)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mHeadState");

            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow Begin mTargetHeadPosition");
            foreach (var item in mTargetHeadPosition)
            {
                LogInstance.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            LogInstance.Log("NPCThreadSafeMeshController ProcessAllow End mTargetHeadPosition");
            //Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mTasksDict");
            //foreach (var kvpItem in mTasksDict)
            //{
            //    var productId = kvpItem.Key;
            //    var task = kvpItem.Value;

            //    Debug.Log($"NPCThreadSafeMeshController ProcessAllow productId = {productId} task = {task}");
            //}
            //Debug.Log("NPCThreadSafeMeshController ProcessAllow End mTasksDict");
        }
#endif

        private void ProcessAllow(TargetStateOfHumanoidBody targetState, ulong processId, ProxyForNPCResourceProcess process, NPCMeshTaskResulutionKind resolutionKind)
        {
#if DEBUG
            //Debug.Log($"NPCThreadSafeMeshController ProcessAllow targetState = {targetState}");
            //Debug.Log($"NPCThreadSafeMeshController ProcessAllow processId = {processId}");
#endif

            RegProcessId(targetState, processId, process, resolutionKind);

#if DEBUG
            //Debug.Log("NPCThreadSafeMeshController ProcessAllow before mMoveHumanoidController.ExecuteAsync");
#endif

            var targetStateForExecuting = mHumanoidBodyController.ExecuteAsync(targetState);

            while (targetStateForExecuting.State == StateOfHumanoidTaskOfExecuting.Created)
            {
            }

#if DEBUG
            //Debug.Log("NPCThreadSafeMeshController ProcessAllow after mMoveHumanoidController.ExecuteAsync");
#endif

            process.State = StateOfNPCProcess.Running;
        }

        private void RegProcessId(TargetStateOfHumanoidBody targetState, ulong processId, ProxyForNPCResourceProcess process, NPCMeshTaskResulutionKind resolutionKind)
        {
            var displacedProcessesIdList = new List<ulong>();

            if (targetState.HState.HasValue)
            {
                switch (resolutionKind)
                {
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mHState);
                        mHState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mTargetPosition);
                        mTargetPosition.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mVState);
                        mVState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mHandsState);
                        mHandsState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mHandsActionState);
                        mHandsActionState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mHeadState);
                        mHeadState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mTargetHeadPosition);
                        mTargetHeadPosition.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
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
                    //Debug.Log($"NPCThreadSafeMeshController CreateTargetState displacedProcessId = {displacedProcessId}");
#endif

                    if (mHState.Contains(displacedProcessId))
                    {
                        mHState.Remove(displacedProcessId);
                    }

                    if (mTargetPosition.Contains(displacedProcessId))
                    {
                        mTargetPosition.Remove(displacedProcessId);
                    }

                    if (mVState.Contains(displacedProcessId))
                    {
                        mVState.Remove(displacedProcessId);
                    }

                    if (mHandsState.Contains(displacedProcessId))
                    {
                        mHandsState.Remove(displacedProcessId);
                    }

                    if (mHandsActionState.Contains(displacedProcessId))
                    {
                        mHandsActionState.Remove(displacedProcessId);
                    }

                    if (mHeadState.Contains(displacedProcessId))
                    {
                        mHeadState.Remove(displacedProcessId);
                    }

                    if (mTargetHeadPosition.Contains(displacedProcessId))
                    {
                        mTargetHeadPosition.Remove(displacedProcessId);
                    }

                    var displacedTask = mTasksDict[displacedProcessId];
                    displacedTask.State = StateOfNPCProcess.Canceled;
                    mTasksDict.Remove(displacedProcessId);
                }
            }

#if UDEBUG
            //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged before mTasksDict.Count = {mTasksDict.Count}");
#endif

            mTasksDict[processId] = process;

#if DEBUG
            //Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged after mTasksDict.Count = {mTasksDict.Count}");
#endif
        }

        private void ProcessForbiden(ProxyForNPCResourceProcess process)
        {
#if DEBUG
            //LogInstance.Log($"NPCThreadSafeMeshController ProcessForbiden npcMeshTask = {npcMeshTask}");
#endif

            process.State = StateOfNPCProcess.Canceled;
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
