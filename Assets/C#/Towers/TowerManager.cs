using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    public List<GameObject> towersInScene = new List<GameObject>(); // Список башен находящихся в сцене
    public List<GameObject> towerStore = new List<GameObject>();    // Список башен которые можно воссоздать
    public static TowerManager instance;
    public bool BuildMode = false;
    private GameObject turretToBuild;
    public void AddTower (int index) {                              //Функция добавления башни в сцене
        towersInScene.Add(Instantiate(towerStore[index]));
    }
    public void DeleteTower (GameObject tower) {                    //Функция удаления башни в сцене
        towersInScene.Remove(tower);
        GameObject.Destroy(tower);
    }
    void Awake () {
        if(instance != null) { Debug.LogError("More than 1 Tower Manager in scene!"); return; }
        instance = this;
    }
    void Start () {
        turretToBuild = towerStore[0];
    }
    public GameObject GetTowerToBuild () {
        if (turretToBuild == null) return towerStore[0];
        return turretToBuild;
    }
    public void  SetTowerToBuild (GameObject t) {
        turretToBuild = t;
    }


}
