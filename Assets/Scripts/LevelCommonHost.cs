using MyNPCLib;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSoundModeling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelCommonHost : MonoBehaviour, ILevelCommonHost
    {
        public void Awake()
        {
#if UNITY_EDITOR
            var logInstance = new LogProxyForDebug();
            LogInstance.SetLogProxy(logInstance);

            LogInstance.Log("Begin");
#endif

            mEntityDictionary = new EntityDictionary();
            mNPCProcessInfoCache = new NPCProcessInfoCache();
            mOldLogicalObjectsBus = new OldLogicalObjectsBus();
            mBusOfCGStorages = new BusOfCGStorages(mEntityDictionary);
            mHandThingsBus = new HandThingsBus();
            mLogicalSoundBus = new LogicalSoundBus();
        }

        private IEntityDictionary mEntityDictionary;

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        private NPCProcessInfoCache mNPCProcessInfoCache;

        public NPCProcessInfoCache NPCProcessInfoCache => mNPCProcessInfoCache;

        private OldLogicalObjectsBus mOldLogicalObjectsBus;
        public OldLogicalObjectsBus OldLogicalObjectsBus => mOldLogicalObjectsBus;

        private BusOfCGStorages mBusOfCGStorages;
        public BusOfCGStorages BusOfCGStorages => mBusOfCGStorages;

        private HandThingsBus mHandThingsBus;

        public HandThingsBus HandThingsBus => mHandThingsBus;

        private LogicalSoundBus mLogicalSoundBus;

        public LogicalSoundBus LogicalSoundBus => mLogicalSoundBus;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
