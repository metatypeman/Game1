using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum NPCMeshTaskState
    {
        WaitWaitingToRun,
        Running,
        RanToCompletion,
        Canceled,
        Faulted
    }

    public delegate void NPCMeshTaskStateChanged(NPCMeshTaskState state);

    public class NPCMeshTask : IObjectToString
    {
        public int TaskId { get; set; }
        public int ProcessId { get; set; }

        private object mStateLockObj = new object();
        private NPCMeshTaskState mState = NPCMeshTaskState.WaitWaitingToRun;
        public NPCMeshTaskState State
        {
            get
            {
                lock(mStateLockObj)
                {
                    return mState;
                }
            }

            set
            {
                lock (mStateLockObj)
                {
                    if(mState == value)
                    {
                        return;
                    }

                    mState = value;

                    Task.Run(() => {
                        OnStateChanged?.Invoke(mState);
                        switch(mState)
                        {
                            case NPCMeshTaskState.WaitWaitingToRun:
                                break;

                            case NPCMeshTaskState.Running:
                                OnStateChangedToRunning?.Invoke();
                                break;

                            case NPCMeshTaskState.RanToCompletion:
                                OnStateChangedToRanToCompletion?.Invoke();
                                break;

                            case NPCMeshTaskState.Canceled:
                                OnStateChangedToCanceled?.Invoke();
                                break;

                            case NPCMeshTaskState.Faulted:
                                OnStateChangedToFaulted?.Invoke();
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(State), mState, null);
                        }
                    });
                }
            }
        }

        public bool IsExecuting
        {
            get
            {
                lock (mStateLockObj)
                {
                    switch (mState)
                    {
                        case NPCMeshTaskState.WaitWaitingToRun:
                        case NPCMeshTaskState.Running:
                            return true;
                    }

                    return false;
                }
            }
        }

        public event NPCMeshTaskStateChanged OnStateChanged;
        public event Action OnStateChangedToRunning;
        public event Action OnStateChangedToRanToCompletion;
        public event Action OnStateChangedToCanceled;
        public event Action OnStateChangedToFaulted;

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCMeshTask)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCMeshTask)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(TaskId)} = {TaskId}");
            sb.AppendLine($"{spaces}{nameof(ProcessId)} = {ProcessId}");
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            return sb.ToString();
        }
    }

    public enum NPCMeshTaskResulutionKind
    {
        Unknow,
        Allow,
        AllowAdd,
        Forbiden
    }

    public class DisagreementByHStateInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.HState;
        public List<int> CurrentProcessesId { get; set; }
        public HumanoidHState CurrentValue { get; set; } = HumanoidHState.Stop;
        public int TargetProcessId { get; set; }
        public HumanoidHState TargetValue { get; set; } = HumanoidHState.Stop;

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByHStateInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByHStateInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if(CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach(var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }

    public class DisagreementByTargetPositionInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.TargetPosition;
        public List<int> CurrentProcessesId { get; set; }
        public Vector3? CurrentValue {get; set;}
        public int TargetProcessId { get; set; }
        public Vector3? TargetValue { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByTargetPositionInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByTargetPositionInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }

    public class DisagreementByVStateInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.VState;
        public List<int> CurrentProcessesId { get; set; }
        public HumanoidVState CurrentValue { get; set; } = HumanoidVState.Ground;
        public int TargetProcessId { get; set; }
        public HumanoidVState TargetValue { get; set; } = HumanoidVState.Ground;

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByVStateInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByVStateInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }

    public class DisagreementByHandsStateInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.HandsState;
        public List<int> CurrentProcessesId { get; set; }
        public HumanoidHandsState CurrentValue { get; set; } = HumanoidHandsState.FreeHands;
        public int TargetProcessId { get; set; }
        public HumanoidHandsState TargetValue { get; set; } = HumanoidHandsState.FreeHands;

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByHandsStateInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByHandsStateInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }

    public class DisagreementByHandsActionStateInfo : IObjectToString
    {
        public HumanoidStateKind Kind => HumanoidStateKind.HandsActionState;
        public List<int> CurrentProcessesId { get; set; }
        public HumanoidHandsActionState CurrentValue { get; set; } = HumanoidHandsActionState.Empty;
        public int TargetProcessId { get; set; }
        public HumanoidHandsActionState TargetValue { get; set; } = HumanoidHandsActionState.Empty;

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(DisagreementByHandsActionStateInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(DisagreementByHandsActionStateInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (CurrentProcessesId == null)
            {
                sb.AppendLine($"{spaces}{nameof(CurrentProcessesId)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(CurrentProcessesId)}");
                foreach (var currentProcessId in CurrentProcessesId)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(currentProcessId)} = {currentProcessId}");
                }
                sb.AppendLine($"{spaces}End {nameof(CurrentProcessesId)}");
            }
            sb.AppendLine($"{spaces}{nameof(CurrentValue)} = {CurrentValue}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            sb.AppendLine($"{spaces}{nameof(TargetValue)} = {TargetValue}");
            return sb.ToString();
        }
    }

    public class NPCMeshTaskResulution : IObjectToString
    {
        public NPCMeshTaskResulutionKind Kind { get; set; } = NPCMeshTaskResulutionKind.Unknow;
        public int TargetProcessId { get; set; }
        public TargetStateOfHumanoidController TargetState { get; set; }
        public DisagreementByHStateInfo DisagreementByHState { get; set; }
        public DisagreementByTargetPositionInfo DisagreementByTargetPosition { get; set; }
        public DisagreementByVStateInfo DisagreementByVState { get; set; }
        public DisagreementByHandsStateInfo DisagreementByHandsState { get; set; }
        public DisagreementByHandsActionStateInfo DisagreementByHandsActionState { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCMeshTaskResulution)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCMeshTaskResulution)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");

            if(TargetState == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetState)} = null");
            }
            else
            {
                sb.Append(TargetState.ToString(nextN));
            }

            if (DisagreementByHState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHState.ToString(nextN));
            }

            if (DisagreementByTargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByTargetPosition)} = null");
            }
            else
            {
                sb.Append(DisagreementByTargetPosition.ToString(nextN));
            }

            if (DisagreementByVState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByVState)} = null");
            }
            else
            {
                sb.Append(DisagreementByVState.ToString(nextN));
            }

            if (DisagreementByHandsState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHandsState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHandsState.ToString(nextN));
            }

            if (DisagreementByHandsActionState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHandsActionState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHandsActionState.ToString(nextN));
            }

            return sb.ToString();
        }
    }

    public class NPCThreadSafeMeshController : IDisposable
    {
        public NPCThreadSafeMeshController(IMoveHumanoidController movehumanoidController, NPCProcessesContext context)
        {
            mMoveHumanoidController = movehumanoidController;
            mContext = context;
            mMoveHumanoidController.OnHumanoidStatesChanged += OnHumanoidStatesChanged;
        }

        private void OnHumanoidStatesChanged(List<HumanoidStateKind> changedStates)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            Task.Run(() => {
#if UNITY_EDITOR
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged Begin changedStates");
                foreach (var changedState in changedStates)
                {
                    Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged changedState = {changedState}");
                }
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged End changedStates");
#endif

                var displacedProcessesIdList = new List<int>();

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

                        default: throw new ArgumentOutOfRangeException(nameof(changedState), changedState, null);
                    }
                }

                displacedProcessesIdList = displacedProcessesIdList.Distinct().ToList();

#if UNITY_EDITOR
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged displacedProcessesIdList.Count = {displacedProcessesIdList.Count}");
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged before mTasksDict.Count = {mTasksDict.Count}");
#endif

                foreach (var displacedProcessId in displacedProcessesIdList)
                {
#if UNITY_EDITOR
                    Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged displacedProcessId = {displacedProcessId}");
#endif

                    var targetTask = mTasksDict[displacedProcessId];
                    mTasksDict.Remove(displacedProcessId);
                    targetTask.State = NPCMeshTaskState.RanToCompletion;
                }

#if UNITY_EDITOR
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged after mTasksDict.Count = {mTasksDict.Count}");
#endif
            });
        }

        private IMoveHumanoidController mMoveHumanoidController;
        private NPCProcessesContext mContext;

        public NPCMeshTask Execute(IMoveHumanoidCommandsPackage package, int processId)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            var result = new NPCMeshTask();
            result.ProcessId = processId;
            //result.TaskId = package.TaskId;

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController Execute package = {package} processId = {processId}");
#endif

            var targetState = CreateTargetState(package);

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController Execute targetState = {targetState}");
#endif

            var resolution = CreateResolution(mMoveHumanoidController.States, targetState, processId);

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController Execute resolution = {resolution}");
#endif

            var kindOfResolution = resolution.Kind;

            switch(kindOfResolution)
            {
                case NPCMeshTaskResulutionKind.Allow:
                case NPCMeshTaskResulutionKind.AllowAdd:
                    ProcessAllow(targetState, processId, result, kindOfResolution);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kindOfResolution), kindOfResolution, null);
            }

            return result;
        }

        private TargetStateOfHumanoidController CreateTargetState(IMoveHumanoidCommandsPackage package)
        {
#if UNITY_EDITOR
            //Debug.Log("NPCThreadSafeMeshController CreateTargetState package = " + package);
#endif

            var result = new TargetStateOfHumanoidController();

            var commandsList = package.Commands;

            if (commandsList.Count == 0)
            {
                return result;
            }

            var hStateCommandsList = new List<IHumanoidHStateCommand>();
            var vStateCommandsList = new List<IHumanoidVStateCommand>();
            var handsStateCommandsList = new List<IHumanoidHandsStateCommand>();
            var handsActionStateCommandsList = new List<IHumanoidHandsActionStateCommand>();

            foreach (var command in commandsList)
            {
                var kind = command.Kind;

                switch (kind)
                {
                    case MoveHumanoidCommandKind.HState:
                        hStateCommandsList.Add(command as IHumanoidHStateCommand);
                        break;

                    case MoveHumanoidCommandKind.VState:
                        vStateCommandsList.Add(command as IHumanoidVStateCommand);
                        break;

                    case MoveHumanoidCommandKind.HandsState:
                        handsStateCommandsList.Add(command as IHumanoidHandsStateCommand);
                        break;

                    case MoveHumanoidCommandKind.HandsActionState:
                        handsActionStateCommandsList.Add(command as IHumanoidHandsActionStateCommand);
                        break;

                    default: throw new ArgumentOutOfRangeException("kind", kind, null);
                }
            }

#if UNITY_EDITOR
            //Debug.Log("NPCThreadSafeMeshController Execute hStateCommandsList.Count = " + hStateCommandsList.Count);
            //Debug.Log("NPCThreadSafeMeshController Execute vStateCommandsList.Count = " + vStateCommandsList.Count);
            //Debug.Log("NPCThreadSafeMeshController Execute handsStateCommandsList.Count = " + handsStateCommandsList.Count);
            //Debug.Log("NPCThreadSafeMeshController Execute handsActionStateCommandsList.Count = " + handsActionStateCommandsList.Count);
#endif

            if (hStateCommandsList.Count > 0)
            {
                var targetCommand = hStateCommandsList.First();

#if UNITY_EDITOR
                //Debug.Log("NPCThreadSafeMeshController CreateTargetState targetCommand = " + targetCommand);
#endif

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
                        if (!targetPosition.HasValue)
                        {
                            result.HState = HumanoidHState.Stop;
                        }
                        break;
                }
            }

            if (vStateCommandsList.Count > 0)
            {
                var targetCommand = vStateCommandsList.First();

#if UNITY_EDITOR
                //Debug.Log("NPCThreadSafeMeshController CreateTargetState targetCommand = " + targetCommand);
#endif

                result.VState = targetCommand.State;
            }

            if (handsStateCommandsList.Count > 0)
            {
                var targetCommand = handsStateCommandsList.First();

#if UNITY_EDITOR
                //Debug.Log("NPCThreadSafeMeshController CreateTargetState targetCommand = " + targetCommand);
#endif

                result.HandsState = targetCommand.State;
            }

            if (handsActionStateCommandsList.Count > 0)
            {
                var targetCommand = handsActionStateCommandsList.First();

#if UNITY_EDITOR
                //Debug.Log("NPCThreadSafeMeshController CreateTargetState targetCommand = " + targetCommand);
#endif

                result.HandsActionState = targetCommand.State;
            }

            if(result.HandsState.HasValue)
            {
                var targeHandsState = result.HandsState.Value;

                switch (targeHandsState)
                {
                    case HumanoidHandsState.FreeHands:
                        result.HandsActionState = HumanoidHandsActionState.Empty;
                        break;
                }
            }

            return result;
        }

        private NPCMeshTaskResulution CreateResolution(StatesOfHumanoidController sourceState, TargetStateOfHumanoidController targetState, int processId)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController CreateTargetState sourceState = {sourceState}");
            Debug.Log($"NPCThreadSafeMeshController CreateTargetState targetState = {targetState}");
            Debug.Log($"NPCThreadSafeMeshController CreateTargetState processId = {processId}");
            DumpProcesses();
