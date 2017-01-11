using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootType : MonoBehaviour {

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public GameObject gun;

    public float timer;
    Ray shootRay = new Ray();
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Start () {
        gunParticles = gun.GetComponent<ParticleSystem>();
        gunLine = gun.GetComponent<LineRenderer>();
        gunAudio = gun.GetComponent<AudioSource>();
        gunLight = gun.GetComponent<Light>();
    }

    void Update () {
        timer += Time.deltaTime;
        if (timer >= timeBetweenBullets * effectsDisplayTime) {
            DisableEffects();
        }
    }

    void DisableEffects () {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void Shoot (Transform target) {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, gun.transform.position);

        shootRay.origin = gun.transform.position;
        shootRay.direction = gun.transform.forward;

        Hit(target);
    }

    private void Hit (Transform target) {
        gunLine.SetPosition(1, target.position);
    }
}
