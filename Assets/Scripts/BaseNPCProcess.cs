using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum NPCProcessStatus
    {
        WaitingToRun,
        Running,
        ActiveRunning,
        WaitingForActivation,
        WaitingForChildrenToComplete,
        RanToCompletion,
        Canceled,
        Faulted
    }

    public abstract class BaseNPCProcess
    {
        protected BaseNPCProcess()
        {
        }

        protected BaseNPCProcess(NPCProcessesContext context)
        {
            Context = context;
        }

        private NPCProcessesContext mContext;
        private object mContextLockObj = new object();

        public NPCProcessesContext Context
        {
            get
            {
                lock(mContextLockObj)
                {
                    return mContext;
                }
            }

            set
            {
                lock (mContextLockObj)
                {
                    if (mContext == value)
                    {
                        return;
                    }

                    if(Status != NPCProcessStatus.WaitingToRun)
                    {
                        return;
                    }

                    var oldContext = mContext;
                    mContext = value;

                    oldContext?.RemoveChild(this);

                    if (mContext == null)
                    {
                        return;
                    }

                    mContext.AddChild(this);

                    CurrentId = mContext.GetNewProcessId();

#if UNITY_EDITOR
                    Debug.Log($"BaseNPCProcess Context mCurrentId = {mCurrentId}");
#endif
                }
            }
        }

        private int mCurrentId;
        private object mCurrentIdLockObj = new object();

        protected int CurrentId
        {
            get
            {
                lock(mCurrentIdLockObj)
                {
                    return mCurrentId;
                }
            }

            set
            {
                lock (mCurrentIdLockObj)
                {
                    mCurrentId = value;
                }
            }
        }

        private NPCProcessStatus mStatus = NPCProcessStatus.WaitingToRun;
        private object mStatusLockObj = new object();

        public NPCProcessStatus Status
        {
            get
            {
                lock(mStatusLockObj)
                {
                    return mStatus;
                }
            }

            protected set
            {
                lock (mStatusLockObj)
                {
                    mStatus = value;
                }
            }
        }

        public bool IsExecuting
        {
            get
            {
                switch(Status)
                {
                    case NPCProcessStatus.RanToCompletion:
                    case NPCProcessStatus.Canceled:
                    case NPCProcessStatus.Faulted:
                        return false;
                }

                return true;
            }
        }

        public void Run()
        {
#if UNITY_EDITOR
            Debug.Log("Begin Run");
#endif
            NRun();

#if UNITY_EDITOR
            Debug.Log("End Run");
#endif
        }

        public void RunAsync()
        {
#if UNITY_EDITOR
            Debug.Log("Begin RunAsync");
#endif
            mTask = Task.Run(() => {
                NRun();
            });

#if UNITY_EDITOR
            Debug.Log("End RunAsync");
#endif
        }

        protected Task mTask;

        private void NRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin NRun");
#endif

            Status = NPCProcessStatus.Running;

            OnRun();

            Status = NPCProcessStatus.RanToCompletion;

#if UNITY_EDITOR
            Debug.Log("End NRun");
#endif
        }

        protected abstract void OnRun();

        public void WaitProsesses(List<BaseNPCProcess> processesList)
        {
#if UNITY_EDITOR
            Debug.Log("Begin WaitProsesses");
#endif

            var oldStatus = Status;

            while(true)
            {
                if(processesList.All(p => p.IsExecuting))
                {
                    continue;
                }

                break;
            }

            Status = oldStatus;

#if UNITY_EDITOR
            Debug.Log("End WaitProsesses");
#endif
        }

        protected NPCMeshTask Execute(IMoveHumanoidCommand command)
        {
            return Context.Execute(command, CurrentId);
        }

        protected NPCMeshTask Execute(IMoveHumanoidCommandsPackage package)
        {
            return Context.Execute(package, CurrentId);
        }
    }
}
