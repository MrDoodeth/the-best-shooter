using UnityEngine;

public class ExplosiveEnemyHealth : EnemyHealth
{
    private ExplosiveEnemyAttack exAttack;
    public override void TakeDamage(float damage)
    {
        if (isAlive)
            Die();
    }
    public override void Die()
    {
        base.Die();
        exAttack.ExploedWithDelay();
    }
    void Start()
    {
        exAttack = GetComponent<ExplosiveEnemyAttack>();
        Init();
    }
}
