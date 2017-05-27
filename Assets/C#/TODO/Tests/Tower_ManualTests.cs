using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
namespace TD.Tests
{
    public class Tower_ManualTests : MonoBehaviour
    {
        public string testfile_path;
        public GameObject tower;
        public Vector3 position;
        public Vector3 gridPos;
        // Use this for initialization
        void Start()
        {
            var tgs = TowerGridSystem.Instance;
            tgs.transform.SetPositionAndRotation(gridPos, Quaternion.identity);
            using (StreamReader sr = new StreamReader(File.OpenRead(testfile_path)))
                tgs.LoadData(sr);

            if (!tgs.BuildTowerAt(position, tower))
                Debug.Log(tgs.GetCellInfo(position));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
#if UNITY_EDITOR
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
                    using(StreamReader sr= new StreamReader( File.OpenRead(((Tower_ManualTests)target).testfile_path)))
                        tgs.LoadData(sr);
                }
            }
        }
    } 
#endif
}