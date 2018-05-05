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
            mLogicalObjectsBus = new LogicalIndexStorage();
            mGameObjectsBus = new GameObjectsBus();
        }

        private IEntityDictionary mEntityDictionary;

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        private NPCProcessInfoCache mNPCProcessInfoCache;

        public NPCProcessInfoCache NPCProcessInfoCache => mNPCProcessInfoCache;

        private LogicalIndexStorage mLogicalObjectsBus;
        public LogicalIndexStorage LogicalObjectsBus => mLogicalObjectsBus;

        private GameObjectsBus mGameObjectsBus;
        public GameObjectsBus GameObjectsBus => mGameObjectsBus;
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
