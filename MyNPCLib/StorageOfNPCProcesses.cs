using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcesses : IDisposable
    {
        public StorageOfNPCProcesses(IIdFactory idFactory, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache, INPCContext context)
        {
            mContext = context;
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mStorageOfNPCProcessInfo = new StorageOfNPCProcessInfo(entityDictionary, npcProcessInfoCache);
            mActivatorOfNPCProcessEntryPointInfo = new ActivatorOfNPCProcessEntryPointInfo();
        }

        #region private members
        private INPCContext mContext;
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private StorageOfNPCProcessInfo mStorageOfNPCProcessInfo;
        private ActivatorOfNPCProcessEntryPointInfo mActivatorOfNPCProcessEntryPointInfo;
        private Dictionary<ulong, BaseNPCProcess> mSingletonsDict = new Dictionary<ulong, BaseNPCProcess>();
        private object mDisposeLockObj = new object();
        private bool mIsDisposed;
        #endregion

        public bool AddTypeOfProcess(Type type)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses AddTypeOfProcess type = {type?.FullName}");
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

        public BaseNPCProcess GetProcess(NPCInternalCommand command)
        {
#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses GetProcess command = {command}");
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
            LogInstance.Log($"StorageOfNPCProcesses GetProcess processInfo = {processInfo}");
#endif

            if(processInfo == null)
            {
                return null;
            }

            var rankedEntryPointsList = mActivatorOfNPCProcessEntryPointInfo.GetRankedEntryPoints(processInfo, command.Params);

#if DEBUG
            LogInstance.Log($"StorageOfNPCProcesses GetProcess rankedEntryPointsList.Count = {rankedEntryPointsList.Count}");
#endif

            if(rankedEntryPointsList.Count == 0)
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
                            instance = CreateInstanceByProcessInfo(processInfo);
                            mSingletonsDict[key] = instance;
                        }
                    }
                    break;

                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:
                    {
                        instance = CreateInstanceByProcessInfo(processInfo);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(startupMode), startupMode, null);
            }

            return instance;
        }

        private BaseNPCProcess CreateInstanceByProcessInfo(NPCProcessInfo npcProcessInfo)
        {
            var instance = (BaseNPCProcess)Activator.CreateInstance(npcProcessInfo.Type);
            instance.Id = mIdFactory.GetNewId();
            instance.Context = mContext;
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
        }
    }
}
