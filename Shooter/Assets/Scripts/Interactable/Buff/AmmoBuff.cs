using Shooter.Weapon;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class AmmoBuff : Buff
    {
        [SerializeField, Range(1, 1000)] private int ammoAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (WeaponsSwitcher.currentWeapon is not null && WeaponsSwitcher.currentWeapon is Gun gun)
                {
                    gun.ReserveAmmo += ammoAmount;
                    print("===AmmoAdded===");
                    Destroy(gameObject);
                }
            }
        }
    }
}