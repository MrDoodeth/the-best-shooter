using UnityEngine;
using TMPro;

public class DisplayAmmo : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private WeaponsSwitching switching;
    void Start()
    {
        ammoText.enabled = false;
    }

    private void OnEnable()
    {
        PlayerController.OnGunTaken += OnTaken;
        PlayerController.OnGunTaken += DetermineAmmo;

        PlayerController.OnShootInput += DetermineAmmo;
        Gun.OnReloadEnd += DetermineAmmo;

        PlayerController.OnGunDroped += OnDroped;
        PlayerController.OnGunChanged += DetermineAmmo;
    }

    private void OnDisable()
    {
        PlayerController.OnGunTaken -= OnTaken;
        PlayerController.OnGunTaken -= DetermineAmmo;

        PlayerController.OnShootInput -= DetermineAmmo;
        Gun.OnReloadEnd -= DetermineAmmo;

        PlayerController.OnGunDroped -= OnDroped;
        PlayerController.OnGunChanged -= DetermineAmmo;
    }

    private void OnTaken() => ammoText.enabled = true;

    private void OnDroped() => ammoText.enabled = false;

    private void DetermineAmmo()
    {
        if (switching.currentGun != null)
            ammoText.text = $"{switching.currentGun.currentAmmo}/{switching.currentGun.reserveAmmo}";
    }
}
