using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


public class ShootType : MonoBehaviour {
    public List<Attack> Attacks = new List<Attack>(); // Список всех типов атак данной башни

    void Awake () {
        // Добавление ссылок на компоненты эффекта
        foreach(Attack a in Attacks) {
            a.gunParticles = a.gunPoint.GetComponent<ParticleSystem>(); // Сам эффект
            a.gunLine = a.gunPoint.GetComponent<LineRenderer>();        // Прорисовка линии
            a.gunAudio = a.gunPoint.GetComponent<AudioSource>();        // Звук при выстреле
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
        public int damage;                   // Кол-во дамага на 1 способ атаки
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
       
        #region ManyTargets
        // При выборе режима "ManyTargets" 
        [Range(1, 30)]
        public int countOfTargets;           // Кол-во целей 
        public float disToNextTarget;        // Дистанция до следующей цели при атаке молнией  
        #endregion

        #endregion

        public void DisableEffects () {
            // Стирание эффета
            switch (attackType) {
                case TypeOfAttack.Physics: {
                        gunLine.enabled = false;
                        break;
                    }
                case TypeOfAttack.Electric: {
                        gunLine.enabled = false;
                        break;
                    }
            }
            
            
        }
        public void Shoot (List<Transform> targets, Transform currentTarget) {
            if (timer >= bulletsDelay && Time.timeScale != 0) {
                timer = 0f;
                //Включаем звук выстрела
                gunAudio.Play();
                gunParticles.Stop();
                gunParticles.Play();
                shootRay.origin = gunPoint.transform.position;
                shootRay.direction = gunPoint.transform.forward;

                

                // Выбор спсобов выстрела по типу атаки и таргета
                if (targetType == TypeOfTargeting.ManyTargets || targetType == TypeOfTargeting.AoeAroundTarget || targetType == TypeOfTargeting.AoeAroundSelf) {
                    switch (attackType) {
                        case TypeOfAttack.Electric: {
                                HitElectric(targets,currentTarget);
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
                else HitSinglePhysics(currentTarget);
            }
        }
        #region Hit
        /// <summary>
        /// Методы с Hit в названии вызываются при атаке
        /// </summary>
        /// <param name="target">Цель удара</param>
        private void HitElectric (List<Transform> targets,Transform currentTarget) {
            Transform target = currentTarget; // Берем ближайшую цель и начинаем цепочку электричества от неё
            Transform from = gunPoint.transform;
            if (targets.Count > 1) { // Если целей больше чем одна
                gunLine.enabled = true;
                gunLine.SetPosition(0, from.transform.position);
                float coeff, randCoeff;
                for (int o = 1; o < randomPointsCount - 1; o++) {
                    coeff = ((float)o) / (randomPointsCount - 1);
                    Vector3 pos = from.position * (1 - coeff) + target.position * coeff;
                    randCoeff = Vector3.Distance(from.position, pos) / 20 + 0.01f;
                    gunLine.SetPosition(o, pos + UnityEngine.Random.insideUnitSphere * randCoeff);
                }
                gunLine.SetPosition(randomPointsCount - 1, target.position);
            }
            else {
                gunLine.enabled = true;
                gunLine.SetPosition(0, from.transform.position);
                float coeff, randCoeff;
                for (int o = 1; o < randomPointsCount - 1; o++) {
                    coeff = ((float)o) / (randomPointsCount - 1);
                    Vector3 pos = from.position * (1 - coeff) + target.position * coeff;
                    randCoeff = Vector3.Distance(from.position, pos) / 20 + 0.01f;
                    gunLine.SetPosition(o, pos + UnityEngine.Random.insideUnitSphere * randCoeff);
                }
                gunLine.SetPosition(randomPointsCount - 1, target.position);
            }
        }
        private void HitSinglePhysics (Transform target) {
            gunLine.enabled = true;
            gunLine.SetPosition(0, gunPoint.transform.position);
            gunLine.SetPosition(1, target.position);
        }
        #endregion

    }
}

