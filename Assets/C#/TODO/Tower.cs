﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace TD
{
    public class Tower : MonoBehaviour
    {

        // TODO: Сделать списком. Так же добавить загрузку из потока
        UpgradableParams upgradableParameters = new UpgradableParams() {
            targetListUpdateFreq = 3f,
            TowerDamage=15,
            MaxTargetCount=2,
            TargetSearchRadius=5,
        }; // Список улучшаемых параметров
        List<Transform> targetList=new List<Transform>();            // Список целей

        uint upgradeLevel,          // Уровень башни
             maxUpgradeLevel;       // Максимальный уровень башни

        float baseUpgradePrice,     // Начальная стоимость повышения уровня
              priceIncreasePercent, // Процент изменения стоимости повышения уровня
              timer,                // Для проверки частоты удара
              basePrice,            // Цена продажи
              sellPrice;            // Цена покупки
        Texture2D icon;
        public LayerMask lm;

        void FixedUpdate()
        {
            
            if (timer >= upgradableParameters.TargetDamageFrequency)
            {
                timer = 0f;
                TargetDamage();
            }
        }

        // Инициализация
        void Awake()
        {
            upgradableParameters.targetsMask = lm;
            StartCoroutine("FindTargetsWithDelay", upgradableParameters.targetListUpdateFreq);
        }

        // Обновление
        void Update()
        {

        }

        // Поиск цели с в промежуток времени
        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                TargetSearch();
                timer += delay;
            }
        }

        // Поиск целей
        void TargetSearch()
        {
            targetList.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, upgradableParameters.TargetSearchRadius, upgradableParameters.targetsMask); // Проверка присутствия целей в радиусе
            if (targetsInViewRadius.Length > 0)
            {
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < 360f / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, upgradableParameters.wallMask))
                        { // Проверка на стены между верхушкой и целью
                            targetList.Add(target);
                        }
                    }
                }
            }
            targetList.OrderBy(target => transform.position - target.position); //Сортировка по дальности
        }

        // Нанесение урона
        void TargetDamage()
        {
            for (int i = 0; i < upgradableParameters.MaxTargetCount && i < targetList.Count; i++)
            {
                if (targetList[i])
                {
                    NPC npc = targetList[i].gameObject.GetComponent<NPC>();
                    npc.DoDamage(upgradableParameters.TowerDamage);
                }
            }
        }

        // Повышение уровня
        public void Upgrade()
        {
            upgradeLevel++;
            upgradableParameters.TowerDamage *= 2;
            upgradableParameters.TargetSearchRadius *= 1.3f;
            upgradableParameters.TargetDamageFrequency *= 1.1f;
        }

        // Обработчик инициализации классов наследников
        public virtual void Init()
        {

        }

        // Визуализация спецэффектов
        public virtual void DrawEffects()
        {

        }

        // Обновление логики
        public virtual void UpdateLogic()
        {

        }

        // Визуализация информации
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
            Gizmos.DrawSphere(transform.position, upgradableParameters.TargetSearchRadius);
        }

        // Перевод из градусов в радианы
        public static Vector3 DirFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegrees), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }

    public class UpgradableParams
    {
        public LayerMask targetsMask,       // Возможные цели
                         wallMask;          // Стены через которые башня не видет
        public float TargetSearchRadius,    // Радиус поиска целей
                         TargetDamageFrequency, // Частота нанесения урона по целям
                         targetListUpdateFreq;  // Частота обновления списка целей
        public int MaxTargetCount,        // Максимальное количество целей
                         TowerDamage;
    }
}