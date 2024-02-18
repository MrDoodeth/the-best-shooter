using UnityEngine;
[RequireComponent(typeof(ExplosiveComponent), typeof(Enemy))]

public class ExplosiveEnemyHealth : MonoBehaviour, IDamageable
{
    private Enemy enemy;
    private ExplosiveComponent exComponent;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        exComponent = GetComponent<ExplosiveComponent>();
    }
    public void TakeDamage(float damage)
    {
        if (enemy.isAlive)
            Die();
    }
    public void Die()
    {
        enemy.isAlive = false;
        EnemiesSpawner.OnEnemyDeath?.Invoke();
        EnemiesSpawner.OnGetReward?.Invoke(enemy.rewardAmount);
        Debug.Log("Death");
        exComponent.ExploedWithDelay();
    }
}
