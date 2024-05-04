using Shooter.Player;
using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(ExplosiveComponent), typeof(Enemy))]

    public class ExplosiveEnemyHealth : MonoBehaviour, IDamagable
    {
        private Enemy enemy;
        private ExplosiveComponent exComponent;
        void Start()
        {
            enemy = GetComponent<Enemy>();
            exComponent = GetComponent<ExplosiveComponent>();
        }
        public void TakeDamage(int damage)
        {
            if (enemy.isAlive)
                Die();
        }
        public void Die()
        {
            enemy.isAlive = false;
            EnemiesSpawner.OnEnemyDeath?.Invoke();
            PlayerScore.OnGetReward?.Invoke(enemy.rewardAmount);
            Debug.Log("Death");
            exComponent.ExploedWithDelay();
        }
    }
}
