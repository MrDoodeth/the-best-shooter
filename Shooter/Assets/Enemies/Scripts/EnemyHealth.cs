using ShooterInterfaces;
using System.Collections;
using UnityEngine;
public class EnemyHealth : Enemy, IDamageable
{

    [Header("Health")]
    [SerializeField][Range(1, 5000)] private float maxHealth = 100;
    [SerializeField][Range(1, 30)] private float contusionDelay = 3f;
    [SerializeField][Range(1, 30)] private float deathDelay = 3f;
    [HideInInspector] public bool isAlive = true;
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
            isAlive = false;
            EnemesSpawner.OnEnemyDeath?.Invoke();
            isAlive = false;
            agent.enabled = false;
            MakeKinematic(false);
            animator.enabled = false;
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
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
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
            agent.enabled = true;
        }
    }
    public void GetContusion() => StartCoroutine(Contusion());

    IEnumerator Contusion()
    {
        isAlive = false;
        agent.enabled = false;
        MakeKinematic(false);
        animator.enabled = false;
        yield return new WaitForSeconds(contusionDelay);
        yield return new WaitUntil(() => agent.isOnNavMesh);
        { 
            //return rigs in world position
            transform.position = collid.transform.position;
            collid.transform.localPosition = Vector3.zero;
        }
        animator.enabled = true;
        animator.SetTrigger("isStandUp");
        MakeKinematic(true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        agent.enabled = true;
        isAlive = true;
    }
}
