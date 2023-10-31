using ShooterInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Enemy, IAttackable
{
    [Header("Fight")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float damage = 33f;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float timeBetweenAttack;
    void Start()
    {
        Init();
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerPosition[0].transform);

        if (!alreadyAttacked)
        {
            //attack           
            alreadyAttacked = true;
            Attack(damage);
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ChasePlayer()
    {
        if (agent.isOnNavMesh == true)
            agent.SetDestination(playerPosition[0].transform.position);
    }
    public virtual void Attack(float _damage)
    {
        Collider[] playerHit = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);

        foreach (Collider hit in playerHit)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (isAlive)
        {
            if (playerInAttackRange)
            {
                AttackPlayer();
            }
            else
            {
                Invoke(nameof(ChasePlayer), 0.5f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
