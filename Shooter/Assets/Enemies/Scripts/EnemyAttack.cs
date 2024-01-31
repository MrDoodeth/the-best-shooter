using UnityEngine;
public class EnemyAttack : Enemy, IAttackable
{   
    private EnemyHealth health;

    [Header("Fight")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float damage = 33f;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float timeBetweenAttack;

    private bool isRun;
    private bool isAttack;
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        Init();
    }

    #region Attack
    private void AttackPlayer()
    {
        if (agent.isOnNavMesh == true)
            agent.SetDestination(transform.position);

        transform.LookAt(playerPosition[0].transform);
        isAttack = true;
        isRun = false;
        if (!alreadyAttacked)
        {
            //attack           
            alreadyAttacked = true;
        }
    }

    public virtual void Attack()
    {
        Collider[] playerHit = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);

        foreach (Collider hit in playerHit)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    #endregion

    private void ChasePlayer()
    {
        if (agent.isOnNavMesh && !animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            agent.SetDestination(playerPosition[0].transform.position);
        isAttack = false;
        isRun = true;
    }
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        AnimationsUpdate();

        if (health.isAlive)
        {
            if (playerInAttackRange)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
    }
    private void AnimationsUpdate()
    {
        animator.SetBool("isRunning", isRun);
        animator.SetBool("isAttacking", isAttack);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
