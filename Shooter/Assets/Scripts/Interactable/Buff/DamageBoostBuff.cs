using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class DamageBoostBuff : Buff
    {
        [SerializeField, Range(1, 50)] private int damageRatio;
        [SerializeField, Range(0.01f, 20f)] private float duration;
        [SerializeField] private Sprite icon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                PlayerBuffs.OnDamageBoostBuffActivated?.Invoke(BuffState.DamageBoost, icon, duration, damageRatio);
                print("===DAMAGE_BOOST===");
                Destroy(gameObject);
            }
        }
    }
}
