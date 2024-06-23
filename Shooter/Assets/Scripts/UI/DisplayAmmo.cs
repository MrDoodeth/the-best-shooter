using Shooter.Player;
using Shooter.Weapon;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class DisplayAmmo : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoText;

        private void OnEnable()
        {
            PlayerController.OnWeaponTaken += DetermineAmmo;

            PlayerController.OnAttackInput += DetermineAmmo;
            Gun.OnReloadEnd += DetermineAmmo;
            Gun.OnBuffUse += DetermineAmmo;

            PlayerController.OnWeaponDroped += OnDroped;
            PlayerController.OnWeaponChanged += DetermineAmmo;
        }

        private void OnDisable()
        {
            PlayerController.OnWeaponTaken -= DetermineAmmo;

            PlayerController.OnAttackInput -= DetermineAmmo;
            Gun.OnReloadEnd -= DetermineAmmo;
            Gun.OnBuffUse -= DetermineAmmo;

            PlayerController.OnWeaponDroped -= OnDroped;
            PlayerController.OnWeaponChanged -= DetermineAmmo;
        }

        private void OnDroped() => ammoText.enabled = false;

        private void DetermineAmmo()
        {       
            if (WeaponsSwitcher.currentWeapon is Gun gun)
            {
                ammoText.enabled = true;
                if (WeaponsSwitcher.currentWeapon is not null)
                    ammoText.text = $"{gun.currentAmmo}/{gun.ReserveAmmo}";
            }
            else
            {
                ammoText.enabled = false;
            }

        }
    }
}