#endif

            var result = new NPCMeshTaskResulution();
            result.TargetProcessId = processId;
            result.TargetState = targetState;

            var theSame = true;

            if (targetState.HState.HasValue)
            {
                var targetHState = targetState.HState.Value;

                if (mHState.Count == 0)
                {
                    theSame = false;
                }
                else
                { 
                    if(!mHState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;


                    }
                }
            }

            if (targetState.TargetPosition.HasValue)
            {
                var targetPosition = targetState.TargetPosition.Value;

                if(mTargetPosition.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if(!mTargetPosition.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

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

                    }
                }
            }

            /*
        public DisagreementByHStateInfo DisagreementByHState { get; set; }
        public DisagreementByTargetPositionInfo DisagreementByTargetPosition { get; set; }
        public DisagreementByVStateInfo DisagreementByVState { get; set; }
        public DisagreementByHandsStateInfo DisagreementByHandsState { get; set; }
        public DisagreementByHandsActionStateInfo DisagreementByHandsActionState { get; set; }
             */

            if (targetState.HandsState.HasValue)
            {
                var targetHandsState = targetState.HandsState.Value;

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

                    }
                }
            }

            if(result.Kind == NPCMeshTaskResulutionKind.Unknow)
            {
                if(theSame)
                {
                    result.Kind = NPCMeshTaskResulutionKind.AllowAdd;
                }
                else
                {
                    result.Kind = NPCMeshTaskResulutionKind.Allow;
                }
            }

