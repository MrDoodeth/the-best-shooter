using UnityEngine;
using TMPro;

public class DisplayAmmo : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    void Start()
    {
        ammoText.enabled = false;
    }

    private void OnEnable()
    {
        PlayerController.OnWeaponTaken += OnTaken;
        PlayerController.OnWeaponTaken += DetermineAmmo;

        PlayerController.OnAttackInput += DetermineAmmo;
        Gun.OnReloadEnd += DetermineAmmo;

        PlayerController.OnWeaponDroped += OnDroped;
        PlayerController.OnWeaponChanged += DetermineAmmo;
    }

    private void OnDisable()
    {
        PlayerController.OnWeaponTaken -= OnTaken;
        PlayerController.OnWeaponTaken -= DetermineAmmo;

        PlayerController.OnAttackInput -= DetermineAmmo;
        Gun.OnReloadEnd -= DetermineAmmo;

        PlayerController.OnWeaponDroped -= OnDroped;
        PlayerController.OnWeaponChanged -= DetermineAmmo;
    }

    private void OnTaken() => ammoText.enabled = true;

    private void OnDroped() => ammoText.enabled = false;

    private void DetermineAmmo()
    {
        Gun currentGun = WeaponsSwitcher.currentWeapon as Gun;
        if (WeaponsSwitcher.currentWeapon != null)
            ammoText.text = $"{currentGun.currentAmmo}/{currentGun.reserveAmmo}";
    }
}
