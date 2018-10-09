using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingPersistLogicalData;
using MyNPCLib.Logical;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyNPCLib
{
    public class BaseNPCContext : INPCContext
    {
        public BaseNPCContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null, INPCHostContext npcHostContext = null)
        {
            mEntityLogger = entityLogger;

            if (entityDictionary == null)
            {
                mEntityDictionary = new EntityDictionary();
            }
            else
            {
                mEntityDictionary = entityDictionary;
            }
            
#if DEBUG
            //Log($"npcHostContext.SelfEntityId = {npcHostContext?.SelfEntityId} npcHostContext.IsReady = {npcHostContext?.IsReady}");
#endif

            mIdFactory = new IdFactory();

            mNPCHostContext = npcHostContext;

            mBodyResourcesManager = new NPCBodyResourcesManager(mEntityLogger, mIdFactory, mEntityDictionary, npcHostContext, this);
            mRightHandResourcesManager = new NPCHandResourcesManager(mEntityLogger, mIdFactory, mEntityDictionary, npcHostContext, KindOfHand.Right, this);
            mLeftHandResourcesManager = new NPCHandResourcesManager(mEntityLogger, mIdFactory, mEntityDictionary, npcHostContext, KindOfHand.Left, this);
            mStorageOfNPCProcesses = new StorageOfNPCProcesses(mEntityLogger, mIdFactory, mEntityDictionary, npcProcessInfoCache, this);

            mSystemPropertiesDictionary = new SystemPropertiesDictionary(mEntityDictionary);

            mStorageOfSpecialEntities = new StorageOfSpecialEntities();

            mContextOfCGStorage = new ContextOfCGStorage(mEntityDictionary);

            mVisionObjectsStorage = new VisionObjectsStorage(mEntityLogger, mEntityDictionary, npcHostContext, mSystemPropertiesDictionary, mStorageOfSpecialEntities);

            if (npcHostContext != null)
            {
                npcHostContext.OnReady += NpcHostContext_OnReady;
                npcHostContext.BodyHost.OnDie += BodyHost_OnDie;
                npcHostContext.OnLogicalSound += NpcHostContext_OnLogicalSound;
            }

            if (mNPCHostContext != null && mNPCHostContext.IsReady)
            {
                InitLogicalSubSystem();
            }
        }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        private void NpcHostContext_OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            //Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            OnLogicalSound(logicalSoundPackage);
        }

        protected virtual void OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
        }

        private void NpcHostContext_OnReady()
        {
#if DEBUG
            //Log("Begin");
#endif

#if DEBUG
            //Log($"mNPCHostContext.SelfEntityId = {mNPCHostContext.SelfEntityId} mNPCHostContext.IsReady = {mNPCHostContext.IsReady}");
#endif

            InitLogicalSubSystem();
        }

        private void InitLogicalSubSystem()
        {
#if DEBUG
            //Log("Begin");
#endif

            mStorageOfSpecialEntities.SelfEntityId = mNPCHostContext.SelfEntityId;

            mContextOfCGStorage.SetHostStorage(mNPCHostContext.SelfHostStorage);
            mContextOfCGStorage.SetWorldHostStorage(mNPCHostContext.BusOfCGStorages.GeneralStorageWithPublicFacts);

            var visibleObjectsCGStorage = new VisibleObjectsCGStorage(mEntityDictionary, mStorageOfSpecialEntities, mNPCHostContext.BusOfCGStorages);
            mContextOfCGStorage.SetWorldHostStorage(visibleObjectsCGStorage);

            var mainStorage = mContextOfCGStorage.MainCGStorage;

            mVisionObjectsStorage.BusOfHostStorage = mNPCHostContext.BusOfCGStorages;
            mVisionObjectsStorage.LogicalStorage = mainStorage;

#if DEBUG
            //Log("NEXT");
#endif
            mSelfLogicalObject = new SelfLogicalObject(mEntityLogger, mEntityDictionary, mainStorage, mSystemPropertiesDictionary, mNPCHostContext);
            
#if DEBUG
            //Log("NEXT NEXT");
#endif

            lock (mIsReadyLockObj)
            {
                mIsLogicalSubSystemReady = true;
            }

            TryDelayedBootstrap();
        }

        private void BodyHost_OnDie()
        {
            Dispose();
        }

        #region private members
        private IEntityLogger mEntityLogger;
        private INPCHostContext mNPCHostContext;
        private readonly IdFactory mIdFactory;
        private readonly IEntityDictionary mEntityDictionary;
        
        private readonly SystemPropertiesDictionary mSystemPropertiesDictionary;
        private readonly VisionObjectsStorage mVisionObjectsStorage;
        private readonly NPCBodyResourcesManager mBodyResourcesManager;
        private readonly NPCHandResourcesManager mRightHandResourcesManager;
        private readonly NPCHandResourcesManager mLeftHandResourcesManager;
        private readonly StorageOfNPCProcesses mStorageOfNPCProcesses;
        private readonly Dictionary<ulong, INPCProcess> mProcessesDict = new Dictionary<ulong, INPCProcess>();
        private readonly Dictionary<ulong, List<ulong>> mParentChildrenProcessesDict = new Dictionary<ulong, List<ulong>>();
        private readonly Dictionary<ulong, ulong> mChildParentDict = new Dictionary<ulong, ulong>();
        private readonly object mProcessesDictLockObj = new object();
        private ContextOfCGStorage mContextOfCGStorage;
        
        private StorageOfSpecialEntities mStorageOfSpecialEntities;

        private readonly object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        private readonly object mIsReadyLockObj = new object();
        private bool mIsLogicalSubSystemReady;
        private object mNeedDelayedBootstrapLockObj = new object();
        private bool mNeedDelayedBootstrap;
        private Type mDelayedBootstrapType;
        #endregion

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        public ContextOfCGStorage ContextOfCGStorage
        {
            get
            {
                lock (mIsReadyLockObj)
                {
                    return mContextOfCGStorage;
                }
            }
        }

        public ICGStorage MainCGStorage
        {
            get
            {
                lock (mIsReadyLockObj)
                {
                    return mContextOfCGStorage?.MainCGStorage;
                }
            }
        }

        public GlobalCGStorage GlobalCGStorage
        {
            get
            {
                lock (mIsReadyLockObj)
                {
                    return mContextOfCGStorage?.GlobalCGStorage;
                }
            }
        }

        public bool IsLogicalSubSystemReady
        {
            get
            {
                lock (mIsReadyLockObj)
                {
                    return mIsLogicalSubSystemReady;
                }
            }
        }

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
            //Log($"type = {type?.FullName}");
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
            //Log($"type = {type?.FullName}");
