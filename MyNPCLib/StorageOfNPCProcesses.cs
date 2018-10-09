using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcesses : IDisposable
    {
        public StorageOfNPCProcesses(IEntityLogger entityLogger, IIdFactory idFactory, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCContext context)
        {
            mEntityLogger = entityLogger;
            mContext = context;
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mStorageOfNPCProcessInfo = new StorageOfNPCProcessInfo(entityLogger,entityDictionary, npcProcessInfoCache);
            mActivatorOfNPCProcessEntryPointInfo = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);
        }

        #region private members
        private IEntityLogger mEntityLogger;
        private INPCContext mContext;
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private StorageOfNPCProcessInfo mStorageOfNPCProcessInfo;
        private ActivatorOfNPCProcessEntryPointInfo mActivatorOfNPCProcessEntryPointInfo;
        private Dictionary<ulong, BaseNPCProcess> mSingletonsDict = new Dictionary<ulong, BaseNPCProcess>();
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;
        #endregion

        public StorageOfNPCProcessInfo StorageOfNPCProcessInfo => mStorageOfNPCProcessInfo;
        public ActivatorOfNPCProcessEntryPointInfo ActivatorOfNPCProcessEntryPointInfo => mActivatorOfNPCProcessEntryPointInfo;

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

        public bool AddTypeOfProcess(Type type)
        {
#if DEBUG
            //Log($"type = {type?.FullName}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            return mStorageOfNPCProcessInfo.AddTypeOfProcess(type);
        }

        public BaseNPCProcessInvocablePackage GetProcess(NPCInternalCommand command)
        {
#if DEBUG
            //Log($"command = {command}");
#endif

            lock (mDisposeLockObj)
            {
                if (mIsDisposed)
                {
                    throw new ElementIsNotActiveException();
                }
            }

            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var processInfo = mStorageOfNPCProcessInfo.GetNPCProcessInfo(command.Key);

#if DEBUG
            //Log($"processInfo = {processInfo}");
#endif

            if(processInfo == null)
            {
                return null;
            }

            var targetEntryPoint = mActivatorOfNPCProcessEntryPointInfo.GetTopEntryPoint(processInfo, command.Params);

            if (targetEntryPoint == null)
            {
                return null;
            }

            var key = processInfo.Key;
            var startupMode = processInfo.StartupMode;

            BaseNPCProcess instance = null;

            switch (processInfo.StartupMode)
            {
                case NPCProcessStartupMode.Singleton:
                    {
                        if(mSingletonsDict.ContainsKey(key))
                        {
                            instance = mSingletonsDict[key];
                        }
                        else
                        {
                            instance = CreateInstanceByProcessInfo(processInfo, command.Priority, startupMode);
                            mSingletonsDict[key] = instance;
                        }
                    }
                    break;

                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:
                    {
                        instance = CreateInstanceByProcessInfo(processInfo, command.Priority, startupMode);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(startupMode), startupMode, null);
            }

            var result = new BaseNPCProcessInvocablePackage() {
                Process = instance,
                Command = command,
                EntryPoint = targetEntryPoint.EntryPoint,
                RankOfEntryPoint = targetEntryPoint.Rank
            };

            return result;
        }

        private BaseNPCProcess CreateInstanceByProcessInfo(NPCProcessInfo npcProcessInfo, float priority, NPCProcessStartupMode startupMode)
        {
            var instance = (BaseNPCProcess)Activator.CreateInstance(npcProcessInfo.Type);

            instance.Id = mIdFactory.GetNewId();
            instance.Context = mContext;
            instance.Info = npcProcessInfo;
            instance.LocalPriority = priority;
            instance.StartupMode = startupMode;
            instance.EntityLogger = mEntityLogger;

            if(startupMode == NPCProcessStartupMode.Singleton)
            {
                NPCProcessHelpers.RegProcess(mContext, instance, startupMode, KindOfLinkingToInitiator.Standalone, 0ul, false);
            }

            return instance;
        }

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

            mStorageOfNPCProcessInfo.Dispose();

            mSingletonsDict.Clear();
        }
    }
}
