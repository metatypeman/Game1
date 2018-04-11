using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContext : INPCContext
    {
        public BaseNPCContext(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null, INPCHostContext npcHostContext = null)
        {
            if (entityDictionary == null)
            {
                mEntityDictionary = new EntityDictionary();
            }
            else
            {
                mEntityDictionary = entityDictionary;
            }

            mIdFactory = new IdFactory();
            mBodyResourcesManager = new NPCBodyResourcesManager(mIdFactory, mEntityDictionary, npcHostContext, this); 
            mRightHandResourcesManager = new NPCHandResourcesManager(mIdFactory, mEntityDictionary, npcHostContext, KindOfHand.Right, this);
            mLeftHandResourcesManager = new NPCHandResourcesManager(mIdFactory, mEntityDictionary, npcHostContext, KindOfHand.Left, this);
            mStorageOfNPCProcesses = new StorageOfNPCProcesses(mIdFactory, mEntityDictionary, npcProcessInfoCache, this);
        }

        #region private members
        private readonly IdFactory mIdFactory;
        private readonly IEntityDictionary mEntityDictionary;
        private readonly NPCBodyResourcesManager mBodyResourcesManager;
        private readonly NPCHandResourcesManager mRightHandResourcesManager;
        private readonly NPCHandResourcesManager mLeftHandResourcesManager;       
        private readonly StorageOfNPCProcesses mStorageOfNPCProcesses;
        private readonly Dictionary<ulong, INPCProcess> mProcessesDict = new Dictionary<ulong, INPCProcess>();
        private readonly Dictionary<ulong, List<ulong>> mParentChildrenProcessesDict = new Dictionary<ulong, List<ulong>>();
        private readonly Dictionary<ulong, ulong> mChildParentDict = new Dictionary<ulong, ulong>();
        private readonly object mProcessesDictLockObj = new object();

        private readonly object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        #endregion

        public StateOfNPCContext State
        {
            get
            {
                lock(mStateLockObj)
                {
                    return mState;
                }
            }
        }

        public INPCBodyResourcesManager Body => mBodyResourcesManager;
        public INPCResourcesManager DefaultHand => mRightHandResourcesManager;   
        public INPCResourcesManager RightHand => mRightHandResourcesManager;
        public INPCResourcesManager LeftHand => mLeftHandResourcesManager;

        public bool AddTypeOfProcess<T>()
        {
            return AddTypeOfProcess(typeof(T));
        }

        public bool AddTypeOfProcess(Type type)
        {
#if DEBUG
            //LogInstance.Log($"BaseNPCContext AddTypeOfProcess type = {type?.FullName}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            return mStorageOfNPCProcesses.AddTypeOfProcess(type);
        }

        public void Bootstrap<T>()
        {
            Bootstrap(typeof(T));
        }

        public void Bootstrap(Type type)
        {
#if DEBUG
            //LogInstance.Log($"BaseNPCContext Bootstrap type = {type?.FullName}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    throw new ElementIsNotActiveException();
                }

                if(mState == StateOfNPCContext.Working)
                {
                    return;
                }

                mState = StateOfNPCContext.Working;
            }

            mBodyResourcesManager.Bootstrap();
            mRightHandResourcesManager.Bootstrap();
            mLeftHandResourcesManager.Bootstrap();

            if(type == null)
            {
                return;
            }

            var npcProcessInfo = mStorageOfNPCProcesses.StorageOfNPCProcessInfo.GetNPCProcessInfo(type);

#if DEBUG
            //LogInstance.Log($"BaseNPCContext Bootstrap type = {type?.FullName}");
#endif

            if (npcProcessInfo == null)
            {
                return;
            }

            var command = new NPCCommand();
            command.Name = npcProcessInfo.Name;

#if DEBUG
            //LogInstance.Log($"BaseNPCContext Bootstrap command = {command}");
#endif

            Send(command);
        }

        public virtual void Bootstrap()
        {
            Bootstrap(null);
        }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            //LogInstance.Log($"BaseNPCContext Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return new NotValidAbstractNPCProcess(this);
                }

                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, mEntityDictionary);

#if DEBUG
            //LogInstance.Log($"BaseNPCContext Send internalCommand = {internalCommand}");
