using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    public List<GameObject> towersInScene = new List<GameObject>(); // Список башен находящихся в сцене
    public List<GameObject> towerTypes = new List<GameObject>();    // Список башен которые можно воссоздать

    public void AddTower (int index) {                              //Функция добавления башни в сцене
        towersInScene.Add(Instantiate(towerTypes[index]));
    }
    public void DeleteTower (GameObject tower) {                    //Функция удаления башни в сцене
        towersInScene.Remove(tower);
        GameObject.Destroy(tower);
    }
    void Start () {
        //AddTower(1);
        //DeleteTower(towersInScene[0]);
        //Для теста
    }
}