#if UNITY_EDITOR
            Debug.Log("NPCThreadSafeMeshController CreateTargetState NEXT");
            Debug.Log("End NPCThreadSafeMeshController CreateTargetState");
#endif
            return result;
        }

        private List<int> mHState = new List<int>();
        private List<int> mTargetPosition = new List<int>();
        private List<int> mVState = new List<int>();
        private List<int> mHandsState = new List<int>();
        private List<int> mHandsActionState = new List<int>();
        private Dictionary<int, NPCMeshTask> mTasksDict = new Dictionary<int, NPCMeshTask>();

#if UNITY_EDITOR
        private void DumpProcesses()
        {
            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mHState");
            foreach(var item in mHState)
            {
                Debug.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mHState");

            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mTargetPosition");
            foreach (var item in mTargetPosition)
            {
                Debug.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mTargetPosition");

            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mVState");
            foreach (var item in mVState)
            {
                Debug.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mVState");

            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mHandsState");
            foreach (var item in mHandsState)
            {
                Debug.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mHandsState");

            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mHandsActionState");
            foreach (var item in mHandsActionState)
            {
                Debug.Log($"NPCThreadSafeMeshController ProcessAllow item = {item}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mHandsActionState");

            Debug.Log("NPCThreadSafeMeshController ProcessAllow Begin mTasksDict");
            foreach(var kvpItem in mTasksDict)
            {
                var productId = kvpItem.Key;
                var task = kvpItem.Value;

                Debug.Log($"NPCThreadSafeMeshController ProcessAllow productId = {productId} task = {task}");
            }
            Debug.Log("NPCThreadSafeMeshController ProcessAllow End mTasksDict");
        }
#endif

        private void ProcessAllow(TargetStateOfHumanoidController targetState, int processId, NPCMeshTask npcMeshTask, NPCMeshTaskResulutionKind resolutionKind)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController ProcessAllow targetState = {targetState}");
            Debug.Log($"NPCThreadSafeMeshController ProcessAllow processId = {processId}");
#endif

            RegProcessId(targetState, processId, npcMeshTask, resolutionKind);

            mMoveHumanoidController.ExecuteAsync(targetState);

            npcMeshTask.State = NPCMeshTaskState.Running;
        }

        private void RegProcessId(TargetStateOfHumanoidController targetState, int processId, NPCMeshTask npcMeshTask, NPCMeshTaskResulutionKind resolutionKind)
        {
            var displacedProcessesIdList = new List<int>();

            if (targetState.HState.HasValue)
            {
                switch(resolutionKind)
                {
                    case NPCMeshTaskResulutionKind.Allow:
                        displacedProcessesIdList.AddRange(mHState);
                        mHState.Clear();
                        break;

                    case NPCMeshTaskResulutionKind.AllowAdd:
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(resolutionKind), resolutionKind, null);
                }

                if(!mHState.Contains(processId))
                {
                    mHState.Add(processId);
                }     
            }else{
                if(mHState.Contains(processId))
                {
                    mHState.Remove(processId);
                }
            }

            if (targetState.TargetPosition.HasValue)
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

                if(!mTargetPosition.Contains(processId))
                {
                    mTargetPosition.Add(processId);
                }  
            }else{
                if(mTargetPosition.Contains(processId))
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

                if(!mVState.Contains(processId))
                {
                    mVState.Add(processId);
                }  
            }else{
                if(mVState.Contains(processId))
                {
                    mVState.Remove(processId);
                }
            }

            if (targetState.HandsState.HasValue)
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

                if(!mHandsState.Contains(processId))
                {
                    mHandsState.Add(processId);
                }     
            }else{
                if(mHandsState.Contains(processId))
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

                if(!mHandsActionState.Contains(processId))
                {
                    mHandsActionState.Add(processId);
                }               
            }else{
                if(mHandsActionState.Contains(processId))
                {
                    mHandsActionState.Remove(processId);
                }
            }

            if(displacedProcessesIdList.Count > 0)
            {
                displacedProcessesIdList = displacedProcessesIdList.Distinct().ToList();

                foreach (var displacedProcessId in displacedProcessesIdList)
                {
#if UNITY_EDITOR
                    Debug.Log($"NPCThreadSafeMeshController CreateTargetState displacedProcessId = {displacedProcessId}");
#endif

                    if(mHState.Contains(displacedProcessId))
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

                    mTasksDict.Remove(displacedProcessId);
                }
            }

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged before mTasksDict.Count = {mTasksDict.Count}");
#endif

            mTasksDict[processId] = npcMeshTask;

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged after mTasksDict.Count = {mTasksDict.Count}");
#endif
        }

        private object mDisposeLockObj = new object();
        private bool mIsDisposed;

        public void Dispose()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }

                mIsDisposed = true;
            }

#if UNITY_EDITOR
            Debug.Log("NPCThreadSafeMeshController Dispose");
#endif
        }
    }
}
