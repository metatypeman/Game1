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

    public class NPCMeshTask : IObjectToString
    {
        public int TaskId { get; set; }
        public int ProcessId { get; set; }
        public NPCMeshTaskState State { get; set; } = NPCMeshTaskState.WaitWaitingToRun;

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

    public class NPCMeshTaskResulution : IObjectToString
    {
        public NPCMeshTaskResulutionKind Kind { get; set; } = NPCMeshTaskResulutionKind.Unknow;

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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            //sb.AppendLine($"{spaces}{nameof(ProcessId)} = {ProcessId}");
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
#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged Begin changedStates");
            foreach(var changedState in changedStates)
            {
                Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged changedState = {changedState}");
            }
            Debug.Log($"NPCThreadSafeMeshController OnHumanoidStatesChanged End changedStates");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }
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
                    ProcessAllow(targetState, processId);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kindOfResolution), kindOfResolution, null);
            }

            //mMoveHumanoidController.ExecuteAsync(package);

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
#endif

            var result = new NPCMeshTaskResulution();

            var theSame = true;

            if (targetState.HState.HasValue)
            {
                var targetHState = targetState.HState.Value;

                if (mHState.Count == 0)
                {
                    theSame = false;
                }
                else { 
                    if(!mHState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;
                    }
                }
            }

            if (targetState.TargetPosition.HasValue)
            {

            }

            if (targetState.VState.HasValue)
            {
                var targetVState = targetState.VState.Value;

                //result.VState = targetState.VState.Value;
            }

            if (targetState.HandsState.HasValue)
            {
                var targetHandsState = targetState.HandsState.Value;

                //result.HandsState = targetState.HandsState.Value;
            }

            if (targetState.HandsActionState.HasValue)
            {
                var targetHandsActionState = targetState.HandsActionState.Value;

                //result.HandsActionState = targetState.HandsActionState.Value;
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

            return result;
        }

        private List<int> mHState = new List<int>();
        private List<int> mTargetPosition = new List<int>();
        private List<int> mVState = new List<int>();
        private List<int> mHandsState = new List<int>();
        private List<int> mHandsActionState = new List<int>();

        private void ProcessAllow(TargetStateOfHumanoidController targetState, int processId)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController ProcessAllow targetState = {targetState}");
            Debug.Log($"NPCThreadSafeMeshController ProcessAllow processId = {processId}");
#endif

            RegProcessId(targetState, processId);

            mMoveHumanoidController.ExecuteAsync(targetState);
        }

        private void RegProcessId(TargetStateOfHumanoidController targetState, int processId)
        {
            if (targetState.HState.HasValue)
            {
                mHState.Add(processId);
            }

            if (targetState.TargetPosition.HasValue)
            {
                mTargetPosition.Add(processId);
            }

            if (targetState.VState.HasValue)
            {
                mVState.Add(processId);
            }

            if (targetState.HandsState.HasValue)
            {
                mHandsState.Add(processId);
            }

            if (targetState.HandsActionState.HasValue)
            {
                mHandsActionState.Add(processId);
            }
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
