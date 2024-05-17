using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class HealBuff : Buff
    {
        [SerializeField, Range(1, 1000)] private int healAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                other.transform.GetComponent<PlayerHealth>().CurrentHealth += healAmount;
                print("+++Healing+++");
                Destroy(gameObject);
            }
        }
    }
}