using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(ExplosiveComponent), typeof(Enemy))]
    public class ExplosiveEnemyAttack : MonoBehaviour
    {
        private Enemy enemy;
        private ExplosiveEnemyHealth exHealth;
        private bool isRun;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            exHealth = GetComponent<ExplosiveEnemyHealth>();
        }
        public void Attack()
        {
            if (enemy.isAlive)
                exHealth.Die();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
                Attack();
        }
        private void ChasePlayer()
        {
            if (enemy.agent.isOnNavMesh)
                enemy.agent.SetDestination(enemy.playerTransform.position);
            isRun = true;
        }

        private void Update()
        {
            ChasePlayer();

            UpdateAnimations();
        }
        private void UpdateAnimations()
        {
            enemy.animator.SetBool("isRunning", isRun);
        }
    }
}
