using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Rigidbody[] allRigidbodys;
    protected GameObject[] playerPosition;
    protected bool playerInAttackRange;
    protected bool onFloor;
    protected bool alreadyAttacked;
    protected Collider collider;
    void Start()
    {
        Init();
    }

    protected void Init()
    {
        allRigidbodys = GetComponentsInChildren<Rigidbody>();
        collider = GetComponentInChildren<Collider>();
        MakeKinematic(true);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    protected void MakeKinematic(bool _iskinematic)
    {
        for (int i = 0; i < allRigidbodys.Length; i++)
        {
            allRigidbodys[i].isKinematic = _iskinematic;
        }
    }
}
