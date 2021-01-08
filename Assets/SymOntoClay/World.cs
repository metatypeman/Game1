using SymOntoClay.Helpers;
using SymOntoClay.Scriptables;
using SymOntoClay.UnityAsset.Core;
using SymOntoClay.UnityAsset.Core.Internal.EndPoints.MainThread;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymOntoClay
{
    [AddComponentMenu("SymOntoClay/World")]
    public class World : MonoBehaviour
    {
        public WorldFile WorldFile;

        void Awake()
        {
            _invokerInMainThread = new InvokerInMainThread();

            var supportBasePath = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), Application.productName);

            var logDir = Path.Combine(supportBasePath, "NpcLogs");

            _world = WorldFactory.WorldInstance;

            var worldFullFileName = Path.Combine(Application.dataPath, WorldFile.FullName);

#if DEBUG
            Debug.Log($"World Awake worldFullFileName = {worldFullFileName}");
#endif

            var settings = new WorldSettings();

            QuickLogger.Log($"Application.supportBasePath = {supportBasePath}");

#if DEBUG            
            Debug.Log($"World Awake settings = {settings}");
#endif
        }

        void Start()
        {
#if DEBUG
            Debug.Log("World Start");
#endif
        }

        void Update()
        {
            _invokerInMainThread.Update();
        }

        void Stop()
        {

        }

        private IWorld _world;
        private InvokerInMainThread _invokerInMainThread;
    }
}

