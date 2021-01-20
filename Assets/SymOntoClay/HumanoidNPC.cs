using SymOntoClay.CoreHelper.DebugHelpers;
using SymOntoClay.Scriptables;
using SymOntoClay.UnityAsset.Core;
using SymOntoClay.UnityAsset.Core.Helpers;
using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace SymOntoClay
{
    [AddComponentMenu("SymOntoClay/HumanoidNPC")]
    public class HumanoidNPC : MonoBehaviour, IPlatformSupport, IUHumanoidNPC
    {
        public NPCFile NPCFile;
        public string Id;

        private string _oldName;
        private string _idForFacts;

        public string IdForFacts => _idForFacts;

        void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = GetIdByName();
            }
            else
            {
                var id = GetIdByName();

                if (Id == GetIdByName(_oldName))
                {
                    Id = id;
                }
            }

            if(Id.StartsWith("#`"))
            {
                _idForFacts = Id;
            }
            else
            {
                _idForFacts = $"{Id.Insert(1, "`")}`";
            }           

            _oldName = name;
        }

        private string GetIdByName()
        {
            return GetIdByName(name);
        }

        private string GetIdByName(string nameStr)
        {
            return $"#{nameStr}";
        }

        void Awake()
        {
#if DEBUG
            Debug.Log("HumanoidNPC Awake");
#endif

            var npcFullFileName = Path.Combine(Application.dataPath, NPCFile.FullName);

#if DEBUG
            //_navMeshAgent.updateRotation = false;
            //Debug.Log($"HumanoidNPC Awake _navMeshAgent.updateRotation = {_navMeshAgent.updateRotation}");
            Debug.Log($"HumanoidNPC Awake npcFullFileName = {npcFullFileName}");
#endif

            var npcSettings = new HumanoidNPCSettings();
            npcSettings.Id = Id;
            npcSettings.InstanceId = GetInstanceID();

            npcSettings.LogicFile = npcFullFileName;

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
            var hostListener = GetComponent<IUHostListener>();

            if (hostListener == null)
            {
                return this;
            }

            return hostListener;
        }

        void Stop()
        {
            _npc.Dispose();
        }

        System.Numerics.Vector3 IPlatformSupport.ConvertFromRelativeToAbsolute(SymOntoClay.Core.RelativeCoordinate relativeCoordinate)
        {
            var distance = relativeCoordinate.Distance;
            var angle = relativeCoordinate.HorizontalAngle;

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

        public IHumanoidNPC NPC => _npc;
    }
}
