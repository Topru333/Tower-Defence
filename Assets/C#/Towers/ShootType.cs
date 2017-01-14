using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootType : MonoBehaviour {
    public List<Attack> Attacks = new List<Attack>(); // Список всех типов атак данной башни
    void Awake () {
        // Добавление ссылок на компоненты эффекта
        foreach(Attack a in Attacks) {
            a.gunParticles = a.gunPoint.GetComponent<ParticleSystem>(); // Сам эффект
            a.gunLine = a.gunPoint.GetComponent<LineRenderer>();        // Прорисовка линии
            a.gunAudio = a.gunPoint.GetComponent<AudioSource>();        // Звук при выстреле
            a.gunLight = a.gunPoint.GetComponent<Light>();              // Испускабщий свет
            a.gunLine.numPositions = a.randomPointsCount;
        }
    }
    void Update () {
        // Стирает прорисованный эффект
        foreach (Attack a in Attacks) {
            a.timer += Time.deltaTime;
            if (a.timer >= a.bulletsDelay * a.effectsDisplayTime) {
                a.DisableEffects();
            }
        }
    }

    [Serializable]
    public class  Attack  {
        public enum TypeOfTargeting {        // Способы прицеливания
            AoeAroundSelf,
            AoeAroundTarget,
            SingleTarget,
            ManyTargets
        }
        public enum TypeOfAttack {           // Типы атак
            Fire,
            Poison,
            Physics,
            Ice,
            Electric
        }
        #region Parameters
        public TypeOfTargeting targetType;
        public TypeOfAttack attackType;
        public string note;                  // Краткая заметка для удобства определения
        public bool shooting;                // Переменная дающая знать момент выстрела
        public int damage;                   // Кол во дамага на 1 способ атаки
        [Range(0.1f, 1f)]
        public float bulletsDelay;           // Задержка     
        [Range(0.1f, 1f)]
        public float effectsDisplayTime;     // Время показа эффекта выстрела.
        [Range(2, 10)]
        public int randomPointsCount;        // Количество рандомных точек.
        public GameObject gunPoint;          // Точка выстрела
        public float timer;                  // Таймер для задержки
        Ray shootRay = new Ray();            // Линия видимости балистики
        public ParticleSystem gunParticles;  // Ссылка на эффект
        public LineRenderer gunLine;         // Ссылка на прорисовку лайна встрела
        public AudioSource gunAudio;         // Ссылка на звуковой эффект
        public Light gunLight;               // Ссылка на источник света при выстреле
        #endregion

        public void DisableEffects () {
            // Стирание эффета
            gunLine.enabled = false;
            gunLight.enabled = false;
            shooting = false;
        }
        public void Shoot (List<Transform> targets) {
            shooting = true;
            if (timer >= bulletsDelay && Time.timeScale != 0) {
                timer = 0f;
                //Включаем звук выстрела
                gunAudio.Play();
                //Вспышка света
                gunLight.enabled = true;

                gunParticles.Stop();
                gunParticles.Play();

                gunLine.enabled = true;
                gunLine.SetPosition(0, gunPoint.transform.position);

                shootRay.origin = gunPoint.transform.position;
                shootRay.direction = gunPoint.transform.forward;

                // Выбор спсобов выстрела по типу атаки и таргета
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


        /// <summary>
        /// Методы с Hit в названии вызываются при атаке
        /// </summary>
        /// <param name="target">Цель удара</param>
        private void HitElectric (Transform target) { 
            // Берем рандомные точки на растоянии от башни до цели
            for (int i = 1; i < randomPointsCount - 1; i++) {
                float coeff = ((float)i) / (randomPointsCount - 1);
                Vector3 pos = gunPoint.transform.position * (1 - coeff) + target.position * coeff;
                float randCoeff = Vector3.Distance(gunPoint.transform.position, pos) / 20 + 0.01f;
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

