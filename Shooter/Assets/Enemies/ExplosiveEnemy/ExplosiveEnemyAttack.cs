using ShooterInterfaces;
using UnityEngine;

public class ExplosiveEnemyAttack : EnemyAttack
{
    [Header("Explosion Settings")]
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField, Range(1f, 100f)] private int closeExplosionDamage = 70;
    [SerializeField, Range(1f, 100f)] private int farExplosionDamage = 30;
    private bool endExplosion;

    private ExplosiveEnemyHealth exHealth;
    public override void Attack()
    {
        if (exHealth.isAlive)
            exHealth.Die();
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
            IDamageable entity = col.GetComponentInParent<IDamageable>();

            if (entity != null)
            {
                if (Vector3.Distance(transform.position, col.transform.position) < radius / 2)
                    entity?.TakeDamage(closeExplosionDamage);
                else
                    entity?.TakeDamage(farExplosionDamage);
            }

            EnemyHealth enemy = col.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.GetContusion();
            }

            Rigidbody[] rb = col.GetComponents<Rigidbody>();
            for (int i = 0; i < rb.Length; i++)
            {
                rb[i].AddExplosionForce(force, transform.position, radius, 3f, ForceMode.Impulse);
            }
        }
        endExplosion = true;
        Debug.Log("Boom");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius / 2);
    }

    void Start()
    {
        exHealth = GetComponent<ExplosiveEnemyHealth>();
        Init();
    }
}
