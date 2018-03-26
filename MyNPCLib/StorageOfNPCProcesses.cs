using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class StorageOfNPCProcesses : IDisposable
    {
        public StorageOfNPCProcesses(IIdFactory idFactory, IEntityDictionary entityDictionary, NPCProcessInfoCache npcProcessInfoCache)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mStorageOfNPCProcessInfo = new StorageOfNPCProcessInfo(entityDictionary, npcProcessInfoCache);
        }

        #region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private StorageOfNPCProcessInfo mStorageOfNPCProcessInfo;
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

            var startupMode = processInfo.StartupMode;

            switch (processInfo.StartupMode)
            {
                case NPCProcessStartupMode.Singleton:
                    throw new NotImplementedException();

                case NPCProcessStartupMode.NewInstance:
                case NPCProcessStartupMode.NewStandaloneInstance:
                    throw new NotImplementedException();

                default: throw new ArgumentOutOfRangeException(nameof(startupMode), startupMode, null);
            }

            throw new NotImplementedException();
        }

        private BaseNPCProcess CreateInstanceByProcessInfo(NPCProcessInfo npcProcessInfo)
        {

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
