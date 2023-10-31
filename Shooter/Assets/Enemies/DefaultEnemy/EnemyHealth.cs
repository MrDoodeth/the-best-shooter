using ShooterInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        agent.speed *= 0.8f;



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
    public virtual async void Die()
    {
        await Task.Run(() => EnemesSpawner.OnEnemyDied());
        await Task.Delay(1);
        isAlive = false;
        EnemesSpawner.OnEnemyDeath?.Invoke();
    }

    public virtual void OnGetDamage()
    {
        StartCoroutine(Damage());

        IEnumerator Damage()
        {
            var Render = GetComponent<MeshRenderer>();
            Color col = Render.material.color;
            Render.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            Render.material.color = col;
        }
    }
    public void GetContusion() => StartCoroutine(Contusion());

    IEnumerator Contusion()
    {
        rb.isKinematic = false;
        isAlive = false;
        agent.enabled = false;
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => onFloor == true);
        rb.isKinematic = true;
        isAlive = true;
        agent.enabled = true;
        yield return null;
    }
}
