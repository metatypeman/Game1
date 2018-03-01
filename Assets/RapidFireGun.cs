using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAimCorrector
{
    float GetCorrectingAngle(Vector3 targetPos);
}

public interface ITargetOfShoot
{
    void SetHit(RaycastHit shootHit, int damagePerShot);
}

public enum FireMode
{
    Single,
    Multiple
}

public enum TurnState
{
    On,
    Off
}

public enum InternalStateOfRapidFireGun
{
    TurnedOf,
    TurnedOnShot,
    TurnedOnWasShot,
    BeforeOffIfSingle
}

public interface IRapidFireGun: IAimCorrector, IHandThing
{
    bool UseDebugLine { get; set; }
    FireMode FireMode { get; set; }
    TurnState TurnState { get; set; }
    event Action OnFire;
}

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
    private ParticleSystem gunParticles;
    private LineRenderer gunLine;                           
    private AudioSource gunAudio;                           
    private Light mGunLight;
    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;
    private bool mUseDebugLine;
#endregion

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
                if(gunLine != null)
                {
                    gunLine.enabled = true;
                }      
            }
            else
            {
                gunLine.enabled = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        mGunLight = GetComponent<Light>();

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
            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
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
            gunLine.enabled = false;
        }
       
        FaceLight.enabled = false;
        mGunLight.enabled = false;
    }

    public void Shoot()
    {
        gunAudio.Play();

        // Включаем всвет
        mGunLight.enabled = true;
        FaceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        // Линия выстрела
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        // Настройка луча
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, Range))
        {
            var targetOfShoot = shootHit.collider.GetComponentInParent<ITargetOfShoot>();

            if(targetOfShoot != null)
            {
                targetOfShoot.SetHit(shootHit, DamagePerShot);
            }
            
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }
    }

    public float GetCorrectingAngle(Vector3 targetPos)
    {
        var targetDir = targetPos - transform.position;
        var forward = transform.forward;
        var angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        return angle;
    }
}