#endif

            lock(mIsReadyLockObj)
            {
                if(mIsLogicalSubSystemReady)
                {
                    NBootstrap(type);
                    return;
                }

                lock(mNeedDelayedBootstrapLockObj)
                {
                    mNeedDelayedBootstrap = true;
                    mDelayedBootstrapType = type;
                }
            }
        }

        public virtual void Bootstrap()
        {
            Bootstrap(null);
        }

        private void TryDelayedBootstrap()
        {
#if DEBUG
            //Log($"mNeedDelayedBootstrap = {mNeedDelayedBootstrap}");
#endif

            lock (mNeedDelayedBootstrapLockObj)
            {
                if (!mNeedDelayedBootstrap)
                {
                    return;
                }
            }

            NBootstrap(mDelayedBootstrapType);
        }

        private void NBootstrap(Type type)
        {
#if DEBUG
            //Log($"type = {type?.FullName}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    throw new ElementIsNotActiveException();
                }

                if (mState == StateOfNPCContext.Working)
                {
                    return;
                }

                mState = StateOfNPCContext.Working;
            }

            mBodyResourcesManager.Bootstrap();
            mRightHandResourcesManager.Bootstrap();
            mLeftHandResourcesManager.Bootstrap();
            OnBootsrap();

            if (type == null)
            {
                return;
            }

            var npcProcessInfo = mStorageOfNPCProcesses.StorageOfNPCProcessInfo.GetNPCProcessInfo(type);

