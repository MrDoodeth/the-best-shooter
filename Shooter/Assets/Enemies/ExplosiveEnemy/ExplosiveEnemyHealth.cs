using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Debug.Log("Boom");
        exAttack.ExploedWithDelay();
    }
    void Start()
    {
        exAttack = GetComponent<ExplosiveEnemyAttack>();
        Init();
    }

    void Update()
    {
        
    }
}
