using UnityEngine;
using TMPro;

public class DisplayAmmo : MonoBehaviour
{
    [SerializeField] private TMP_Text ammo;
    [SerializeField] private WeaponsSwitching switching;
    void Start()
    {
        ammo.enabled = false;
    }

    private void OnEnable()
    {
        PlayerController.OnTakenGun += OnTaken;
        PlayerController.OnTakenGun += DetermineAmmo;

        PlayerController.OnShootInput += DetermineAmmo;
        PlayerController.OnReloadEnd += DetermineAmmo;

        PlayerController.OnDropGun += OnDroped;
        PlayerController.OnChangedGun += DetermineAmmo;
    }

    private void OnDisable()
    {
        PlayerController.OnTakenGun -= OnTaken;
        PlayerController.OnTakenGun -= DetermineAmmo;

        PlayerController.OnShootInput -= DetermineAmmo;
        PlayerController.OnReloadEnd -= DetermineAmmo;

        PlayerController.OnDropGun -= OnDroped;
        PlayerController.OnChangedGun -= DetermineAmmo;
    }

    private void OnTaken() => ammo.enabled = true;

    private void OnDroped() => ammo.enabled = false;

    private void DetermineAmmo()
    {
        if (switching.gun is not null)
            ammo.text = $"{switching.gun.currentAmmo}/{switching.gun.reserveAmmo}";
    }
}
