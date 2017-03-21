using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetSystem : MonoBehaviour {

    /// <summary>
    /// Скрипт для поворота башни к цели и дальнейших действий
    /// </summary>
    #region Parameters
    public bool Rotate = false;         // Вращается ли башня
    public bool rotateY;                // Ось вращения пушки
    public bool rotateX;                // Ось вращения верхукши
    public Transform centerGunX;        // Пушка для вращения по Y
    public Transform upOfTower;         // Верхушка башни 
    [Range(0, 360)]
    public float ViewRadius = 50f;      // Радиус башни
    [Range(0, 360)]
    public float ViewAngle = 90f;       // Угол обзора башни
    [Range(1, 50)]
    public float dampingRotation = 5f;  // Скорость поворота
    public LayerMask targetsMask;       // Возможные цели
    public LayerMask wallMask;          // Стены через которые башня не видет
    public Transform currentTarget;     // Цель для атаки на данный момент
    public List<Transform> visibleTargets = new List<Transform>(); // Список всех в поле видимости
    ShootType shootType;                // Ссылка на скрипт в префабе
    #endregion

    // Поиск цели с в промежуток времени
    IEnumerator FindTargetsWithDelay (float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void Awake () {
        StartCoroutine("FindTargetsWithDelay", .2f);
        shootType = GetComponent<ShootType>();
    }

    void FixedUpdate () {
        if (currentTarget != null) {
            RotationTo(currentTarget); // Поворот верхушки к цели
            foreach (Attack a in shootType.Attacks) {
                a.Shoot(visibleTargets, currentTarget);
            }

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
        if (targetsInViewRadius.Length > 0) {
            currentTarget = FindClosest(targetsInViewRadius, transform).transform;
            for (int i = 0; i < targetsInViewRadius.Length; i++) {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - upOfTower.position).normalized;
                if (Vector3.Angle(upOfTower.forward, dirToTarget) < ViewAngle / 2) {
                    float dstToTarget = Vector3.Distance(upOfTower.position, target.position);

                    if (!Physics.Raycast(upOfTower.position, dirToTarget, dstToTarget, wallMask)) { // Проверка на стены между верхушкой и целью
                        visibleTargets.Add(target);
                    }
                }
            }
        }
    }



    // Поворот верхушки
    void RotationTo (Transform target) {
        if (Rotate) {
            var lookPosUp = target.position - upOfTower.position;
            var lookPosGun = lookPosUp;

            if (rotateY) {
                lookPosUp.y = 0;
                var rotationUp = Quaternion.LookRotation(lookPosUp);
                upOfTower.rotation = Quaternion.Slerp(upOfTower.rotation, rotationUp, Time.deltaTime * dampingRotation);
            }
            if (rotateX && centerGunX != null) {
                var rotationGun = Quaternion.LookRotation(lookPosGun);
                centerGunX.rotation = Quaternion.Slerp(centerGunX.rotation, rotationGun, Time.deltaTime * dampingRotation);
            }
        }
        
    }
    private Collider FindClosest (Collider[] targets, Transform from) { // Функция поиска ближайшей цели
        Vector3 position = from.position;
        return targets
            .OrderBy(o => (o.transform.position - position).sqrMagnitude)
            .FirstOrDefault();
    }
}
