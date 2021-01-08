using Assets.SymOntoClay;
using SymOntoClay.Helpers;
using SymOntoClay.Scriptables;
using SymOntoClay.UnityAsset.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SymOntoClay
{
    [AddComponentMenu("SymOntoClay/HumanoidNPC")]
    public class HumanoidNPC : MonoBehaviour
    {
        public NPCFile NPCFile;
        public HostFile HostFile;
        public string Id;

        void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = $"#{Guid.NewGuid().ToString("D").Replace("-", string.Empty)}";
            }
        }

        void Awake()
        {
#if DEBUG
            Debug.Log("HumanoidNPC Awake"); 
#endif

            var npcFullFileName = Path.Combine(Application.dataPath, NPCFile.FullName);

#if DEBUG
            Debug.Log($"HumanoidNPC Awake npcFullFileName = {npcFullFileName}");
#endif

            var npcSettings = new HumanoidNPCSettings();
            npcSettings.Id = Id;

            npcSettings.LogicFile = npcFullFileName;

            if (HostFile != null)
            {
                var hostFullFileName = Path.Combine(Application.dataPath, HostFile.FullName);

#if DEBUG
                Debug.Log($"HumanoidNPC Awake hostFullFileName = {hostFullFileName}");
#endif

                npcSettings.HostFile = hostFullFileName;
            }

            npcSettings.HostListener = GetHostListener();
            //npcSettings.PlatformSupport = new TstPlatformSupport();

#if DEBUG
            Debug.Log($"HumanoidNPC Awake npcSettings = {npcSettings}");
#endif

            QuickLogger.Log($"HumanoidNPC Awake npcSettings = {npcSettings}");

            _npc = WorldFactory.WorldInstance.GetHumanoidNPC(npcSettings);
        }

        private object GetHostListener()
        {
            var hostListener = GetComponent<IHostListener>();

            if (hostListener == null)
            {
                return this;
            }

            return hostListener;
        }

        void Start()
        {
#if DEBUG
            Debug.Log("HumanoidNPC Start");
#endif
        }

        void Update()
        {

        }

        void Stop()
        {
            _npc.Dispose();
        }

        private IHumanoidNPC _npc;
    }
}
