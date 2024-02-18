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

    private void OnTaken() => ammoText.enabled = true;

    private void OnDroped() => ammoText.enabled = false;

    private void DetermineAmmo()
    {
        if (switching.gun is not null)
            ammoText.text = $"{switching.gun.currentAmmo}/{switching.gun.reserveAmmo}";
    }
}
