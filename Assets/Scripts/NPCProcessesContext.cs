using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCProcessesContext
    {
        public NPCProcessesContext(IMoveHumanoidController movehumanoidController)
        {
            mMeshController = new NPCThreadSafeMeshController(movehumanoidController, this);
        }

        private NPCSimpleDI mSimpleDI = new NPCSimpleDI();
        private NPCThreadSafeMeshController mMeshController;

        private List<BaseNPCProcess> mChildProcessesList = new List<BaseNPCProcess>();

        public void AddChild(BaseNPCProcess process)
        {
            if(process == null)
            {
                return;
            }

            if(!mChildProcessesList.Contains(process))
            {
                mChildProcessesList.Add(process);
                if (process.Context != this)
                {
                    process.Context = this;
                }           
            }
        }

        public void RemoveChild(BaseNPCProcess process)
        {
            if (process == null)
            {
                return;
            }

            if(mChildProcessesList.Contains(process))
            {
                mChildProcessesList.Remove(process);
                if(process.Context == this)
                {
                    process.Context = null;
                }
            }
        }

        private int mCount;
        private object mCountLockObj = new object();

        public int GetNewProcessId()
        {
            lock(mCountLockObj)
            {
                mCount++;
                return mCount;
            }
        }

        public NPCMeshTask Execute(IMoveHumanoidCommand command, int processId)
        {
            var commandsPackage = new MoveHumanoidCommandsPackage();
            commandsPackage.Commands.Add(command);
            return Execute(commandsPackage, processId);
        }

        public NPCMeshTask Execute(IMoveHumanoidCommandsPackage package, int processId)
        {
#if UNITY_EDITOR
            Debug.Log($"NPCProcessesContext Execute package = {package} processId = {processId}");
#endif

            return mMeshController.Execute(package);
        }
    }
}
