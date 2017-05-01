using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD;
using UnityEditor;
using System.IO;

public class Tower_ManualTests : MonoBehaviour {
    public string testfile_path;
    public GameObject tower;
    public Vector3 position;
    public Vector3 gridPos;
    // Use this for initialization
    void Start () {
        var tgs = TowerGridSystem.Instance;
        tgs.transform.SetPositionAndRotation(gridPos, Quaternion.identity);
        tgs.LoadData(File.OpenRead(testfile_path));

        if (!tgs.BuildTowerAt(position, tower))
            Debug.Log(tgs.GetCellInfo(position));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
[CustomEditor(typeof(Tower_ManualTests))]
class Tower_ManualTestsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            var tgs = TowerGridSystem.Instance;
            if (GUILayout.Button("UpdatePos"))
            {
                tgs.transform.SetPositionAndRotation(((Tower_ManualTests)target).gridPos, Quaternion.identity);
                Debug.Log(tgs);
            }
            if (GUILayout.Button("BuildTower"))
            {
                if (!tgs.BuildTowerAt(((Tower_ManualTests)target).position, ((Tower_ManualTests)target).tower))
                    Debug.Log(tgs.GetCellInfo(((Tower_ManualTests)target).position));
            }
            if (GUILayout.Button("ReloadData"))
            {
                tgs.LoadData(File.OpenRead(((Tower_ManualTests)target).testfile_path));
            }
        }
    }
}