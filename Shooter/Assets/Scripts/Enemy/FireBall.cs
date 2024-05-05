using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class FireBall : MonoBehaviour
    {
        [HideInInspector] public int damage;
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player")) 
                other.transform.GetComponent<IDamagable>()?.TakeDamage(damage);

            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.SetActive(false);
        }
    }
}
