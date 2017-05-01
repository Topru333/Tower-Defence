using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD;
using UnityEditor;
using System.IO;

public class NPC_ManualTests : MonoBehaviour {

    public string testfile_path;
    public int startID;
    public GameObject npc;
    public NpcWave wave;


    // Use this for initialization
    void Start () {
        LevelManager.Instance.LoadPathGraph(File.OpenRead(testfile_path));
    }
    // Update is called once per frame
    void Update () {
        
    }
}
[CustomEditor(typeof(NPC_ManualTests))]
class NPC_ManualTestsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying&&GUILayout.Button("Spawn"))
                PathSystem.Instance.NPCSpawn(((NPC_ManualTests)target).wave, ((NPC_ManualTests)target).startID);
    }
}