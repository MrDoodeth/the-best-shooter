using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShooterInterfaces;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(WeaponZoom), (typeof(WeaponSway)), typeof(WeaponBobbing))]
public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletHole;

    [Header("Info")]
    public string Name;
    public int Size;
    [Space(10)]

    [Header("Shooting")]
    [SerializeField] private bool automatic;
    [SerializeField] private int damage;
    [SerializeField] private float maxDistance;
    [Space(10)]

    [Header("Recoil")]
    [SerializeField] private float powerOfVerticalRecoil;
    [SerializeField] private float powerOfHorizontalRecoil;
    [SerializeField] private float powerOfForwardRecoil = 0.1f;
    [SerializeField][Range(0.01f, 5f)] private float cameraRecoilRatio = 1f;
    [Space(10)]

    [Header("Reloading")]
    public int currentAmmo;
    public int reserveAmmo;
    [SerializeField] private int maxReserveAmmo;
    [SerializeField] private int magSize;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool reloading = false;
    [Space(10)]

    [Header("Gun Initialization")]
    [SerializeField] public Transform rightHandle;
    [SerializeField] public Transform leftHandle;
    [SerializeField] private Transform muzzle;
    [SerializeField] private FirstPersonController fpc;
    private float timeSinceLastShoot;
    private Transform fireDirection;

    [HideInInspector] public WeaponZoom weaponZoom;
    [HideInInspector] public WeaponSway weaponSway;
    [HideInInspector] public WeaponBobbing weaponBobbing;


    public void SetFPC(FirstPersonController fpc)
    {
        if (fpc == null)
        {
            this.fpc = null;
            weaponZoom.fpc = null;
            return;
        }

        this.fpc = fpc;
        weaponZoom.fpc = fpc;
    }

    private void Awake()
    {
        currentAmmo = magSize;
        reserveAmmo = maxReserveAmmo;
        reloading = false;
        fireDirection = muzzle;

        weaponZoom = GetComponent<WeaponZoom>();
        weaponSway = GetComponent<WeaponSway>();
        weaponBobbing = GetComponent<WeaponBobbing>();

        enabled = false;
    }

    private void OnEnable()
    {
        if (automatic)
            PlayerController.OnShootInput += Shoot;
        else
            PlayerController.OnShootInputDown += Shoot;

        PlayerController.OnReloadInput += StartReload;

        weaponZoom.enabled = true;
        weaponSway.enabled = true;
        weaponBobbing.enabled = true;
    }
    private void OnDisable()
    {
        if (automatic)
            PlayerController.OnShootInput -= Shoot;
        else
            PlayerController.OnShootInputDown -= Shoot;

        PlayerController.OnReloadInput -= StartReload;
        reloading = false;

        weaponZoom.enabled = false;
        weaponSway.enabled = false;
        weaponBobbing.enabled = false;
    }



    private bool CanReload() => !reloading && currentAmmo < magSize && reserveAmmo > 0 && this.gameObject.activeSelf;
    private void StartReload()
    {
        if (CanReload())
        {
            StartCoroutine(Reload());
        }

        IEnumerator Reload()
        {
            reloading = true;
            yield return new WaitForSeconds(reloadTime);


            if (enabled == true)
            {
                ReloadEnd();
            }

            PlayerController.OnReloadEnd?.Invoke();
            reloading = false;
        }
    }

    private void ReloadEnd()
    {
        int neededAmmo = magSize - currentAmmo;
        if (neededAmmo >= reserveAmmo)
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }
        else
        {
            currentAmmo = magSize;
            reserveAmmo -= neededAmmo;
        }
    }

    private bool CanShoot() => !reloading && currentAmmo > 0 && timeSinceLastShoot > 1f / (fireRate / 60f);
    private void Shoot()
    {
        if (CanShoot())
        {
            currentAmmo--;
            timeSinceLastShoot = 0;


            if (Physics.Raycast(fireDirection.position, fireDirection.forward, out RaycastHit hitInfo, maxDistance))
            {
                hitInfo.transform.GetComponent<IDamageable>()?.TakeDamage(damage);
                var hole = Instantiate(bulletHole, hitInfo.point, Quaternion.identity);
                hole.transform.SetParent(hitInfo.transform);            
                OnGunShot();
            }

            Recoil();

            void Recoil()
            {
                transform.localPosition -= Vector3.forward * powerOfForwardRecoil;

                transform.localEulerAngles -= Vector3.right * powerOfVerticalRecoil;
                transform.localEulerAngles -= Vector3.up * Random.Range(-powerOfHorizontalRecoil, powerOfHorizontalRecoil);

                fpc.pitch += -powerOfVerticalRecoil * cameraRecoilRatio;

                fpc.transform.localEulerAngles = new Vector3(0, fpc.transform.localEulerAngles.y + Random.Range(-powerOfHorizontalRecoil, powerOfHorizontalRecoil) / 2, 0);
            }

            void OnGunShot()
            {
                
            }
        }
    }



    private void Update()
    {
        timeSinceLastShoot += Time.deltaTime;

        Debug.DrawRay(fireDirection.position, fireDirection.forward, Color.red);
    }
}
