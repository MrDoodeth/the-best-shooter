using System;
using NaughtyAttributes;
using Shooter.Player;
using UnityEngine;
using Random = UnityEngine.Random;
using Shooter.Enemy;

namespace Shooter.Weapon
{
    [RequireComponent(typeof(WeaponZoom), (typeof(WeaponSwayAndBobbing)))]
    public sealed class Gun : Weapon
    {
        [Space(10)] 
        [SerializeField] private GameObject bulletHole;

        [Header("Shooting")]
        public TypeOfGun typeOfGun;
        [SerializeField] private bool automatic;
        [SerializeField] private float maxDistance;
        [ShowIf("isShotgun")] [SerializeField] [Range(0.01f, 85f)] private float maxSprayRange;
        [ShowIf("isShotgun")] [SerializeField] private int bulletAmount = 10;
        private bool isShotgun() => typeOfGun == TypeOfGun.Shotgun;
        
        
        [Header("Recoil")]
        [SerializeField] private Vector3 defaultCameraRecoil;
        [SerializeField] private Vector3 zoomCameraRecoil;

        [SerializeField] private Vector3 gunRecoil;

        [Space(10)]
        [Header("Reloading")]
        [SerializeField] private int maxReserveAmmo;

        [SerializeField] private int magSize;
        [SerializeField] private float fireRate;
        private float timeSinceLastShoot;
        public bool isReloading { get; private set; }
        public int currentAmmo { get; private set; }
        private int reserveAmmo;

        public int ReserveAmmo
        {
            get => reserveAmmo;
            set
            {
                reserveAmmo = Math.Clamp(value, 0, maxReserveAmmo);
                OnBuffUse?.Invoke();
            }
        }

        public static event Action OnReloadEnd;
        public static event Action OnBuffUse; 

        [Space(10)] [Header("Gun Initialization")] [SerializeField]
        private Transform muzzle;

        private void Awake()
        {
            currentAmmo = magSize;
            ReserveAmmo = maxReserveAmmo;

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
            if (!CanReload()) return;
            
            PlayerController.isControllerAvailable = false;
            animator.enabled = true;
            animator.SetBool("isReloading", true);
            isReloading = true;
            return;

            bool CanReload() => currentAmmo < magSize && ReserveAmmo > 0 && this.gameObject.activeSelf && !weaponZoom.isZoom;
        }

        #region ForAnimations

        private void ReloadEnd() //animator
        {
            if (enabled != true) return;
            
            int neededAmmo = magSize - currentAmmo;
            
            if (neededAmmo >= ReserveAmmo)
            {
                currentAmmo += ReserveAmmo;
                ReserveAmmo = 0;
            }
            else
            {
                currentAmmo = magSize;
                ReserveAmmo -= neededAmmo;
            }

            animator.SetBool("isReloading", false);
            isReloading = false;
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
            if (CanShoot())
            {
                currentAmmo--;
                timeSinceLastShoot = 0;
                
                animator.enabled = true;
                animator.SetTrigger("isShooting");

                switch (typeOfGun)
                {
                    case TypeOfGun.Default: Damage(muzzle.position);
                        break;
                    case TypeOfGun.Shotgun:
                    {
                        for (int bulletCounter = bulletAmount; bulletCounter > 0; bulletCounter--)
                        {
                            Quaternion defaultLocalRotation = muzzle.localRotation;
                            muzzle.localRotation = Quaternion.Euler(
                                muzzle.localRotation.x + Random.Range(-maxSprayRange, maxSprayRange),
                                muzzle.localRotation.y + Random.Range(-maxSprayRange, maxSprayRange),
                                muzzle.localRotation.z);

                            Damage(muzzle.position);

                            muzzle.localRotation = defaultLocalRotation;
                        }
                        break;
                    }
                    case TypeOfGun.SniperRifle:
                        if (weaponZoom.isZoom)
                            Damage(WeaponsSwitcher.FPC.playerCamera.transform.position);
                        else
                            Damage(muzzle.position);
                        break;
                }
                
                if (weaponZoom.isZoom)
                    CameraRecoil.Recoil(zoomCameraRecoil);
                else
                    CameraRecoil.Recoil(defaultCameraRecoil);
                
                Recoil();
            }

            return;

            bool CanShoot() => currentAmmo > 0 && timeSinceLastShoot > 1f / (fireRate / 60f);
            
            void Recoil()
            {
                transform.localPosition -= Vector3.forward * gunRecoil.z;
                
                transform.localEulerAngles -= Vector3.right * gunRecoil.y;
                transform.localEulerAngles -= Vector3.up * Random.Range(-gunRecoil.x, gunRecoil.x);
            }
            
            void Damage(Vector3 origin)
            {
                if (Physics.Raycast(origin, muzzle.forward, out RaycastHit hitInfo, maxDistance, enemiesLayer, QueryTriggerInteraction.Ignore))
                {
                    var entity = hitInfo.transform.GetComponentInParent<IDamagable>();
                    entity?.TakeDamage(PlayerBuffs.CalculateDamage(damage));
                    var hole = Instantiate(bulletHole, hitInfo.point, Quaternion.identity);
                    hole.transform.SetParent(hitInfo.transform);
                    if (hitInfo.transform.TryGetComponent<Rigidbody>(out var rb))
                        rb.AddForceAtPosition(muzzle.forward * powerOfHit, hitInfo.point, ForceMode.Impulse);
                    Debug.DrawLine(muzzle.position, hitInfo.point, Color.green, 1f);
                }
                else
                {
                    Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * maxDistance, Color.red, 1f);
                }
            }
        }

        private void Update()
        {
            timeSinceLastShoot += Time.deltaTime;

            Debug.DrawRay(muzzle.position, muzzle.forward, Color.red);
        }
    }
    
    public enum TypeOfGun
    {
        Default,
        Shotgun,
        SniperRifle
    }
}