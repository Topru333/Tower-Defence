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
            a.OnStart(transform);
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
}

[Serializable]
public class Attack {
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
    public float timer;                  // Таймер для задержки

    private TowerAnimation ta;           // Ссылка на скрипт анимации

    #region ManyTargets
    // При выборе режима "ManyTargets" 
    [Range(1, 30)]
    public int countOfTargets;           // Кол-во целей 
    public float disToNextTarget;        // Дистанция до следующей цели при атаке молнией  
    #endregion

    #endregion

    public void DisableEffects () {
        // Стирание эффета
        ta.StopAnimation();
    }
    
    public void OnStart (Transform tower) {
        ta = tower.gameObject.GetComponent<TowerAnimation>();
        ta.OnStart(attackType);
    }
    public void Shoot (List<Transform> targets, Transform currentTarget) {
        if (timer >= bulletsDelay && Time.timeScale != 0) {
            timer = 0f;
            // Выбор спсобов выстрела по типу атаки и таргета
            if (targetType == TypeOfTargeting.ManyTargets || targetType == TypeOfTargeting.AoeAroundTarget || targetType == TypeOfTargeting.AoeAroundSelf) {
                switch (attackType) {
                    case TypeOfAttack.Electric: {
                            HitElectric(targets, currentTarget);
                            break;
                        }
                    case TypeOfAttack.Fire: {
                            ta.StartAnimation();

                            break;
                        }
                    case TypeOfAttack.Ice: {
                            HitFreeze(targets);
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
            else {
                HitSinglePhysics(currentTarget);
            }
        }
    }
    #region Hit
    /// <summary>
    /// Методы с Hit в названии вызываются при атаке
    /// </summary>
    /// <param name="target">Цель удара</param>
    private void HitElectric (List<Transform> targets, Transform currentTarget) {
        ta.StartAnimation(currentTarget,targets);
    }
    private void HitSinglePhysics (Transform target) {
        ta.StartAnimation(target);
    }
    private void HitFreeze (List<Transform> targets) {
        ta.StartAnimation();
    }
    #endregion

}