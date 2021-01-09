using Assets.SymOntoClay;
using SymOntoClay.CoreHelper.DebugHelpers;
using SymOntoClay.Scriptables;
using SymOntoClay.UnityAsset.Core;
using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace SymOntoClay
{
    [AddComponentMenu("SymOntoClay/HumanoidNPC")]
    public class HumanoidNPC : MonoBehaviour, IPlatformSupport
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

            _navMeshAgent = GetComponent<NavMeshAgent>();

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
            npcSettings.PlatformSupport = this;

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

        private NavMeshAgent _navMeshAgent;

        System.Numerics.Vector3 IPlatformSupport.ConvertFromRelativeToAbsolute(System.Numerics.Vector2 relativeCoordinates)
        {
            var angle = relativeCoordinates.X;
            var distance = relativeCoordinates.Y;

#if DEBUG
            //Debug.Log($"HumanoidNPC ConvertFromRelativeToAbsolute angle = {angle}");
            //Debug.Log($"HumanoidNPC ConvertFromRelativeToAbsolute distance = {distance}");
#endif

            var radAngle = angle * Mathf.Deg2Rad;
            var x = Mathf.Sin(radAngle);
            var y = Mathf.Cos(radAngle);
            var localDirection = new Vector3(x * distance, 0f, y * distance);

#if DEBUG
            //Debug.Log($"HumanoidNPC ConvertFromRelativeToAbsolute localDirection = {localDirection}");
#endif

            var globalDirection = transform.TransformDirection(localDirection);

            var newPosition = globalDirection + transform.position;

            return new System.Numerics.Vector3(newPosition.x, newPosition.y, newPosition.z);
        }

        private IHumanoidNPC _npc;

        [BipedEndpoint("Go", true, DeviceOfBiped.RightLeg, DeviceOfBiped.LeftLeg)]
        public void GoToImpl(CancellationToken cancellationToken,
            [EndpointParam("To", KindOfEndpointParam.Position)] Vector3 point,
            float speed = 12)
        {
#if DEBUG
            Debug.Log($"HumanoidNPC GoToImpl point = {point}");
#endif

            _navMeshAgent.SetDestination(point);
        }
    }
}
