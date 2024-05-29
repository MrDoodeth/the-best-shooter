using Shooter.Player;
using Shooter.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class DisplaySniperAim : MonoBehaviour
    {
        private Image aimImage;
        private void Awake()
        {
            aimImage = GetComponent<Image>();
        }
        private void OnEnable()
        {
            PlayerController.OnZoomInput += DisplayAimImage;
        }
        private void OnDisable()
        {
            PlayerController.OnZoomInput -= DisplayAimImage;
        }
        private void DisplayAimImage(bool isZoom)
        {
            if (WeaponsSwitcher.currentWeapon is not null && WeaponsSwitcher.currentWeapon is Gun { typeOfGun: TypeOfGun.SniperRifle } && !WeaponsSwitcher.FPC.isSprinting)
                aimImage.enabled = isZoom;
            else
                aimImage.enabled = false;
        }
    }
}
