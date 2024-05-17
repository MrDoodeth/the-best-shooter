using UnityEngine;

namespace Shooter.Enemy
{
    public class ToxicArea : MonoBehaviour
    {
        [Tooltip("hp/sec")]
        [HideInInspector] public float damageFrequency;
        [HideInInspector] public float duration;

        private SphereCollider sphereCollider;
        private float timeSinceLastDamage;
        
        public void Deactivate() => Destroy(gameObject, duration);

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (timeSinceLastDamage <= 0f)
                {
                    other.transform.GetComponent<IDamagable>()?.TakeDamage((int)damageFrequency);
                    timeSinceLastDamage = 1f;
                }
                else
                    timeSinceLastDamage -= Time.deltaTime;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            timeSinceLastDamage = 0f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        }
    }
}