#if DEBUG
            //Log($"type = {type?.FullName}");
#endif

            if (npcProcessInfo == null)
            {
                return;
            }

            var command = new NPCCommand();
            command.Name = npcProcessInfo.Name;
            command.Priority = NPCProcessPriorities.Highest;

#if DEBUG
            //Log($"command = {command}");
#endif

            Send(command);
        }

        protected virtual void OnBootsrap()
        {
        }

        public INPCProcess Send(INPCCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return new NotValidAbstractNPCProcess(mEntityLogger, this);
                }

                if (mState != StateOfNPCContext.Working)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, mEntityDictionary);

#if DEBUG
            //Log($"internalCommand = {internalCommand}");
#endif

            var npcProcess = mStorageOfNPCProcesses.GetProcess(internalCommand);

            if (npcProcess == null)
            {
                return new NotValidAbstractNPCProcess(mEntityLogger, this);
            }

            return npcProcess.RunAsync();
        }

        public void RegProcess(INPCProcess process, ulong parentProcessId)
        {
#if DEBUG
            //Log($"process.Id = {process.Id} parentProcessId = {parentProcessId}");
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
            //Log($"process.Id = {process.Id}");
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
#if DEBUG
            //Log($"childProcessId = {childProcessId}");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return null;
                }
            }

            if(childProcessId == 0)
            {
                return null;
            }

#if DEBUG
            //Log($"childProcessId = {childProcessId} NEXT");
#endif

            lock (mProcessesDictLockObj)
            {
#if DEBUG
                //Log($"childProcessId = {childProcessId} NEXT NEXT");
#endif

                if (mChildParentDict.ContainsKey(childProcessId))
                {
#if DEBUG
                    Log("mChildParentDict.ContainsKey(childProcessId)");
#endif

                    var parentId = mChildParentDict[childProcessId];

#if DEBUG
                    //Log($"parentId = {parentId}");
                    //Log($"mProcessesDict.ContainsKey(parentId) = {mProcessesDict.ContainsKey(parentId)}");
#endif
                    if(mProcessesDict.ContainsKey(parentId))
                    {
                        return mProcessesDict[parentId];
                    }            
                }

                return null;
            }
        }

        public NPCResourcesResolutionKind ApproveNPCResourceProcessExecute(BaseNPCResourcesResolution existingsNPCResourcesResulution)
        {
#if DEBUG
            //Log($"existingsNPCResourcesResulution = {existingsNPCResourcesResulution}");
#endif

            var kind = existingsNPCResourcesResulution.Kind;

#if DEBUG
            //Log($"kind = {kind}");
#endif

            switch (kind)
            {
                case NPCResourceKind.Body:
                    return ApproveNPCBodyResourceProcessExecute(existingsNPCResourcesResulution.ToBodyResourcesResulution());

                case NPCResourceKind.Hand:
#if DEBUG
                    //Log("case NPCResourceKind.Hand");
#endif

                    return ApproveNPCHandResourceProcessExecute(existingsNPCResourcesResulution.ToHandResourcesResulution());

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private NPCResourcesResolutionKind ApproveNPCBodyResourceProcessExecute(NPCBodyResourcesResolution existingsNPCResourcesResulution)
        {
            var targetProcessId = existingsNPCResourcesResulution.TargetProcessId;

            var tmpExistingProcessesIdList = new List<ulong>();

            var disagreementByHState = existingsNPCResourcesResulution.DisagreementByHState;

            if (disagreementByHState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHState.CurrentProcessesId);
            }

            var disagreementByTargetPosition = existingsNPCResourcesResulution.DisagreementByTargetPosition;

            if (disagreementByTargetPosition != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByTargetPosition.CurrentProcessesId);
            }

            var disagreementByVState = existingsNPCResourcesResulution.DisagreementByVState;

            if (disagreementByVState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByVState.CurrentProcessesId);
            }

            var disagreementByHandsState = existingsNPCResourcesResulution.DisagreementByHandsState;

            if (disagreementByHandsState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHandsState.CurrentProcessesId);
            }

            var disagreementByHandsActionState = existingsNPCResourcesResulution.DisagreementByHandsActionState;

            if (disagreementByHandsActionState != null)
            {
                tmpExistingProcessesIdList.AddRange(disagreementByHandsActionState.CurrentProcessesId);
            }

            tmpExistingProcessesIdList = tmpExistingProcessesIdList.Distinct().ToList();

            var targetProcessInfo = mProcessesDict[targetProcessId];

            var targetPriority = targetProcessInfo.GlobalPriority;

