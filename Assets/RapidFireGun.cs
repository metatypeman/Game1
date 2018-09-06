using Assets.Scripts;
using MyNPCLib;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RapidFireGun : MonoBehaviour, IRapidFireGun, IReadOnlyLogicalObject
{
    #region Public Fields
    public bool EnableLogging = false;
    public string Marker = $"#{Guid.NewGuid().ToString("D")}";
    [SerializeField]
    public GameObject Body;
    [SerializeField]
    public GameObject GunEnd;
    [SerializeField]
    public Light FaceLight;
    public float EffectsDisplayTime = 0.1f;
    public int DamagePerShot = 20;
    public float TimeBetweenBullets = 0.1f;
    public float Range = 100f;
    #endregion

    #region Private Fields
    private EntityLogger mEntityLogger = new EntityLogger();
    private Transform mGunEndTransform;
    private ParticleSystem mGunParticles;
    private LineRenderer mGunLine;
    private AudioSource mGunAudio;
    private Light mGunLight;
    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;
    private bool mUseDebugLine;
    private Collider mBodyCollider;
    private Rigidbody mBodyRigidbody;

    private HostLogicalObjectStorage mHostLogicalObjectStorage;
    private PassiveLogicalObject mPassiveLogicalObject;
    #endregion

    [MethodForLoggingSupport]
    protected void Log(string message)
    {
        mEntityLogger?.Log(message);
    }

    [MethodForLoggingSupport]
    protected void Error(string message)
    {
        mEntityLogger?.Error(message);
    }

    [MethodForLoggingSupport]
    protected void Warning(string message)
    {
        mEntityLogger?.Warning(message);
    }

    public ulong EntityId => mPassiveLogicalObject.EntityId;
    public object this[ulong propertyKey]
    {
        get
        {
            return mHostLogicalObjectStorage[propertyKey];
            //return mPassiveLogicalObject[propertyKey];
        }

        protected set
        {
            mHostLogicalObjectStorage[propertyKey] = value;
            //mPassiveLogicalObject[propertyKey] = value;
        }
    }

    public AccessPolicyToFact GetAccessPolicyToFact(ulong propertyKey)
    {
        return mPassiveLogicalObject.GetAccessPolicyToFact(propertyKey);
    }

    public bool IsReady => true;

    public bool UseDebugLine
    {
        get
        {
            return mUseDebugLine;
        }

        set
        {
            mUseDebugLine = value;

            if (mUseDebugLine)
            {
                if (mGunLine != null)
                {
                    mGunLine.enabled = true;
                }
            }
            else
            {
                mGunLine.enabled = false;
            }
        }
    }

    // Use this for initialization
    void Start() {
        mEntityLogger.Enabled = EnableLogging;
        mEntityLogger.Marker = Marker;

        var commonLevelHost = LevelCommonHostFactory.Get();

        mHostLogicalObjectStorage = new HostLogicalObjectStorage(commonLevelHost.EntityDictionary);
        commonLevelHost.BusOfCGStorages.AddStorage(mHostLogicalObjectStorage);

        mPassiveLogicalObject = new PassiveLogicalObject(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.OldLogicalObjectsBus, mHostLogicalObjectStorage.EntityId);

        var entityId = mPassiveLogicalObject.EntityId;

        var tmpGameObject = gameObject;
        var instanceId = tmpGameObject.GetInstanceID();

        mHostLogicalObjectStorage["name"] = tmpGameObject.name;
        mPassiveLogicalObject["name"] = tmpGameObject.name;

        commonLevelHost.OldLogicalObjectsBus.RegisterObject(instanceId, this);
        commonLevelHost.HandThingsBus.RegisterThing(entityId, this);
        
        mGunParticles = GetComponent<ParticleSystem>();

        if (mGunParticles == null)
        {
            mGunParticles = GetComponentInChildren<ParticleSystem>();
        }

        mGunLine = GetComponent<LineRenderer>();

        if (mGunLine == null)
        {
            mGunLine = GetComponentInChildren<LineRenderer>();
        }

        mGunAudio = GetComponent<AudioSource>();

        if (mGunAudio == null)
        {
            mGunAudio = GetComponentInChildren<AudioSource>();
        }

        mGunLight = GetComponent<Light>();

        if (mGunLight == null)
        {
            mGunLight = GetComponentInChildren<Light>();
        }

        mGunEndTransform = GunEnd.transform;

        mBodyCollider = Body.GetComponent<Collider>();
        mBodyRigidbody = Body.GetComponent<Rigidbody>();
    }

    private object mFireModeLockObj = new object();
    private FireMode mFireMode = FireMode.Multiple;

    public FireMode FireMode
    {
        get
        {
            lock (mFireModeLockObj)
            {
                return mFireMode;
            }
        }

        set
        {
            lock (mFireModeLockObj)
            {
                if (mFireMode == value)
                {
                    return;
                }

                mFireMode = value;
            }
        }
    }

    private object mTurnStateLockObj = new object();
    private TurnState mTurnState = TurnState.Off;

    public TurnState TurnState
    {
        get
        {
            lock (mTurnStateLockObj)
            {
                return mTurnState;
            }
        }

        set
        {
            lock (mTurnStateLockObj)
            {
                if (mTurnState == value)
                {
                    return;
                }

                mTurnState = value;
            }
        }
    }

    public event Action OnFire;

    private InternalStateOfRapidFireGun mInternalState = InternalStateOfRapidFireGun.TurnedOf;

    // Update is called once per frame
    void Update() {
        mEntityLogger.Enabled = EnableLogging;
        mEntityLogger.Marker = Marker;

        if (mUseDebugLine)
        {
            mGunLine.enabled = true;
            mGunLine.SetPosition(0, mGunEndTransform.position);
            shootRay.origin = mGunEndTransform.position;
            shootRay.direction = mGunEndTransform.forward;
            mGunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }

        var fireMode = FireMode;
        var turnState = TurnState;

        switch (turnState)
        {
            case TurnState.On:
                switch (mInternalState)
                {
                    case InternalStateOfRapidFireGun.TurnedOf:
                        ProcessShoot();
                        break;

                    case InternalStateOfRapidFireGun.TurnedOnShot:
                        timer += Time.deltaTime;
                        if (timer >= EffectsDisplayTime)
                        {
                            ProcessEndShoot();
                        }
                        break;

                    case InternalStateOfRapidFireGun.TurnedOnWasShot:
                        switch (fireMode)
                        {
                            case FireMode.Multiple:
                                timer += Time.deltaTime;
                                if (timer >= TimeBetweenBullets)
                                {
                                    ProcessShoot();
                                }
                                break;

                            case FireMode.Single:
                                mInternalState = InternalStateOfRapidFireGun.BeforeOffIfSingle;
                                DisableEffects();
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(turnState), turnState, null);
                        }
                        break;
                }
                break;

            case TurnState.Off:
                switch (mInternalState)
                {
                    case InternalStateOfRapidFireGun.BeforeOffIfSingle:
                        mInternalState = InternalStateOfRapidFireGun.TurnedOf;
                        break;

                    case InternalStateOfRapidFireGun.TurnedOnShot:
                    case InternalStateOfRapidFireGun.TurnedOnWasShot:
                        mInternalState = InternalStateOfRapidFireGun.TurnedOf;
                        DisableEffects();
                        break;
                }
                break;

            default: throw new ArgumentOutOfRangeException(nameof(turnState), turnState, null);
        }
    }

    private void NotifyAboutFire()
    {
        Task.Run(() => {
            try
            {
                OnFire?.Invoke();
            }
            catch(Exception e)
            {
                Error($"e = {e}");
            }          
        });
    }

    private void ProcessShoot()
    {
        timer = 0f;
        mInternalState = InternalStateOfRapidFireGun.TurnedOnShot;
        Shoot();
        NotifyAboutFire();
    }

    private void ProcessEndShoot()
    {
        timer = 0f;
        mInternalState = InternalStateOfRapidFireGun.TurnedOnWasShot;
        DisableEffects();
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        if (!mUseDebugLine)
        {
            mGunLine.enabled = false;
        }

        FaceLight.enabled = false;
        mGunLight.enabled = false;
    }

    public void Shoot()
    {
        mGunAudio.Play();

        // Включаем всвет
        mGunLight.enabled = true;
        FaceLight.enabled = true;

        mGunParticles.Stop();
        mGunParticles.Play();

        // Линия выстрела
        mGunLine.enabled = true;
        mGunLine.SetPosition(0, mGunEndTransform.position);
        // Настройка луча
        shootRay.origin = mGunEndTransform.position;
        shootRay.direction = mGunEndTransform.forward;

        if (Physics.Raycast(shootRay, out shootHit, Range))
        {
            var targetOfShoot = shootHit.collider.GetComponentInParent<ITargetOfShoot>();

            if (targetOfShoot != null)
            {
                targetOfShoot.SetHit(shootHit, DamagePerShot);
            }

            mGunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            mGunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }
    }

    public float GetCorrectingAngle(Vector3 targetPos)
    {
        var targetDir = targetPos - mGunEndTransform.position;
        var forward = mGunEndTransform.forward;
        var angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        return angle;
    }

    public bool SetToHandsOfHumanoid(IInternalHumanoid humanoid, IInternalHumanoidHostContext internalContext)
    {
#if UNITY_EDITOR
        //Log("Begin");
#endif

        var targetParent = humanoid.RightHandWP.transform;

        if(Body.transform.parent == targetParent)
        {
#if UNITY_EDITOR
            //Log("Body.transform.parent == targetParent");
#endif

            return true;
        }

        internalContext.RightHandThing = this;

        if (mBodyCollider != null)
        {
            mBodyCollider.enabled = false;
        }

        if (mBodyRigidbody != null)
        {
            mBodyRigidbody.isKinematic = true;
        }

        Body.transform.SetParent(targetParent, false);
        //Body.transform.rotation = Quaternion.Euler(0, -180.234f, 0);
        Body.transform.localRotation = Quaternion.Euler(0, -180.234f, 0);
        //Body.transform.rotation = new Quaternion(0, -180.234f, 0, 1);
        Body.transform.localPosition = new Vector3(0, 0, 0.2f);
        humanoid.SetAimCorrector(this);
        Body.gameObject.SetActive(true);

#if UNITY_EDITOR
        //Log("End");
#endif

        return true;
    }

    public bool SetAsAloneAndHide()
    {
#if UNITY_EDITOR
        //Log("Begin");
#endif

        if(Body.transform.parent == null && !Body.gameObject.activeSelf)
        {
#if UNITY_EDITOR
            //Log("RBody.transform.parent == null && !Body.gameObject.activeSelf");
#endif

            return true;
        }

        Body.transform.SetParent(null);
        Body.gameObject.SetActive(false);

#if UNITY_EDITOR
        //Log("End");
#endif

        return true;
    }

    public bool ThrowOutToSurface()
    {
#if UNITY_EDITOR
        //Log("Begin");
#endif

        if (Body.transform.parent == null && Body.gameObject.activeSelf)
        {
#if UNITY_EDITOR
            //Log("Body.transform.parent == null && Body.gameObject.activeSelf");
#endif

            return true;
        }

        Body.transform.SetParent(null);
        Body.gameObject.SetActive(true);

        if(mBodyCollider != null)
        {
            mBodyCollider.enabled = true;
        }
        
        if(mBodyRigidbody != null)
        {
            mBodyRigidbody.isKinematic = false;
            mBodyRigidbody.AddForce(UnityEngine.Random.insideUnitSphere * 200);
        }

#if UNITY_EDITOR
        //Log("End");
#endif

        return true;
    }

    public INPCProcess Send(INPCCommand command)
    {
#if UNITY_EDITOR
        Log($"command = {command}");
#endif

        var process = new NPCThingProcess(mEntityLogger);

        var commandName = command.Name;

        if (commandName == "shoot on")
        {
            TurnState = TurnState.On;

            process.State = StateOfNPCProcess.RanToCompletion;
            return process;
        }

        if(commandName == "shoot off")
        {
            TurnState = TurnState.Off;

            process.State = StateOfNPCProcess.RanToCompletion;
            return process;
        }

        if(commandName == "set single")
        {
            FireMode = FireMode.Single;

            process.State = StateOfNPCProcess.RanToCompletion;
            return process;
        }

        if(commandName == "set multiple")
        {
            FireMode = FireMode.Multiple;

            process.State = StateOfNPCProcess.RanToCompletion;
            return process;
        }

        process.State = StateOfNPCProcess.Faulted;
        return process;
    }

    public object Get(string propertyName)
    {
#if UNITY_EDITOR
        Log($"propertyName = {propertyName}");
#endif

        if(propertyName == "FireMode")
        {
            return FireMode;
        }

        if(propertyName == "TurnState")
        {
            return TurnState;
        }

        return null;
    }
}
