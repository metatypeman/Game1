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

        public NPCProcessesContext Context
        {
            get
            {
                return mContext;
            }

            set
            {
                if(mContext == value)
                {
                    return;
                }


            }
        }

        private NPCProcessStatus mStatus = NPCProcessStatus.WaitingToRun;

        public NPCProcessStatus Status => mStatus;

        public bool IsExecuting
        {
            get
            {
                switch(mStatus)
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

            mStatus = NPCProcessStatus.Running;

            OnRun();

            mStatus = NPCProcessStatus.RanToCompletion;

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

            var oldStatus = mStatus;

            while(true)
            {
                if(processesList.All(p => p.IsExecuting))
                {
                    continue;
                }

                break;
            }

            mStatus = oldStatus;

#if UNITY_EDITOR
            Debug.Log("End WaitProsesses");
#endif
        }
    }
}
