using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootType : MonoBehaviour {
    // Урон
    public int damagePerShot = 20;
    // Задержка
    public float timeBetweenBullets = 0.15f;
    // Точка выстрела
    public GameObject gun;

    // Время показа эффекта выстрела.
    public float effectsDisplayTime = 0.2f;

    // Количество рандомных точек.
    [Range(2, 10)]
    public int randomPointsCount;

    public float timer;

    Ray shootRay = new Ray();
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;

    void Awake () {
        gunParticles = gun.GetComponent<ParticleSystem>();
        gunLine = gun.GetComponent<LineRenderer>();
        gunAudio = gun.GetComponent<AudioSource>();
        gunLight = gun.GetComponent<Light>();
        gunLine.numPositions = randomPointsCount;
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

        shootRay.origin     = gun.transform.position;
        shootRay.direction  = gun.transform.forward;

        Hit(target);
    }

    private void Hit (Transform target) {
        for (int i = 1; i < randomPointsCount - 1; i++)
        {
            float coeff = ((float)i) / (randomPointsCount - 1);
            Vector3 pos = gun.transform.position * (1 - coeff) + target.position * coeff;
            float randCoeff= Vector3.Distance(gun.transform.position, pos)/ 20+0.01f;
            gunLine.SetPosition(i, pos + Random.insideUnitSphere* randCoeff);
        }
        gunLine.SetPosition(randomPointsCount-1, target.position);
    }
}
