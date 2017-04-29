using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour {
    

    UpgradableParams upgradableParameters; // Список улучшаемых параметров
    List<Transform> targetList;            // Список целей
    Transform currentTarget;     // Цель для атаки на данный момент

    uint upgradeLevel,          // Уровень башни
         maxUpgradeLevel;       // Максимальный уровень башни

    float baseUpgradePrice,     // Начальная стоимость повышения уровня
          priceIncreasePercent, // Процент изменения стоимости повышения уровня
          targetListUpdateFreq, // Частота обновления списка целей
          basePrice,            // Цена продажи
          sellPrice;            // Цена покупки
    Texture2D icon;

    void FixedUpdate () {
        if (currentTarget != null) {
            
        }
    }

    // Инициализация
    void Awake () {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    // Обновление
    void Update () {
		
	}

    // Функция поиска ближайшей цели
    Collider FindClosest (Collider[] targets, Transform from) { 
        Vector3 position = from.position;
        return targets.OrderBy(o => (o.transform.position - position).sqrMagnitude).FirstOrDefault();
    }

    // Поиск цели с в промежуток времени
    IEnumerator FindTargetsWithDelay (float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            TargetSearch();
        }
    }

    // Поиск целей
    void TargetSearch()
    {
        targetList.Clear();
        currentTarget = null; // Очистка предыдущих при каждом поиске
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, upgradableParameters.TargetSearchRadius, upgradableParameters.targetsMask); // Проверка присутствия целей в радиусе
        if (targetsInViewRadius.Length > 0) {
            currentTarget = FindClosest(targetsInViewRadius, transform).transform;
            for (int i = 0; i < targetsInViewRadius.Length; i++) {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < 360f / 2) {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, upgradableParameters.wallMask)) { // Проверка на стены между верхушкой и целью
                        targetList.Add(target);
                    }
                }
            }
        }
    }

    // Нанесение урона
    void TargetDamage()
    {

    }

    // Повышение уровня
    public void Upgrade()
    {

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

    }

    // Перевод из градусов в радианы
    public static Vector3 DirFromAngle (float angleInDegrees) {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegrees), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

public class UpgradableParams
{
    public LayerMask targetsMask,       // Возможные цели
                     wallMask;          // Стены через которые башня не видет
    public float TargetSearchRadius,    // Радиус поиска целей
                 TargetDamageFrequency; // Частота нанесения урона по целям
    public int   MaxTargetCount;        // Максимальное количество целей
}