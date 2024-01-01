using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected GameObject[] playerPosition;
    protected bool playerInAttackRange;
    public bool isAlive = true;
    protected Enemy mainEnemy;
    protected bool onFloor;
    protected bool alreadyAttacked;
    protected Rigidbody rb;
    protected Collider col;
    void Start()
    {
        Init();
    }

    protected void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mainEnemy = this;
        playerPosition = GameObject.FindGameObjectsWithTag("Player");
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
