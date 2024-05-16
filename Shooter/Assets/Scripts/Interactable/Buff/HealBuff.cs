using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class HealBuff : MonoBehaviour
    {
        [SerializeField, Range(1, 1000)] private int healAmount;
        [SerializeField, Range(0.01f, 20f)] private float lifeTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                other.transform.GetComponent<PlayerHealth>().CurrentHealth += healAmount;
                print("+++Healing+++");
                Destroy();
            }
        }

        private void Destroy() => Destroy(gameObject);
        private void Start()
        {
            Invoke(nameof(Destroy), lifeTime);
        }
    }
}