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
        PlayerController.OnWeaponTaken += DetermineAmmo;

        PlayerController.OnAttackInput += DetermineAmmo;
        Gun.OnReloadEnd += DetermineAmmo;

        PlayerController.OnWeaponDroped += OnDroped;
        PlayerController.OnWeaponChanged += DetermineAmmo;
    }

    private void OnDisable()
    {
        PlayerController.OnWeaponTaken -= DetermineAmmo;

        PlayerController.OnAttackInput -= DetermineAmmo;
        Gun.OnReloadEnd -= DetermineAmmo;

        PlayerController.OnWeaponDroped -= OnDroped;
        PlayerController.OnWeaponChanged -= DetermineAmmo;
    }

    private void OnDroped() => ammoText.enabled = false;

    private void DetermineAmmo()
    {
        var currentWeapon = WeaponsSwitcher.currentWeapon as Gun;
        ammoText.enabled = true;
        if (WeaponsSwitcher.currentWeapon != null)
            ammoText.text = $"{currentWeapon.currentAmmo}/{currentWeapon.reserveAmmo}";
    }
}
