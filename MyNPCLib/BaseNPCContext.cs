using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContext: INPCContext
    {
        public BaseNPCContext(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null)
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
            mBodyResourcesManager = new NPCBodyResourcesManager(mIdFactory, mEntityDictionary);
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

        public INPCResourcesManager Body => mBodyResourcesManager;
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
            LogInstance.Log($"BaseNPCContext AddTypeOfProcess type = {type?.FullName}");
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
            LogInstance.Log($"BaseNPCContext Bootstrap type = {type?.FullName}");
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

            if(type == null)
            {
                return;
            }

            var npcProcessInfo = mStorageOfNPCProcesses.StorageOfNPCProcessInfo.GetNPCProcessInfo(type);

#if DEBUG
            LogInstance.Log($"BaseNPCContext Bootstrap type = {type?.FullName}");
#endif

            if (npcProcessInfo == null)
            {
                return;
            }

            var command = new NPCCommand();
            command.Name = npcProcessInfo.Name;

#if DEBUG
            LogInstance.Log($"BaseNPCContext Bootstrap command = {command}");
#endif

            Send(command);
        }

        public void Bootstrap()
        {
            Bootstrap(null);
        }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCContext Send command = {command}");
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
            LogInstance.Log($"BaseNPCContext Send internalCommand = {internalCommand}");
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
            LogInstance.Log($"BaseNPCContext RegProcess process.Id = {process.Id} parentProcessId = {parentProcessId}");
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

                throw new NotImplementedException();
            }
        }

        public void UnRegProcess(INPCProcess process)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCContext UnRegProcess process.Id = {process.Id}");
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

            List<ulong> childrenProcesses = null;

            lock (mProcessesDictLockObj)
            {
                if (!mProcessesDict.ContainsKey(id))
                {
                    return;
                }

                mProcessesDict.Remove(id);

                if()
                {
                    childrenProcesses = mParentChildrenProcessesDict[id];
                }

                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("BaseNPCContext Dispose");
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

            lock (mProcessesDictLockObj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
