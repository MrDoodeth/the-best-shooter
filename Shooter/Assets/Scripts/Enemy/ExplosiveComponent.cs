using UnityEngine;

namespace Shooter.Enemy
{
    public class ExplosiveComponent : MonoBehaviour
    {
        [Header("Explosion Settings")]
        [Tooltip("Нужно помнить, что коллайдеров несколько => урон проходит по каждому отдельно")]
        [SerializeField, Range(1f, 100f)] private int closeExplosionDamage = 70;
        [Tooltip("Нужно помнить, что коллайдеров несколько => урон проходит по каждому отдельно")]
        [SerializeField, Range(1f, 100f)] private int farExplosionDamage = 30;
        [SerializeField] private float explosionDelay = 0.15f;
        [SerializeField] private float radius;
        [SerializeField] private float force;
        private bool endExplosion;
        public void ExploedWithDelay()
        {
            if (endExplosion)
                return;

            Invoke(nameof(Explode), explosionDelay);
        }

        private void Explode()
        {
            Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius);

            foreach (var col in overlappedColliders)
            {
                IDamagable entity = col.GetComponentInParent<IDamagable>();

                if (Vector3.Distance(transform.position, col.transform.position) <= radius / 2)
                    entity?.TakeDamage(closeExplosionDamage);
                else
                    entity?.TakeDamage(farExplosionDamage);

                //IContusionable enemy = col.GetComponentInParent<IContusionable>();
                //enemy?.GetContusion();

                //Regdoll отталкивание
                Rigidbody[] rbEntities = col.GetComponents<Rigidbody>();
                for (int i = 0; i < rbEntities.Length; i++)
                {
                    rbEntities[i].AddExplosionForce(force, transform.position, radius, 3f, ForceMode.Impulse);
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

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius / 2);
        }
    }
}
