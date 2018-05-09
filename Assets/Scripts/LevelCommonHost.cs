using MyNPCLib;
using MyNPCLib.Logical;
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
            Debug.Log("LevelCommonHost Awake");

            var logInstance = new LogProxyForDebug();
            LogInstance.SetLogProxy(logInstance);
#endif

            mEntityDictionary = new EntityDictionary();
            mNPCProcessInfoCache = new NPCProcessInfoCache();
            mLogicalObjectsBus = new LogicalObjectsBus();
            mQueriesCache = new QueriesCache(mEntityDictionary);
            mHandThingsBus = new HandThingsBus();
        }

        private IEntityDictionary mEntityDictionary;

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        private NPCProcessInfoCache mNPCProcessInfoCache;

        public NPCProcessInfoCache NPCProcessInfoCache => mNPCProcessInfoCache;

        private LogicalObjectsBus mLogicalObjectsBus;
        public LogicalObjectsBus LogicalObjectsBus => mLogicalObjectsBus;

        private QueriesCache mQueriesCache;

        public QueriesCache QueriesCache => mQueriesCache;

        private HandThingsBus mHandThingsBus;

        public HandThingsBus HandThingsBus => mHandThingsBus;

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
