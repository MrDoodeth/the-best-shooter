using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Fight")] 
        [SerializeField] private float sightRange = 1.5f;
        [SerializeField] protected float attackRange = 1.6f;
        [SerializeField] protected Transform attackPoint;
        [SerializeField] protected int damage = 33;
        [SerializeField] protected LayerMask whatIsPlayer;

        private Enemy enemy;
        //States
        private bool isRun;
        private bool isAttack;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            attackPoint ??= transform;
        }

        #region Attack
        private void AttackPlayer()
        {
            if (enemy.agent.isOnNavMesh)
                enemy.agent.SetDestination(transform.position);

            transform.LookAt(new Vector3(enemy.playerTransform.position.x, transform.position.y, enemy.playerTransform.position.z));

            isAttack = true;
            isRun = false;
        }

        private void Attack()
        {
            Collider[] playerHit = Physics.OverlapSphere(attackPoint.position, attackRange, whatIsPlayer);

            foreach (Collider hit in playerHit)
            {
                if (hit.CompareTag("Player"))
                    hit.GetComponent<IDamagable>()?.TakeDamage(damage);
            }
        }
        #endregion

        private void ChasePlayer()
        {
            if (enemy.agent.isOnNavMesh && !enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("ATTACK"))
                enemy.agent.SetDestination(enemy.playerTransform.position);
            isAttack = false;
            isRun = true;
        }

        private void Update()
        {
            if (!enemy.isAlive) return;
        
            bool playerInAttackRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

            if (playerInAttackRange)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
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
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, sightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
