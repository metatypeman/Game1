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

        private int mCount;
        private object mCountLockObj = new object();

        public int GetNewProcessId()
        {
            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    return 0;
                }
            }

            lock (mCountLockObj)
            {
                mCount++;
                return mCount;
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
