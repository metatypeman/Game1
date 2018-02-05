using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    TurnedOnWillShot,
    TurnedOnWasShot
}

public interface IRapidFireGun
{
    FireMode FireMode { get; set; }
    TurnState TurnState { get; set; }
    event Action OnFire;
}

public class PlayerShooting : MonoBehaviour, IRapidFireGun
{
    public GameObject Body;

    ParticleSystem gunParticles;
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;
    public Light faceLight;
    float effectsDisplayTime = 0.4f;

    float timer;
    Ray shootRay;

    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.5f;
    public float range = 100f;
    RaycastHit shootHit;

    // Use this for initialization
    void Start () {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        StartCoroutine(Timer());
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
    
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        faceLight.enabled = false;
        gunLight.enabled = false;
        gunAudio.Stop();
    }

    //public void StartShoot()
    //{
    //    if (timer >= timeBetweenBullets && Time.timeScale != 0)
    //    {
    //        Shoot();
    //    }
    //}

    public void Shoot()
    {
        timer = 0f;

        gunAudio.Stop();
        gunAudio.Play();

        // Включаем всвет
        gunLight.enabled = true;
        faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        // Линия выстрела
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        // Настройка луча
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range))
        {
#if UNITY_EDITOR
            Debug.Log("PlayerShooting Shoot");
#endif

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
