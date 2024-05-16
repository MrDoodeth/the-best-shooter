using Shooter.Weapon;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class AmmoBuff : MonoBehaviour
    {
        [SerializeField, Range(1, 1000)] private int ammoAmount;
        [SerializeField, Range(0.01f, 20f)] private float lifeTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (WeaponsSwitcher.currentWeapon is not null && WeaponsSwitcher.currentWeapon is Gun gun)
                {
                    gun.ReserveAmmo += ammoAmount;
                    print("===AmmoAdded===");
                    Destroy();
                }
            }
        }

        private void Destroy() => Destroy(gameObject);
        private void Start()
        {
            Invoke(nameof(Destroy), lifeTime);
        }
    }
}