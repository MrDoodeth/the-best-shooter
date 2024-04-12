using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

[RequireComponent(typeof(WeaponZoom), (typeof(WeaponSway)), typeof(WeaponBobbing))]
public sealed class Gun : Weapon
{
    [Space(10)]
    [SerializeField] private GameObject bulletHole;

    [Header("Shooting")]
    [SerializeField] private bool automatic;
    [SerializeField] private float maxDistance;
    private float timeSinceLastShoot;

    [Header("Recoil")]
    [SerializeField] private float powerOfVerticalRecoil;
    [SerializeField] private float powerOfHorizontalRecoil;
    [SerializeField] private float powerOfForwardRecoil = 0.1f;
    [SerializeField] private float powerOfBoltRecoil = 0.1f;
    [SerializeField] private float durationOfBoltRecoil = 0.1f;

    [SerializeField][Range(0.01f, 5f)] private float cameraRecoilRatio = 1f;
    [Space(10)]

    [Header("Reloading")]
    [SerializeField] private int maxReserveAmmo;
    [SerializeField] private int magSize;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    public int currentAmmo { get; private set; }
    public int reserveAmmo { get; private set; }
    public static event Action OnReloadEnd;
    [Space(10)]

    [Header("Gun Initialization")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform bolt;   

    private void Awake()
    {
        currentAmmo = magSize;
        reserveAmmo = maxReserveAmmo;

        Initialization();
        //Важно в конце!!!
        enabled = false;
    }

    private void OnEnable()
    {
        if (automatic)
            PlayerController.OnAttackInput += Shoot;
        else
            PlayerController.OnAttackInputDown += Shoot;

        PlayerController.OnReloadInput += StartReload;

        TurnOnControl();
        animator.enabled = false;
    }
    private void OnDisable()
    {
        if (automatic)
            PlayerController.OnAttackInput -= Shoot;
        else
            PlayerController.OnAttackInputDown -= Shoot;

        PlayerController.OnReloadInput -= StartReload;

        TurnOffControl();        
    }
    private void StartReload()
    {
        bool CanReload() => currentAmmo < magSize && reserveAmmo > 0 && this.gameObject.activeSelf;

        if (CanReload())
        {
            PlayerController.isControllerAvailable = false;
            animator.enabled = true;

            animator.SetBool("isReloading", true);
        }
    }
    #region ForAnimations
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

            animator.SetBool("isReloading", false);
        }
    }
    
    private void AnimatorOff()
    {
        animator.enabled = false;
        PlayerController.isControllerAvailable = true;
        OnReloadEnd?.Invoke();
    }

    #endregion
    private void Shoot()
    {
        bool CanShoot() => currentAmmo > 0 && timeSinceLastShoot > 1f / (fireRate / 60f);

        if (CanShoot())
        {
            currentAmmo--;
            timeSinceLastShoot = 0;

            if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, maxDistance))
            {
                var entity = hitInfo.transform.GetComponentInParent<IDamagable>();
                entity?.TakeDamage(damage);
                var hole = Instantiate(bulletHole, hitInfo.point, Quaternion.identity);
                hole.transform.SetParent(hitInfo.transform);
                if (hitInfo.transform.TryGetComponent<Rigidbody>(out var rb))
                    rb.AddForceAtPosition(muzzle.forward * powerOfHit, hitInfo.point, ForceMode.Impulse);

                OnGunShot();
            }

            Recoil();

            void Recoil()
            {
                transform.localPosition -= Vector3.forward * powerOfForwardRecoil;
                var fPos = bolt.localPosition;
                DOTween.Sequence()
                    .Append(bolt.DOLocalMove(bolt.localPosition - (Vector3.forward * powerOfBoltRecoil), durationOfBoltRecoil))
                    .Append(bolt.DOLocalMove(fPos, durationOfBoltRecoil));

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

        Debug.DrawRay(muzzle.position, muzzle.forward, Color.red);
    }
}
