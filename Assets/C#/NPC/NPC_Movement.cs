using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour {
    public  Transform nextWayPoint;
    public  int maxHealth=100;
    public  float fireResist;
    private NavMeshAgent agent;
    private int currentHealth;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update () {
        agent.SetDestination(nextWayPoint.position);
    }
}