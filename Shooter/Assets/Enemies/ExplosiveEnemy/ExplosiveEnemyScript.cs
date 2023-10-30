using ShooterInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExplosiveEnemyScript : DefaultEnemyScript
{
    [Header("Explosion Settings")]
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField, Range(1f, 100f)] private int closeExplosionDamage = 70;
    [SerializeField, Range(1f, 100f)] private int farExplosionDamage = 30;
    private bool endExplosion;



    public override void Attack()
    {
        if (isAlive)
            Die();
    }
    public override void TakeDamage(float damage)
    {
        if (isAlive)
            Die();
    }

    public override async void Die()
    {
        await Task.Run(() => EnemesSpawner.OnEnemyDied());
        await Task.Delay(1);
        Debug.Log("Boom");
        isAlive = false;
        EnemesSpawner.OnEnemyDeath?.Invoke();
        ExploedWithDelay();
    }
    public void ExploedWithDelay()
    {
        if (endExplosion)
            return;

        Invoke(nameof(Explode), 0.15f);
    }
    private void Explode()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var col in overlappedColliders)
        {
            Rigidbody rb = col.attachedRigidbody;

            if (rb != null)
            {
                IDamageable entity = rb?.GetComponent<IDamageable>();


                if (Vector3.Distance(transform.position, rb.transform.position) < radius / 2)
                    entity?.TakeDamage(closeExplosionDamage);
                else
                    entity?.TakeDamage(farExplosionDamage);

                if (rb.CompareTag("Enemy"))
                    rb.GetComponent<DefaultEnemyScript>().GetContusion();

                rb.AddExplosionForce(force, transform.position, radius, 3f, ForceMode.Impulse);
            }
        }
        endExplosion = true;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius / 2);
    }
}
