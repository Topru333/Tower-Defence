using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TowersPanel : MonoBehaviour {

    public GameObject towerButton;
    public void Awake () {
        int i = 0;
        foreach (GameObject tower in TowerManager.instance.towerStore) {

            GameObject go = Instantiate(towerButton);
            go.GetComponent<Button>().onClick.AddListener(() => AddTower(tower));
            go.transform.SetParent(transform);
            go.GetComponent<Image>().sprite = tower.GetComponent<TowerInformation>().Icon;
            go.SetActive(true);
            i++;
        }
    }
    public void AddTower (GameObject tower) {
        if (TowerManager.instance.GetTowerToBuild() == tower && TowerManager.instance.BuildMode) {
            TowerManager.instance.BuildMode = false;
            return;
        }
        TowerManager.instance.BuildMode = true;
        TowerManager.instance.SetTowerToBuild (tower);
       
    }
}
