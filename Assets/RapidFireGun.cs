using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RapidFireGun : MonoBehaviour, IRapidFireGun
{
    #region Public Fields
    public GameObject Body;
    public GameObject GunEnd;
    public Light FaceLight;
    public float EffectsDisplayTime = 0.1f;
    public int DamagePerShot = 20;
    public float TimeBetweenBullets = 0.1f;
    public float Range = 100f;
    #endregion

    #region Private Fields
    private Transform mGunEndTransform;
    private ParticleSystem mGunParticles;
    private LineRenderer mGunLine;                           
    private AudioSource mGunAudio;                           
    private Light mGunLight;
    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;
    private bool mUseDebugLine;
    #endregion

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

            if(mUseDebugLine)
            {
                if(mGunLine != null)
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
    void Start () {
        var gameInfo = MyGameObjectFactory.CreateByComponent(this, typeof(IRapidFireGun));
        MyGameObjectsBus.RegisterObject(gameInfo);

        mGunParticles = GetComponent<ParticleSystem>();

        if(mGunParticles == null)
        {
            mGunParticles = GetComponentInChildren<ParticleSystem>();
        }

        mGunLine = GetComponent<LineRenderer>();

        if(mGunLine == null)
        {
            mGunLine = GetComponentInChildren<LineRenderer>();
        }

        mGunAudio = GetComponent<AudioSource>();

        if(mGunAudio == null)
        {
            mGunAudio = GetComponentInChildren<AudioSource>();
        }

        mGunLight = GetComponent<Light>();

        if(mGunLight == null)
        {
            mGunLight = GetComponentInChildren<Light>();
        }

        mGunEndTransform = GunEnd.transform;
    }

    private object mFireModeLockObj = new object();
    private FireMode mFireMode = FireMode.Multiple;

    public FireMode FireMode
    {
        get
        {
            lock(mFireModeLockObj)
            {
                return mFireMode;
            }
        }

        set
        {
            lock (mFireModeLockObj)
            {
                if(mFireMode == value)
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
            lock(mTurnStateLockObj)
            {
                return mTurnState;
            }
        }

        set
        {
            lock (mTurnStateLockObj)
            {
                if(mTurnState == value)
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
    void Update () {
        if(mUseDebugLine)
        {
            mGunLine.enabled = true;
            mGunLine.SetPosition(0, mGunEndTransform.position);
            shootRay.origin = mGunEndTransform.position;
            shootRay.direction = mGunEndTransform.forward;
            mGunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }

        var fireMode = FireMode;
        var turnState = TurnState;

        switch(turnState)
        {
            case TurnState.On:
                switch(mInternalState)
                {
                    case InternalStateOfRapidFireGun.TurnedOf:
                        ProcessShoot();
                        break;

                    case InternalStateOfRapidFireGun.TurnedOnShot:
                        timer += Time.deltaTime;
                        if(timer >= EffectsDisplayTime)
                        {
                            ProcessEndShoot();
                        }
                        break;

                    case InternalStateOfRapidFireGun.TurnedOnWasShot:
                        switch(fireMode)
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
            OnFire?.Invoke();
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
        if(!mUseDebugLine)
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

            if(targetOfShoot != null)
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

    public bool SetToHandsOfHumanoid(IHumanoid humanoid)
    {
#if UNITY_EDITOR
        Debug.Log("Begin RapidFireGun SetToHandsOfHumanoid");
#endif

        Body.transform.SetParent(humanoid.RightHandWP.transform, false);
        //Body.transform.rotation = Quaternion.Euler(0, -180.234f, 0);
        Body.transform.localRotation = Quaternion.Euler(0, -180.234f, 0);
        //Body.transform.rotation = new Quaternion(0, -180.234f, 0, 1);
        Body.transform.localPosition = new Vector3(0, 0, 0.2f);


#if UNITY_EDITOR
        Debug.Log("End RapidFireGun SetToHandsOfHumanoid");
#endif

        return true;
    }
}
