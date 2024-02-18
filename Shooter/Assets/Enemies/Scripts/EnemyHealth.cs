using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour, IDamageable, IContusionable
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        if (!enemy.isAlive) return;

        enemy.currentHealth -= damage;

        //agent.speed *= 0.8f;

        if (enemy.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnGetDamage();
        }
    }
    public void Die()
    {
        enemy.isAlive = false;
        EnemiesSpawner.OnEnemyDeath?.Invoke();
        EnemiesSpawner.OnGetReward?.Invoke(enemy.rewardAmount);
        enemy.agent.enabled = false;
        enemy.allRigidbodys.SetIsKinematics(false);
        enemy.animator.enabled = false;

        StartCoroutine(Death());

        IEnumerator Death()
        {           
            yield return new WaitForSeconds(enemy.deathDelay);
            Destroy(gameObject);         
        }
    }

    private void OnGetDamage()
    {
        StartCoroutine(Damage());

        IEnumerator Damage()
        {
            enemy.agent.enabled = false;
            enemy.animator.SetTrigger("isHit");
            enemy.animator.SetBool("isMirror",Random.value > 0.5f);
            yield return new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length + 0.8f);
            enemy.agent.enabled = true;
        }
    }
    public void GetContusion() => StartCoroutine(Contusion());

    IEnumerator Contusion()
    {
        enemy.isAlive = false;
        enemy.agent.enabled = false;
        enemy.allRigidbodys.SetIsKinematics(false);
        enemy.animator.enabled = false;
        yield return new WaitForSeconds(enemy.contusionDelay);
        yield return new WaitUntil(() => enemy.agent.isOnNavMesh);
        { 
            //return rigs in world position
            transform.position = enemy.allColliders[0].transform.position;
            enemy.allColliders[0].transform.localPosition = Vector3.zero;
        }
        enemy.animator.enabled = true;
        enemy.animator.SetTrigger("isStandUp");
        enemy.allRigidbodys.SetIsKinematics(true);
        yield return new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        enemy.agent.enabled = true;
        enemy.isAlive = true;
        yield return null;
    }
}
