using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject[] playerPosition;
    protected bool playerInAttackRange;
    protected bool isAlive = true;
    protected bool onFloor;
    protected bool alreadyAttacked;
    protected Rigidbody rb;
    protected Collider col;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        playerPosition = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            onFloor = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            onFloor = false;
    }
}
