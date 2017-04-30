using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ManualTests : MonoBehaviour {

    public string testfile_path;
    public int startID;
    public GameObject npc;
	// Use this for initialization
	void Start () {

        LevelManager.Instance.LoadPathGraph(testfile_path);
        NpcWave wave;
        wave.count = 10;
        wave.delay = 1;
        wave.NPC = npc;
        wave.reward = 10;
        PathSystem.Instance.NPCSpawn(wave, startID);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
