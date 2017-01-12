using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour {

    /// <summary>
    /// Скрипт для поворота башни к цели и дальнейших действий
    /// </summary>

    public Transform upOfTower; // Верхушка башни 

    [Range(0, 200)]
    public float ViewRadius = 50f; // Радиус башни
    [Range(0, 360)]
    public float ViewAngle = 90f; // Угол обзора башни
    public float dampingRotation = 5f; // Скорость поворота

    public LayerMask targetsMask; // Возможные цели
    public LayerMask obstacleMask; // Стены через которые башня не видет

    public List<Transform> visibleTargets = new List<Transform>();// Список всех в поле видимости
    public Transform currentTarget; // Цель для атаки на данный момент

    ShootType shootType;

    // Поиск цели с в промежуток времени
    IEnumerator FindTargetsWithDelay (float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void Awake () {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }
    void Start () {
        if (upOfTower == null) upOfTower = transform; // Если верхушки нету то башня обозначает себя как верхушку.
        shootType = GetComponent<ShootType>();
    }

    void FixedUpdate () {
        if (currentTarget != null) {
            RotationTo(currentTarget); // Поворот верхушки к цели
            if(shootType.timer >= shootType.timeBetweenBullets && Time.timeScale != 0)
                shootType.Shoot(currentTarget);
        }
    }

    // Перевод из градусов в радианы
    public Vector3 DirFromAngle (float angleInDegrees) {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegrees), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Функция поиска целей
    void FindVisibleTargets () {
        visibleTargets.Clear();
        currentTarget = null; // Очистка предыдущих при каждом поиске
        Collider[] targetsInViewRadius = Physics.OverlapSphere(upOfTower.position, ViewRadius, targetsMask); // Проверка присутствия целей в радиусе
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - upOfTower.position).normalized;
            if (Vector3.Angle(upOfTower.forward, dirToTarget) < ViewAngle / 2) {
                float dstToTarget = Vector3.Distance(upOfTower.position, target.position);

                if (!Physics.Raycast(upOfTower.position, dirToTarget, dstToTarget, obstacleMask)) { // Проверка на стены между верхушкой и целью
                    visibleTargets.Add(target);
                    currentTarget = target;
                }
            }
        }

    }

    // Поворот верхушки
    void RotationTo (Transform target) {
        var lookPos = target.position - upOfTower.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        upOfTower.rotation = Quaternion.Slerp(upOfTower.rotation, rotation, Time.deltaTime * dampingRotation);
    }

}
