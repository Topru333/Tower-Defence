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
    public class NPC_ManualTests : MonoBehaviour
    {

        public string testfile_path;
        public int startID;
        public GameObject npc;
        public NpcWave wave;


        // Use this for initialization
        void Start()
        {
            using (StreamReader sr =new StreamReader(File.OpenRead(testfile_path)))
                LevelManager.Instance.LoadPathGraph(sr);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(NPC_ManualTests))]
    class NPC_ManualTestsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Application.isPlaying && GUILayout.Button("Spawn"))
                PathSystem.Instance.NPCSpawn(((NPC_ManualTests)target).wave);
        }
    }
#endif
}