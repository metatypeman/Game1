using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCProcessesContext: IDisposable
    {
        public NPCProcessesContext(IMoveHumanoidController movehumanoidController)
        {
            mMeshController = new NPCThreadSafeMeshController(movehumanoidController, this);
        }

        private NPCSimpleDI mSimpleDI = new NPCSimpleDI();

        public void RegisterInstance<T>(object instance) where T : class
        {
            mSimpleDI.RegisterInstance<T>(instance);
        }

        public void RemoveInstance<T>() where T : class
        {
            mSimpleDI.RemoveInstance<T>();
        }

        public T GetInstance<T>() where T : class
        {
            return mSimpleDI.GetInstance<T>();
        }

        private NPCThreadSafeMeshController mMeshController;

        private object mChildProcessesListLockObj = new object();
        private List<BaseNPCProcess> mChildProcessesList = new List<BaseNPCProcess>();

        public void AddChild(BaseNPCProcess process)
        {
            lock(mDisposeLockObj)
            {
                if(mIsDisposed)
                {
                    return;
                }
            }

            lock(mChildProcessesListLockObj)
            {
                if (process == null)
                {
                    return;
                }

                if (!mChildProcessesList.Contains(process))
                {
                    mChildProcessesList.Add(process);
                    if (process.Context != this)
                    {
                        process.Context = this;
                    }
                }
            }
        }

        public void RemoveChild(BaseNPCProcess process)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }

            lock (mChildProcessesListLockObj)
            {
                if (process == null)
                {
                    return;
                }

                if (mChildProcessesList.Contains(process))
                {
                    mChildProcessesList.Remove(process);
                    if (process.Context == this)
                    {
                        process.Context = null;
                    }
                }
            }
        }

        private int mProcessIdCount;
        private object mProcessIdCountLockObj = new object();

        public int GetNewProcessId()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return 0;
                }
            }

            lock (mProcessIdCountLockObj)
            {
                mProcessIdCount++;
                return mProcessIdCount;
            }
        }

        private int mTaskIdCount;
        private object mTaskIdCountLockObj = new object();
        
        public int GetNewTaskId()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return 0;
                }
            }
            
            lock(mTaskIdCountLockObj)
            {
                mTaskIdCount++;
                return mTaskIdCount;
            }
        }
        
        public NPCMeshTask Execute(IMoveHumanoidCommand command, int processId)
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            var commandsPackage = new MoveHumanoidCommandsPackage();
            commandsPackage.Commands.Add(command);
            return Execute(commandsPackage, processId);
        }

        public NPCMeshTask Execute(IMoveHumanoidCommandsPackage package, int processId)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCProcessesContext Execute package = {package} processId = {processId}");
#endif
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return null;
                }
            }

            return mMeshController.Execute(package, processId);
        }

        public NPCMeshTaskResulutionKind ApproveNPCMeshTaskExecute(NPCMeshTaskResulution existingsNPCMeshTaskResulution)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute existingsNPCMeshTaskResulution = {existingsNPCMeshTaskResulution}");
#endif

            var targetProcessId = existingsNPCMeshTaskResulution.TargetProcessId;

            var tmpExistingProcessesIdList = new List<int>();

            var disagreementByHState = existingsNPCMeshTaskResulution.DisagreementByHState;

            if(disagreementByHState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHState.CurrentProcessesId);
            }

            var disagreementByTargetPosition = existingsNPCMeshTaskResulution.DisagreementByTargetPosition;

            if (disagreementByTargetPosition != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByTargetPosition.CurrentProcessesId);
            }

            var disagreementByVState = existingsNPCMeshTaskResulution.DisagreementByVState;

            if (disagreementByVState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByVState.CurrentProcessesId);
            }

            var disagreementByHandsState = existingsNPCMeshTaskResulution.DisagreementByHandsState;

            if (disagreementByHandsState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHandsState.CurrentProcessesId);
            }

            var disagreementByHandsActionState = existingsNPCMeshTaskResulution.DisagreementByHandsActionState;

            if (disagreementByHandsActionState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHandsActionState.CurrentProcessesId);
            }

            tmpExistingProcessesIdList = tmpExistingProcessesIdList.Distinct().ToList();

            var targetProcessInfo = mChildProcessesList.FirstOrDefault(p => p.CurrentId == targetProcessId);

            var targetPriority = targetProcessInfo.GlobalPriority;


#if UNITY_EDITOR
            Debug.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute targetPriority = {targetPriority}");
#endif
            foreach (var existingProcessesId in tmpExistingProcessesIdList)
            {
#if UNITY_EDITOR
                Debug.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute existingProcessesId = {existingProcessesId}");
#endif

                var currentProicessInfo = mChildProcessesList.FirstOrDefault(p => p.CurrentId == existingProcessesId);

#if UNITY_EDITOR
                Debug.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute currentProicessInfo.GlobalPriority = {currentProicessInfo.GlobalPriority}");
#endif

                if (currentProicessInfo.GlobalPriority > targetPriority)
                {
                    return NPCMeshTaskResulutionKind.Forbiden;
                }
            }

            return NPCMeshTaskResulutionKind.Allow;//tmp
        }

        public void Die()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return;
                }
            }
            
            mMeshController.Die();
            Dispose();
        }
        
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;

        public void Dispose()
        {
            lock(mDisposeLockObj)
            {
                if(mIsDisposed)
                {
                    return;
                }

                mIsDisposed = true;
            }

#if UNITY_EDITOR
            Debug.Log("NPCProcessesContext Dispose");
#endif

            mMeshController?.Dispose();

            foreach(var childProcess in mChildProcessesList)
            {
                childProcess.Dispose();
            }
        }
    }
}
