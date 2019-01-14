using MyNPCLib;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelCommonHost : MonoBehaviour, ILevelCommonHost, IInvokingInMainThread
    {
        public LevelCommonHost()
        {
#if UNITY_EDITOR
            var logInstance = new LogProxyForDebug();
            LogInstance.SetLogProxy(logInstance);

            //LogInstance.Log("Begin");
#endif

            mEntityDictionary = new EntityDictionary();
            mNPCProcessInfoCache = new NPCProcessInfoCache();
            mOldLogicalObjectsBus = new OldLogicalObjectsBus();
            mBusOfCGStorages = new BusOfCGStorages(mEntityDictionary);
            mHandThingsBus = new HandThingsBus();
            mLogicalSoundBus = new LogicalSoundBus();

            //var terrainObj = GameObject.Find("Terrain");

            //var terrain = terrainObj.GetComponent<Terrain>();
            //var terrainData = terrain.terrainData;

            //LogInstance.Log($"terrainData.size = {terrainData.size}");

            //mRTreeNode = new RTreeNode(new Vector3(0, 0, 0), new Vector3(terrainData.size.x, 0, terrainData.size.z));

            //mHostNavigationRegistry = new NavigationRegistry(mRTreeNode);
            mHostNavigationRegistry = new NavigationRegistry(this);
        }

        public void Awake()
        {
            Application.logMessageReceived += (string condition, string stackTrace, LogType type) => {
                File.AppendAllText("c:/Users/Sergey/Unity.log", $"------------- {condition} {stackTrace} {stackTrace}{Environment.NewLine}");
            };

            var terrainObj = GameObject.Find("Terrain");

            var terrain = terrainObj.GetComponent<Terrain>();
            var terrainData = terrain.terrainData;

            //LogInstance.Log($"terrainData.size = {terrainData.size}");

            mRTreeNode = new RTreeNode(new Vector3(0, 0, 0), new Vector3(terrainData.size.x, 0, terrainData.size.z));

            mHostNavigationRegistry.RTreeNode = mRTreeNode;
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

        private NavigationRegistry mHostNavigationRegistry;

        public IHostNavigationRegistry HostNavigationRegistry => mHostNavigationRegistry;

        // Use this for initialization
        void Start()
        {
            mHostNavigationRegistry.PrepareAllInfo();
        }

        private RTreeNode mRTreeNode;

        private object mTmpQueueLockObj = new object();
        private Queue<IInvocableInMainThreadObj> mTmpQueue = new Queue<IInvocableInMainThreadObj>();

        private void ProcessInvocable()
        {
            List<IInvocableInMainThreadObj> invocableList = null;

            lock (mTmpQueueLockObj)
            {
                if (mTmpQueue.Count > 0)
                {
                    invocableList = mTmpQueue.ToList();
                    mTmpQueue.Clear();
                }
            }

            if (invocableList == null)
            {
                return;
            }

            foreach (var invocable in invocableList)
            {
                invocable.Invoke();
            }
        }

        public void SetInvocableObj(IInvocableInMainThreadObj invokableObj)
        {
            lock (mTmpQueueLockObj)
            {
                mTmpQueue.Enqueue(invokableObj);
            }
        }

        // Update is called once per frame
        void Update()
        {
            ProcessInvocable();
            mRTreeNode.Draw();
        }
    }
}
