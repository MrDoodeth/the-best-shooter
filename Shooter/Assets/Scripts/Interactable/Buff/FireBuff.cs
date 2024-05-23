using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class FireBuff : Buff
    {
        [SerializeField, Range(1, 1000)] private int damage;
        [SerializeField, Range(0.01f, 20f)] private float duration;
        [SerializeField] private Sprite icon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                PlayerBuffs.OnFireBuffActivated?.Invoke(BuffState.Fire, icon, duration, damage);
                print("---FIRE---");
                Destroy(gameObject);
            }
        }
    }
}
