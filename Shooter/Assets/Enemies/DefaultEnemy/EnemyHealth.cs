using ShooterInterfaces;
using System.Collections;
using UnityEngine;
public class EnemyHealth : Enemy, IDamageable
{

    [Header("Health")]
    [SerializeField][Range(1, 5000)] private float maxHealth = 100;
    protected float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Init();
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //agent.speed *= 0.8f;



        if (currentHealth <= 0)
        {
            Die();
            Destroy(gameObject);
        }
        else
        {
            OnGetDamage();
        }
    }
    public virtual void Die()
    {
        StartCoroutine(Death());
        IEnumerator Death()
        {
            EnemesSpawner.OnEnemyDied();
            mainEnemy.isAlive = false;
            EnemesSpawner.OnEnemyDeath?.Invoke();
            yield return null;
        }
    }

    public virtual void OnGetDamage()
    {
        StartCoroutine(Damage());

        IEnumerator Damage()
        {
            animator.SetTrigger("isHit");
            animator.SetBool("isMirror",Random.value > 0.5f);
            agent.enabled = false;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.26f);
            agent.enabled = true;
            yield return null;
        }
    }
    public void GetContusion() => StartCoroutine(Contusion());

    IEnumerator Contusion()
    {
        rb.isKinematic = false;
        mainEnemy.isAlive = false;
        agent.enabled = false;
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => onFloor == true);
        rb.isKinematic = true;
        mainEnemy.isAlive = true;
        agent.enabled = true;
        yield return null;
    }
}
