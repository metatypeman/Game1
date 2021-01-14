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
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            var npcFullFileName = Path.Combine(Application.dataPath, NPCFile.FullName);

#if DEBUG
            //_navMeshAgent.updateRotation = false;
            //Debug.Log($"HumanoidNPC Awake _navMeshAgent.updateRotation = {_navMeshAgent.updateRotation}");
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
            if(_isDead)
            {
                return;
            }

            if(_isWalking)
            {
                var nextPosition = _navMeshAgent.nextPosition;
                if (_targetPosition.x == nextPosition.x && _targetPosition.z == nextPosition.z)
                {
                    PerformStop();

                    lock (_lockObj)
                    {
                        lock(_walkingRepresentative)
                        {
                            _walkingRepresentative.IsFinished = true;
                        }
                        
                        _walkingRepresentative = null;
                    }

#if DEBUG
                    Debug.Log("HumanoidNPC Update Walking has been stoped.");
#endif
                }
            }
        }

        void Stop()
        {
            _npc.Dispose();
        }

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Rigidbody _rigidbody;

        private object _lockObj = new object();

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

        private bool _hasRifle;
        private bool _isWalking;
        private bool _isAim;
        private bool _isDead;

        private ActionRepresentative _walkingRepresentative;

        private Vector3 _targetPosition;

        private void UpdateAnimator()
        {
            _animator.SetBool("hasRifle", _hasRifle);
            _animator.SetBool("walk", _isWalking);
            _animator.SetBool("isAim", _isAim);
            _animator.SetBool("isDead", _isDead);
        }

        private void PerformStop()
        {
            _navMeshAgent.ResetPath();
            _isWalking = false;
            UpdateAnimator();
        }

        [BipedEndpoint("Go", DeviceOfBiped.RightLeg, DeviceOfBiped.LeftLeg)]
        public void GoToImpl(CancellationToken cancellationToken,
            [EndpointParam("To", KindOfEndpointParam.Position)] Vector3 point,
            float speed = 12)
        {
#if DEBUG
            Debug.Log($"HumanoidNPC GoToImpl point = {point}");
#endif

            var representative = new ActionRepresentative();

            _npc.RunInMainThread(() => {
                _targetPosition = point;
                _navMeshAgent.SetDestination(point);
                _isWalking = true;
                UpdateAnimator();

                lock(_lockObj)
                {
                    _walkingRepresentative = representative;
                }
            });

#if DEBUG
            Debug.Log($"HumanoidNPC GoToImpl Walking has been started.");
#endif

            while(true)
            {
                lock(representative)
                {
                    if(representative.IsFinished)
                    {
                        break;
                    }

                    if(cancellationToken.IsCancellationRequested)
                    {
                        _npc.RunInMainThread(() => {
                            lock (_lockObj)
                            {
                                if (_walkingRepresentative == representative)
                                {
                                    _walkingRepresentative = null;
                                    PerformStop();
                                }
                            }
                        });

                        break;
                    }
                }

                Thread.Sleep(1000);
            }

#if DEBUG
            Debug.Log($"HumanoidNPC GoToImpl Walking has been stoped.");
#endif
        }
    }
}
