using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    public GameObject Body;

    ParticleSystem gunParticles;                    // Эффект выстрела
    LineRenderer gunLine;                           // Линия выстрела
    AudioSource gunAudio;                           // Звук выстрела
    Light gunLight;
    public Light faceLight;
    float effectsDisplayTime = 0.4f;

    float timer;
    Ray shootRay;

    public int damagePerShot = 20;                  // Уорона за выстрел
    public float timeBetweenBullets = 0.5f;
    public float range = 100f;
    RaycastHit shootHit;

    // Use this for initialization
    void Start () {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        //StartShoot();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
        //gunLine.SetPosition(0, transform.position);
        //// Настройка луча
        //shootRay.origin = transform.position;
        //shootRay.direction = transform.forward;

        //if (Physics.Raycast(shootRay, out shootHit, range))
        //{
        //    gunLine.SetPosition(1, shootHit.point);
        //}
        //else
        //{
        //    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        //}
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        faceLight.enabled = false;
        gunLight.enabled = false;
    }

    public void StartShoot()
    {
        if (timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
    }

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
