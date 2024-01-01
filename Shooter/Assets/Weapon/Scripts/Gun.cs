using UnityEngine;
using ShooterInterfaces;
using System;
using Random = UnityEngine.Random;

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
    public bool reloading = false;
    [Space(10)]

    [Header("Gun Initialization")]
    [SerializeField] public Transform rightHandle;
    [SerializeField] public Transform leftHandle;
    [SerializeField] private Transform muzzle;
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private Animator animator;
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


        animator = GetComponent<Animator>();

        enabled = false;
    }

    private void OnEnable()
    {
        TurnOnControl();
        animator.enabled = false;
    }
    private void OnDisable()
    {
        TurnOffControl();
        CancelReload();
    }


    private void TurnOffControl()
    {
        if (automatic)
            PlayerController.OnShootInput -= Shoot;
        else
            PlayerController.OnShootInputDown -= Shoot;

        PlayerController.OnReloadInput -= StartReload;

        weaponZoom.enabled = false;
        weaponSway.enabled = false;
        weaponBobbing.enabled = false;
    }

    private void TurnOnControl()
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

    private bool CanReload() => !reloading && currentAmmo < magSize && reserveAmmo > 0 && this.gameObject.activeSelf;
    private void StartReload()
    {
        if (CanReload())
        {
            TurnOffControl();
            reloading = true;
            animator.enabled = true;

            animator.SetBool("isReloading", true);

        }
    }

    private void ReloadEnd() //animator
    {
        if (enabled == true)
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

            PlayerController.OnReloadEnd?.Invoke();

            reloading = false;
            animator.SetBool("isReloading", false);
            CancelReload();
            TurnOnControl();
        }
    }

    public void CancelReload()
    {
        animator.SetBool("isReloading", false);
    }

    private void AnimatorOff()
    {
        animator.enabled = false;
        reloading = false;
        //Debug.LogError("Stop");
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
                hitInfo.transform.GetComponentInParent<IDamageable>()?.TakeDamage(damage);
                //hitInfo.transform.GetComponent<IDamageable>()?.TakeDamage(damage);
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
