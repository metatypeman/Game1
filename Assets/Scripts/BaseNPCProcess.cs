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

    public abstract class BaseNPCProcess : IDisposable
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
                lock (mContextLockObj)
                {
                    return mContext;
                }
            }

            set
            {
                lock (mDisposeLockObj)
                {
                    if (mIsDisposed)
                    {
                        return;
                    }
                }

                lock (mContextLockObj)
                {
                    if (mContext == value)
                    {
                        return;
                    }

                    if (Status != NPCProcessStatus.WaitingToRun)
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
                lock (mCurrentIdLockObj)
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
                lock (mStatusLockObj)
                {
                    return mStatus;
                }
            }

            protected set
            {
                lock (mDisposeLockObj)
                {
                    if (mIsDisposed)
                    {
                        return;
                    }
                }

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
                switch (Status)
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
            Debug.Log("BaseNPCProcess Begin Run");
#endif
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            NRun();

#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess End Run");
#endif
        }

        public void RunAsync()
        {
#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess Begin RunAsync");
#endif
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            mTask = Task.Run(() => {
                NRun();
            });

#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess End RunAsync");
#endif
        }

        protected Task mTask;

        private void NRun()
        {
#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess Begin NRun");
#endif

            Status = NPCProcessStatus.Running;

            OnRun();

            Status = NPCProcessStatus.RanToCompletion;

#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess End NRun");
#endif
        }

        protected abstract void OnRun();

        protected void WaitProsesses(List<BaseNPCProcess> processesList)
        {
#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess Begin WaitProsesses");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            var oldStatus = Status;

            while (true)
            {
                lock (mDisposeLockObj)
                {
                    if (mIsDisposed)
                    {
                        return;
                    }
                }

                if (processesList.All(p => p.IsExecuting))
                {
                    continue;
                }

                break;
            }

            Status = oldStatus;

#if UNITY_EDITOR
            Debug.Log("BaseNPCProcess End WaitProsesses");
#endif
        }

        protected NPCMeshTask Execute(IMoveHumanoidCommand command)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            return Context.Execute(command, CurrentId);
        }

        protected NPCMeshTask Execute(IMoveHumanoidCommandsPackage package)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            return Context.Execute(package, CurrentId);
        }

        private object mDisposeLockObj = new object();
        private bool mIsDisposed;

        protected bool IsDisposed
        {
            get
            {
                lock (mDisposeLockObj)
                {
                    return mIsDisposed;
                }
            }
        }

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
            Debug.Log("BaseNPCProcess Dispose");
#endif
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}
