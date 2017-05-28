using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace TD
{
    public class Tower : MonoBehaviour
    {

        // TODO: Сделать списком. Так же добавить загрузку из потока
        UpgradableParams[] upgradableParameters = new UpgradableParams[]{new UpgradableParams() {
            targetListUpdateFreq = 3f,
            TowerDamage=15,
            MaxTargetCount=2,
            TargetSearchRadius=25
        } }; // Список улучшаемых параметров
        List<Transform> targetList=new List<Transform>();            // Список целей

        uint upgradeLevel,          // Уровень башни
             maxUpgradeLevel=1;       // Максимальный уровень башни

        float baseUpgradePrice,     // Начальная стоимость повышения уровня
              priceIncreasePercent, // Процент изменения стоимости повышения уровня
              timer,                // Для проверки частоты удара
              basePrice,            // Цена продажи
              sellPrice;            // Цена покупки
        public Sprite icon;
        public LayerMask lm;

        void FixedUpdate()
        {
            
            if (timer >= upgradableParameters[upgradeLevel].TargetDamageFrequency)
            {
                timer = 0f;
                TargetDamage();
            }
        }

        // Инициализация
        void Awake()
        {
            for(int i=0;i<maxUpgradeLevel;i++)
                upgradableParameters[i].targetsMask = lm;
            StartCoroutine("FindTargetsWithDelay", upgradableParameters[upgradeLevel].targetListUpdateFreq);
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
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, upgradableParameters[upgradeLevel].TargetSearchRadius, upgradableParameters[upgradeLevel].targetsMask); // Проверка присутствия целей в радиусе
            if (targetsInViewRadius.Length > 0)
            {
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < 360f / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, upgradableParameters[upgradeLevel].wallMask))
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
            for (int i = 0; i < upgradableParameters[upgradeLevel].MaxTargetCount && i < targetList.Count; i++)
            {
                if (targetList[i])
                {
                    NPC npc = targetList[i].gameObject.GetComponent<NPC>();
                    npc.DoDamage(upgradableParameters[upgradeLevel].TowerDamage);
                }
            }
        }

        // Повышение уровня
        public void Upgrade()
        {
            if(upgradeLevel<maxUpgradeLevel)
                upgradeLevel++;
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
            Gizmos.DrawSphere(transform.position, upgradableParameters[upgradeLevel].TargetSearchRadius);
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