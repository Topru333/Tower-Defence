using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootType : MonoBehaviour {

    

    public List<Attack> Attacks = new List<Attack>();


    public Texture2D Icon;

    public string Name;
    public string Information;

    void Awake () {
        foreach(Attack a in Attacks) {
            a.gunParticles = a.gun.GetComponent<ParticleSystem>();
            a.gunLine = a.gun.GetComponent<LineRenderer>();
            a.gunAudio = a.gun.GetComponent<AudioSource>();
            a.gunLight = a.gun.GetComponent<Light>();   
            a.gunLine.numPositions = a.randomPointsCount;
        }
    }

    void Update () {
        foreach (Attack a in Attacks) {
            a.timer += Time.deltaTime;
            if (a.timer >= a.bulletsDelay * a.effectsDisplayTime) {
                a.DisableEffects();
            }
        }
    }

    

    

    

    [Serializable]
    public class  Attack  {

        public string note;
        public bool shooting;

        public enum TypeOfTargeting {
            AoeAroundSelf,
            AoeAroundTarget,
            SingleTarget,
            ManyTargets
        }
        public enum TypeOfAttack {
            Fire,
            Poison,
            Physics,
            Ice,
            Electric
        }

        public TypeOfTargeting targetType;
        public TypeOfAttack attackType;

        public int damage;
        // Задержка
        [Range(0.1f, 1f)]
        public float bulletsDelay;
        // Время показа эффекта выстрела.
        [Range(0.1f, 1f)]
        public float effectsDisplayTime;
        // Количество рандомных точек.
        [Range(2, 10)]
        public int randomPointsCount;
        // Точка выстрела
        public GameObject gun;

        public float timer;

        Ray shootRay = new Ray();
        public ParticleSystem gunParticles;
        public LineRenderer gunLine;
        public AudioSource gunAudio;
        public Light gunLight;

        public void DisableEffects () {
            gunLine.enabled = false;
            gunLight.enabled = false;
            shooting = false;
        }
        public void Shoot (List<Transform> targets) {
            shooting = true;
            if (timer >= bulletsDelay && Time.timeScale != 0) {
                timer = 0f;

                gunAudio.Play();

                gunLight.enabled = true;

                gunParticles.Stop();
                gunParticles.Play();

                gunLine.enabled = true;
                gunLine.SetPosition(0, gun.transform.position);

                shootRay.origin = gun.transform.position;
                shootRay.direction = gun.transform.forward;
                if (targetType == TypeOfTargeting.ManyTargets || targetType == TypeOfTargeting.AoeAroundTarget || targetType == TypeOfTargeting.AoeAroundSelf) {
                    switch (attackType) {
                        case TypeOfAttack.Electric: {
                                HitElectric(targets[UnityEngine.Random.Range(0,targets.Count)]);
                                break;
                            }
                        case TypeOfAttack.Fire: {

                                break;
                            }
                        case TypeOfAttack.Ice: {

                                break;
                            }
                        case TypeOfAttack.Poison: {

                                break;
                            }
                        case TypeOfAttack.Physics: {

                                break;
                            }
                        default:
                            break;
                            
                    }
                    
                }
                else HitSingle(targets[targets.Count -1]);
            }
        }
        private void HitElectric (Transform target) {
            
            for (int i = 1; i < randomPointsCount - 1; i++) {
                float coeff = ((float)i) / (randomPointsCount - 1);
                Vector3 pos = gun.transform.position * (1 - coeff) + target.position * coeff;
                float randCoeff = Vector3.Distance(gun.transform.position, pos) / 20 + 0.01f;
                gunLine.SetPosition(i, pos + UnityEngine.Random.insideUnitSphere * randCoeff);
            }
            gunLine.SetPosition(randomPointsCount - 1, target.position);
            target.SendMessage("AddDamage",damage);
            
        }
        private void HitSingle (Transform target) {
            gunLine.SetPosition(1, target.position);

        }
    }


}

