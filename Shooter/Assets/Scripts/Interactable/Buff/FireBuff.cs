using Shooter.Enemy;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class FireBuff : Buff
    {
        [SerializeField, Range(1, 1000)] private int damage;
        [SerializeField, Range(0.01f, 20f)] private float duration;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                EnemiesSpawner.OnFireBuffActivate?.Invoke(duration, damage);
                print("---FIRE---");
                Destroy(gameObject);
            }
        }
    }
}
