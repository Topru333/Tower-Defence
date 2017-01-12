using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour {
    public  Transform nextWayPoint;
    private NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update () {
        agent.SetDestination(nextWayPoint.position);
    }
}