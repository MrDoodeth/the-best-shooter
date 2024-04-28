using UnityEngine;
[RequireComponent(typeof(Enemy))]
public class EnemyAttack : MonoBehaviour
{
    [Header("Fight")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private int damage = 33;
    [SerializeField] private LayerMask whatIsPlayer;

    private Enemy enemy;
    //States
    private bool isRun;
    private bool isAttack;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    #region Attack
    private void AttackPlayer()
    {
        if (enemy.agent.isOnNavMesh)
            enemy.agent.SetDestination(transform.position);

        transform.LookAt(enemy.playerPosition);
        isAttack = true;
        isRun = false;
    }

    public void Attack()
    {
        Collider[] playerHit = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);

        foreach (Collider hit in playerHit)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
    #endregion

    private void ChasePlayer()
    {
        if (enemy.agent.isOnNavMesh && !enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            enemy.agent.SetDestination(enemy.playerPosition.position);
        isAttack = false;
        isRun = true;
    }
    void Update()
    {
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (enemy.isAlive)
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

        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        enemy.animator.SetBool("isRunning", isRun);
        enemy.animator.SetBool("isAttacking", isAttack);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
