﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContext: INPCContext
    {
        public BaseNPCContext(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null, IHumanoidBodyController humanoidBodyController = null)
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
            mBodyResourcesManager = new NPCBodyResourcesManager(mIdFactory, mEntityDictionary, humanoidBodyController);
            mLeftHandResourcesManager = new NPCHandResourcesManager(mIdFactory, mEntityDictionary);
            mRightHandResourcesManager = new NPCHandResourcesManager(mIdFactory, mEntityDictionary);
            mStorageOfNPCProcesses = new StorageOfNPCProcesses(mIdFactory, mEntityDictionary, npcProcessInfoCache, this);
        }

        #region private members
        private IdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private NPCBodyResourcesManager mBodyResourcesManager;
        private NPCHandResourcesManager mLeftHandResourcesManager;
        private NPCHandResourcesManager mRightHandResourcesManager;
        private StorageOfNPCProcesses mStorageOfNPCProcesses;
        private Dictionary<ulong, INPCProcess> mProcessesDict = new Dictionary<ulong, INPCProcess>();
        private Dictionary<ulong, List<ulong>> mParentChildrenProcessesDict = new Dictionary<ulong, List<ulong>>();
        private object mProcessesDictLockObj = new object();

        private object mStateLockObj = new object();
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
        public INPCResourcesManager LeftHand => mLeftHandResourcesManager;
        public INPCResourcesManager RightHand => mRightHandResourcesManager;

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
                    return new NotValidAbstractNPCProcess();
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
                return new NotValidAbstractNPCProcess();
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
