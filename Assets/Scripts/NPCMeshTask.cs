using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNPCLib;

namespace Assets.Scripts
{
    public enum NPCMeshTaskState
    {
        WaitWaitingToRun,
        Running,
        RanToCompletion,
        CanceledByOwner,
        CanceledByHost,
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
                lock (mStateLockObj)
                {
                    return mState;
                }
            }

            set
            {
                lock (mStateLockObj)
                {
                    if (mState == value)
                    {
                        return;
                    }

                    mState = value;

                    Task.Run(() => {
                        OnStateChanged?.Invoke(mState);
                        switch (mState)
                        {
                            case NPCMeshTaskState.WaitWaitingToRun:
                                break;

                            case NPCMeshTaskState.Running:
                                OnStateChangedToRunning?.Invoke();
                                break;

                            case NPCMeshTaskState.RanToCompletion:
                                OnStateChangedToRanToCompletion?.Invoke();
                                break;

                            case NPCMeshTaskState.CanceledByOwner:
                                OnStateChangedToCanceledByOwner?.Invoke();
                                break;

                            case NPCMeshTaskState.CanceledByHost:
                                OnStateChangedToCanceledByHost?.Invoke();
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
        public event Action OnStateChangedToCanceledByOwner;
        public event Action OnStateChangedToCanceledByHost;
        public event Action OnStateChangedToFaulted;

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(TaskId)} = {TaskId}");
            sb.AppendLine($"{spaces}{nameof(ProcessId)} = {ProcessId}");
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            return sb.ToString();
        }
    }
}
