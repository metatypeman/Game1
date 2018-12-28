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
            var terrainObj = GameObject.Find("Terrain");

            var scaleX = terrainObj.transform.lossyScale.x;

            Debug.Log($"scaleX = {scaleX}");

            var terrain = terrainObj.GetComponent<Terrain>();
            var terrainData = terrain.terrainData;

            Debug.Log($"terrainData.size = {terrainData.size}");

            mRTreeNode = new RTreeNode(new Vector3(0, 0, 0), new Vector3(terrainData.size.x, 0, terrainData.size.z));

            //mPlainRect = new PlainRect(new Vector3(0, 0, 0), new Vector3(terrainData.size.x, 0, terrainData.size.z));            
        }

        private RTreeNode mRTreeNode;
        //private PlainRect mPlainRect;

        // Update is called once per frame
        void Update()
        {
            //mPlainRect.DrawDebug();
            mRTreeNode.Draw();
        }

        void OnDrawGizmos()
        {
            var terrain = GameObject.Find("Terrain");

        }
    }
}