#endif

            var npcProcess = mStorageOfNPCProcesses.GetProcess(internalCommand);

            if (npcProcess == null)
            {
                return new NotValidAbstractNPCProcess(this);
            }

            return npcProcess.RunAsync();
        }

        public void RegProcess(INPCProcess process, ulong parentProcessId)
        {
#if DEBUG
            //LogInstance.Log($"BaseNPCContext RegProcess process.Id = {process.Id} parentProcessId = {parentProcessId}");
#endif
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }
            }

            if(process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            var id = process.Id;

            if (id == 0)
            {
                throw new ArgumentNullException("process.Id");
            }

            lock(mProcessesDictLockObj)
            {
                if(mProcessesDict.ContainsKey(id))
                {
                    return;
                }

                mProcessesDict[id] = process;

                if(parentProcessId > 0)
                {
                    List<ulong> childrenProcessesIdList = null;

                    if(mParentChildrenProcessesDict.ContainsKey(parentProcessId))
                    {
                        childrenProcessesIdList = mParentChildrenProcessesDict[parentProcessId];

                        if(!childrenProcessesIdList.Contains(id))
                        {
                            childrenProcessesIdList.Add(id);
                        }
                    }
                    else
                    {
                        childrenProcessesIdList = new List<ulong>() { id };
                        mParentChildrenProcessesDict[parentProcessId] = childrenProcessesIdList;
                    }

                    mChildParentDict[id] = parentProcessId;
                }            
            }
        }

        public void UnRegProcess(INPCProcess process)
        {
#if DEBUG
            //LogInstance.Log($"BaseNPCContext UnRegProcess process.Id = {process.Id}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }
            }

            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            var id = process.Id;

            if (id == 0)
            {
                throw new ArgumentNullException("process.Id");
            }

            List<ulong> childrenProcessesIdList = null;

            lock (mProcessesDictLockObj)
            {
                if (!mProcessesDict.ContainsKey(id))
                {
                    return;
                }

                mProcessesDict.Remove(id);

                if(mParentChildrenProcessesDict.ContainsKey(id))
                {
                    childrenProcessesIdList = mParentChildrenProcessesDict[id];
                    mParentChildrenProcessesDict.Remove(id);
                } 
                
                if(mChildParentDict.ContainsKey(id))
                {
                    mChildParentDict.Remove(id);
                }

                mBodyResourcesManager.UnRegProcess(id);

                mLeftHandResourcesManager.UnRegProcess(id);
                mRightHandResourcesManager.UnRegProcess(id);              
            }

            if(childrenProcessesIdList != null)
            {
                foreach(var childProcessId in childrenProcessesIdList)
                {
                    var childProcess = mProcessesDict[childProcessId];
                    childProcess.Dispose();
                }               
            }        
        }

        public INPCProcess GetParentProcess(ulong childProcessId)
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return null;
                }
            }

            if(childProcessId == 0)
            {
                throw new ArgumentNullException(nameof(childProcessId));
            }

            lock (mProcessesDictLockObj)
            {
                if(mChildParentDict.ContainsKey(childProcessId))
                {
                    var parentId = mChildParentDict[childProcessId];
                    return mProcessesDict[parentId];
                }

                return null;
            }
        }

        public NPCMeshTaskResulutionKind ApproveNPCMeshTaskExecute(NPCResourcesResulution existingsNPCMeshTaskResulution)
        {
#if DEBUG
            //LogInstance.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute existingsNPCMeshTaskResulution = {existingsNPCMeshTaskResulution}");
#endif

            var targetProcessId = existingsNPCMeshTaskResulution.TargetProcessId;

            var tmpExistingProcessesIdList = new List<ulong>();

            var disagreementByHState = existingsNPCMeshTaskResulution.DisagreementByHState;

            if (disagreementByHState != null)
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

            var targetProcessInfo = mProcessesDict[targetProcessId];

            var targetPriority = targetProcessInfo.GlobalPriority;


#if DEBUG
            //LogInstance.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute targetPriority = {targetPriority}");
#endif
            foreach (var existingProcessesId in tmpExistingProcessesIdList)
            {
#if DEBUG
                //LogInstance.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute existingProcessesId = {existingProcessesId}");
#endif

                var currentProcessInfo = mProcessesDict[existingProcessesId];

#if DEBUG
                //LogInstance.Log($"NPCProcessesContext ApproveNPCMeshTaskExecute currentProcessInfo.GlobalPriority = {currentProcessInfo.GlobalPriority}");
#endif

                if (currentProcessInfo.GlobalPriority > targetPriority)
                {
                    return NPCMeshTaskResulutionKind.Forbiden;
                }
            }

            return NPCMeshTaskResulutionKind.Allow;//tmp
        }

        public virtual object NoTypedBlackBoard => null;

        public void Dispose()
        {
#if DEBUG
            //LogInstance.Log("BaseNPCContext Dispose");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCContext.Destroyed;
            }

            mBodyResourcesManager.Dispose();
            mLeftHandResourcesManager.Dispose();
            mRightHandResourcesManager.Dispose();
           
            mStorageOfNPCProcesses.Dispose();

            foreach(var processesKVPItem in mProcessesDict)
            {
                processesKVPItem.Value.Dispose();
            }
        }
    }
}
