﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum OldNPCProcessStatus
    {
        WaitingToRun,
        Running,
        WaitingForNPCMeshActivation,
        NPCMeshRunning,
        RanToCompletion,
        Canceled,
        Faulted
    }

    public static class OldBaseNPCProcessPriorities
    {
        public const float Highest = 1.0F;
        public const float AboveNormal = 0.75F;
        public const float Normal = 0.5F;
        public const float BelowNormal = 0.2F;
        public const float Lowest = 0.01F;
    }
    
    public abstract class OldBaseNPCProcess : IDisposable
    {
        protected OldBaseNPCProcess()
        {
        }

        protected OldBaseNPCProcess(OldNPCProcessesContext context)
        {
            Context = context;
        }

        private OldNPCProcessesContext mContext;
        private object mContextLockObj = new object();

        public OldNPCProcessesContext Context
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

                    if (Status != OldNPCProcessStatus.WaitingToRun)
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
                    //Debug.Log($"BaseNPCProcess Context mCurrentId = {mCurrentId}");
#endif
                }

                OnChangeContext();
            }
        }

        protected virtual void OnChangeContext()
        {
        }

        private OldBaseNPCProcess mParentProcess;
        private List<OldBaseNPCProcess> mChildrenProcesses = new List<OldBaseNPCProcess>();
        private object mChildrenProcessesLockObj = new object();
        
        public OldBaseNPCProcess ParentProcess
        {
            get
            {
                lock(mChildrenProcessesLockObj)
                {
                    return mParentProcess;
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

                lock (mChildrenProcessesLockObj)
                {
                    if (mParentProcess == value)
                    {
                        return;
                    }

                    if (mParentProcess == this)
                    {
                        return;
                    }

                    if (Status != OldNPCProcessStatus.WaitingToRun)
                    {
                        return;
                    }

                    var oldParent = mParentProcess;

                    mParentProcess = value;
                    oldParent?.RemoveChild(this);
                    mParentProcess?.AddChild(this);
                }
            }
        }

        public void AddChild(OldBaseNPCProcess process)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            lock (mChildrenProcessesLockObj)
            {
                if (process == null)
                {
                    return;
                }

                if (process == this)
                {
                    return;
                }

                if(!mChildrenProcesses.Contains(process))
                {
                    mChildrenProcesses.Add(process);
                    if(process.ParentProcess != this)
                    {
                        process.ParentProcess = this;
                    }
                }
            }
        }

        public void RemoveChild(OldBaseNPCProcess process)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            lock (mChildrenProcessesLockObj)
            {
                if (process == null)
                {
                    return;
                }

                if (process == this)
                {
                    return;
                }

                if (mChildrenProcesses.Contains(process))
                {
                    mChildrenProcesses.Remove(process);
                    if(process.ParentProcess == this)
                    {
                        process.ParentProcess = null;
                    }
                }
            }
        }

        private float mLocalPriority = OldBaseNPCProcessPriorities.Normal;
        private object mPriorityLockObj = new object();
        
        public float LocalPriority
        {
            get
            {
                lock(mPriorityLockObj)
                {
                    return mLocalPriority;
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
                
                lock(mPriorityLockObj)
                {
                    mLocalPriority = value;
                }
            }
        }
        
        public float GlobalPriority
        {
            get
            {
                lock(mPriorityLockObj)
                {
                    var result = mLocalPriority;

                    if(ParentProcess != null)
                    {
                        result *= ParentProcess.GlobalPriority;
                    }

                    return result;
                }
            }
        }
        
        private int mCurrentId;
        private object mCurrentIdLockObj = new object();

        public int CurrentId
        {
            get
            {
                lock (mCurrentIdLockObj)
                {
                    return mCurrentId;
                }
            }

            protected set
            {
                lock (mCurrentIdLockObj)
                {
                    mCurrentId = value;
                }
            }
        }

        private OldNPCProcessStatus mStatus = OldNPCProcessStatus.WaitingToRun;
        private object mStatusLockObj = new object();

        public OldNPCProcessStatus Status
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
                    case OldNPCProcessStatus.RanToCompletion:
                    case OldNPCProcessStatus.Canceled:
                    case OldNPCProcessStatus.Faulted:
                        return false;
                }

                return true;
            }
        }

        public void Run()
        {
#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess Begin Run");
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
            //Debug.Log("BaseNPCProcess End Run");
#endif
        }

        public void RunAsync()
        {
#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess Begin RunAsync");
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
            //Debug.Log("BaseNPCProcess End RunAsync");
#endif
        }

        protected Task mTask;

        private void NRun()
        {
#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess Begin NRun");
#endif

            Status = OldNPCProcessStatus.Running;

            OnRun();

            Status = OldNPCProcessStatus.RanToCompletion;

#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess End NRun");
#endif
        }

        protected abstract void OnRun();

        protected void WaitProsesses(List<OldBaseNPCProcess> processesList)
        {
#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess Begin WaitProsesses");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

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

#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess End WaitProsesses");
#endif
        }

        protected void WaitNPCMeshTask(NPCMeshTask task)
        {
#if UNITY_EDITOR
            //Debug.Log($"BaseNPCProcess Begin WaitNPCMeshTask task = {task}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            while (true)
            {
                lock (mDisposeLockObj)
                {
                    if (mIsDisposed)
                    {
                        return;
                    }
                }

                if (task.IsExecuting)
                {
                    continue;
                }

                break;
            }

#if UNITY_EDITOR
            //Debug.Log("BaseNPCProcess End WaitNPCMeshTask");
#endif
        }

        protected NPCMeshTask Execute(IInternalMoveHumanoidCommand command)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            var task = Context.Execute(command, CurrentId);
            PostProcessExecutedTask(task);
            return task;
        }

        protected NPCMeshTask Execute(IOldMoveHumanoidCommandsPackage package)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            var task = Context.Execute(package, CurrentId);
            PostProcessExecutedTask(task);
            return task;
        }

        private void PostProcessExecutedTask(NPCMeshTask task)
        {
#if UNITY_EDITOR
            //Debug.Log($"BaseNPCProcess PostProcessExecutedTask task = {task}");
#endif

            Status = ConvertNPCMeshTaskStateToActiveNPCProcessStatus(task.State);

            task.OnStateChanged += (NPCMeshTaskState state) => {
#if UNITY_EDITOR
                //Debug.Log($"BaseNPCProcess PostProcessExecutedTask OnStateChanged state = {state}");
#endif
                Status = ConvertNPCMeshTaskStateToActiveNPCProcessStatus(state);
            };
        }

        private OldNPCProcessStatus ConvertNPCMeshTaskStateToActiveNPCProcessStatus(NPCMeshTaskState npcMeshTaskState)
        {
#if UNITY_EDITOR
            //Debug.Log($"BaseNPCProcess PostProcessExecutedTask ConvertNPCMeshTaskStateToActiveNPCProcessStatus npcMeshTaskState = {npcMeshTaskState}");
#endif

            switch(npcMeshTaskState)
            {
                case NPCMeshTaskState.WaitWaitingToRun:
                    return OldNPCProcessStatus.WaitingForNPCMeshActivation;

                case NPCMeshTaskState.Running:
                    return OldNPCProcessStatus.NPCMeshRunning;

                case NPCMeshTaskState.RanToCompletion:
                    return OldNPCProcessStatus.Running;

                case NPCMeshTaskState.CanceledByHost:
                    return OldNPCProcessStatus.Running;

                case NPCMeshTaskState.CanceledByOwner:
                    return OldNPCProcessStatus.Running;

                case NPCMeshTaskState.Faulted:
                    return OldNPCProcessStatus.Running;

                default: throw new ArgumentOutOfRangeException(nameof(npcMeshTaskState), npcMeshTaskState, null);
            }
        }

        protected bool InfinityCycleCondition
        {
            get
            {
                return !IsDisposed;
            }
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
            //Debug.Log("BaseNPCProcess Dispose");
#endif
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}