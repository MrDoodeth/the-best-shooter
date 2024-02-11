using UnityEngine;
[RequireComponent (typeof(Enemy))]

public class ExplosiveEnemyHealth : MonoBehaviour, IDamageable
{
    private Enemy enemy;
    private ExplosiveEnemyAttack exAttack;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        exAttack = GetComponent<ExplosiveEnemyAttack>();
    }
    public void TakeDamage(float damage)
    {
        if (enemy.isAlive)
            Die();
    }
    public void Die()
    {
        enemy.isAlive = false;
        EnemesSpawner.OnEnemyDeath?.Invoke();
        exAttack.ExploedWithDelay();
    }
}
