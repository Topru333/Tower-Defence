using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootType),typeof(TargetSystem), typeof(TowerAnimation))]
public class TowerInformation : MonoBehaviour {

    public string Name;                 // Имя башни
    public string Information;          // Краткая информация о ней
    public int lvl = 1;                 // Уровень башни
    public int costForBuy;              // Стоимость башни для покупки
    [Range(0,100)]
    public int sellCoefficient;         // Коэффициент продажи
    public int costForSell {            // Стоимость башни для продажи
        get {
            if (lvl == 1) return (int)((costForBuy / 100M) * sellCoefficient);
            else return (int)((costForBuy + (costForBuy / 2 + lvlСoefficient * (lvl - 1)))/100M) * sellCoefficient;
        }
    }         
    public int lvlСoefficient;          // Коэффициент прокачки
    public int costForLvlUp {           // Стоимость башни для прокачки
        get {
            return costForBuy/2 + lvlСoefficient * lvl;
        }
    }         
    public Sprite Icon;              // Иконка башни в Canvas


}