#if DEBUG
            //Log($"targetPriority = {targetPriority}");
#endif
            foreach (var existingProcessesId in tmpExistingProcessesIdList)
            {
#if DEBUG
                //Log($"existingProcessesId = {existingProcessesId}");
#endif

                var currentProcessInfo = mProcessesDict[existingProcessesId];

#if DEBUG
                //Log($"currentProcessInfo.GlobalPriority = {currentProcessInfo.GlobalPriority}");
#endif

                if (currentProcessInfo.GlobalPriority > targetPriority)
                {
                    return NPCResourcesResolutionKind.Forbiden;
                }
            }

            return NPCResourcesResolutionKind.Allow;
        }

        private NPCResourcesResolutionKind ApproveNPCHandResourceProcessExecute(NPCHandResourcesResolution existingsNPCResourcesResulution)
        {
#if DEBUG
            //Log($"existingsNPCResourcesResulution = {existingsNPCResourcesResulution}");
#endif

            var targetProcessId = existingsNPCResourcesResulution.TargetProcessId;

#if DEBUG
            //Log($"targetProcessId = {targetProcessId}");
            //Log($"mProcessesDict.Count = {mProcessesDict.Count}");
            //Log($"mProcessesDict.ContainsKey(targetProcessId) = {mProcessesDict.ContainsKey(targetProcessId)}");
#endif
 
            var targetProcessInfo = mProcessesDict[targetProcessId];

#if DEBUG
            //Log($"targetProcessInfo == null = {targetProcessInfo == null}");
#endif

            var targetPriority = targetProcessInfo.GlobalPriority;

#if DEBUG
            //Log($"targetPriority = {targetPriority}");
#endif

            foreach (var existingProcessesId in existingsNPCResourcesResulution.CurrentProcessesId)
            {
#if DEBUG
                //Log($"existingProcessesId = {existingProcessesId}");
#endif
                if(mProcessesDict.ContainsKey(existingProcessesId))
                {
                    var currentProcessInfo = mProcessesDict[existingProcessesId];

#if DEBUG
                    //Log($"currentProcessInfo.GlobalPriority = {currentProcessInfo.GlobalPriority}");
#endif

                    if (currentProcessInfo.GlobalPriority > targetPriority)
                    {
#if DEBUG
                        //Log("currentProcessInfo.GlobalPriority > targetPriority");
#endif

                        return NPCResourcesResolutionKind.Forbiden;
                    }
                }
            }

#if DEBUG
            //Log("return NPCResourcesResolutionKind.Allow");
#endif

            return NPCResourcesResolutionKind.Allow;
        }

        public virtual object NoTypedBlackBoard => null;

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

        private Dictionary<int, CancellationToken> mCancelationTokenDict = new Dictionary<int, CancellationToken>();
        private readonly object mCancelationTokenDictLockObj = new object();

        public void RegCancellationToken(int taskId, CancellationToken token)
        {
#if DEBUG
            //Log($"taskId = {taskId}");
#endif
            lock (mCancelationTokenDictLockObj)
            {
                if(mCancelationTokenDict.ContainsKey(taskId))
                {
                    return;
                }

                mCancelationTokenDict[taskId] = token;
            }
        }

        public CancellationToken? GetCancellationToken(int taskId)
        {
#if DEBUG
            //Log($"taskId = {taskId}");
#endif
            lock (mCancelationTokenDictLockObj)
            {
                if(mCancelationTokenDict.ContainsKey(taskId))
                {
                    return mCancelationTokenDict[taskId];
                }

                return null;
            }
        }

        public void UnRegCancellationToken(int taskId)
        {
#if DEBUG
            //Log($"taskId = {taskId}");
#endif
            lock (mCancelationTokenDictLockObj)
            {
                if (!mCancelationTokenDict.ContainsKey(taskId))
                {
                    return;
                }

                mCancelationTokenDict.Remove(taskId);
            }
        }

        public void CallInMainUI(Action function)
        {
            mBodyResourcesManager.CallInMainUI(function);
        }

        public TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            return mBodyResourcesManager.CallInMainUI(function);
        }

        private BaseAbstractLogicalObject mSelfLogicalObject;

        public BaseAbstractLogicalObject SelfLogicalObject
        {
            get
            {
                return mSelfLogicalObject;
            }
        }

        public BaseAbstractLogicalObject GetLogicalObject(string query, params QueryParam[] paramsCollection)
        {
#if DEBUG
            //Log($"GetLogicalObject query = {query}");
#endif
            return new LogicalObject(mEntityLogger, query, paramsCollection, mEntityDictionary, mContextOfCGStorage.MainCGStorage, mSystemPropertiesDictionary, mVisionObjectsStorage);
        }

        public BaseAbstractLogicalObject GetLogicalObject(ICGStorage query)
        {
#if DEBUG
            //Log($"GetLogicalObject query = {query}");
#endif
            return new LogicalObject(mEntityLogger, query, mEntityDictionary, mContextOfCGStorage.MainCGStorage, mSystemPropertiesDictionary, mVisionObjectsStorage);
        }

        public BaseAbstractLogicalObject GetLogicalObject(RuleInstancePackage query)
        {
#if DEBUG
            //Log($"GetLogicalObject query = {query}");
#endif
            return new LogicalObject(mEntityLogger, query, mEntityDictionary, mContextOfCGStorage.MainCGStorage, mSystemPropertiesDictionary, mVisionObjectsStorage);
        }

        public BaseAbstractLogicalObject GetLogicalObject(RuleInstance query)
        {
#if DEBUG
            //Log($"GetLogicalObject query = {query}");
#endif
            return new LogicalObject(mEntityLogger, query, mEntityDictionary, mContextOfCGStorage.MainCGStorage, mSystemPropertiesDictionary, mVisionObjectsStorage);
        }

        public BaseAbstractLogicalObject GetLogicalObject(BaseVariant query)
        {
#if DEBUG
            //Log($"query = {query}");
#endif

            if (!query.IsEntityCondition)
            {
                throw new NotImplementedException();
                //return;
            }

            var entityConditionRuleInstance = query.AsEntityCondition.RuleInstance;
            var ruleInstancesPackage = new RuleInstancePackage(entityConditionRuleInstance);
            var initQueryStorage = new QueryCGStorage(mEntityDictionary, ruleInstancesPackage);

            var queryStorage = ConvertorEntityConditionToQuery.Convert(initQueryStorage);

            return new LogicalObject(mEntityLogger, queryStorage, mEntityDictionary, mContextOfCGStorage.MainCGStorage, mSystemPropertiesDictionary, mVisionObjectsStorage);
        }

        public IList<VisionObject> VisibleObjects
        {
            get
            {
                return mVisionObjectsStorage.VisibleObjects;
            }
        }

        public void Dispose()
        {
#if DEBUG
            //Log("Begin");
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
            mVisionObjectsStorage.Dispose();

            foreach(var processesKVPItem in mProcessesDict)
            {
                processesKVPItem.Value.Dispose();
            }
        }
    }
}
