using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    List<UpgradableParams> upgradableParameters; // Список улучшаемых параметров
    List<NPC> targetList;                        // Список целей

    uint upgradeLevel,          // Уровень башни
         maxUpgradeLevel;       // Максимальный уровень башни

    float baseUpgradePrice,     // Начальная стоимость повышения уровня
          priceIncreasePercent, // Процент изменения стоимости повышения уровня
          targetListUpdateFreq, // Частота обновления списка целей
          basePrice,            // Цена продажи
          sellPrice;            // Цена покупки
    Texture2D icon;


    // Инициализация
    void Awake () {
		
	}

    // Обновление
    void Update () {
		
	}

    // Поиск целей
    void TargetSearch()
    {

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
    
}

public class UpgradableParams
{
    public float TargetSearchRadius,    // Радиус поиска целей
                 TargetDamageFrequency; // Частота нанесения урона по целям
    public int   MaxTargetCount;        // Максимальное количество целей
}