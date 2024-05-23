using System;
using DG.Tweening;
using Shooter.Player;
using UnityEngine;
using Random = UnityEngine.Random;
using Shooter.Enemy;

namespace Shooter.Weapon
{
    [RequireComponent(typeof(WeaponZoom), (typeof(WeaponSwayAndBobbing)))]
    public sealed class Gun : Weapon
    {
        [Space(10)] [SerializeField] private GameObject bulletHole;

        [Header("Shooting")] [SerializeField] private bool automatic;
        [SerializeField] private float maxDistance;

        [SerializeField] private bool isShotgun = false;

        [Tooltip("Использовать только если isShotgun = true")] [SerializeField] [Range(0.01f, 85f)]
        private float maxSprayRange;

        [Tooltip("Использовать только если isShotgun = true")]
        [SerializeField] private int bulletAmount = 10;

        private float timeSinceLastShoot;

        [Header("Recoil")] [SerializeField] private float powerOfVerticalRecoil;
        [SerializeField] private float powerOfHorizontalRecoil;
        [SerializeField] private float powerOfForwardRecoil = 0.1f;
        [SerializeField] private float powerOfBoltRecoil = 0.1f;
        [SerializeField] private float durationOfBoltRecoil = 0.1f;

        [SerializeField] [Range(0.01f, 5f)] private float cameraRecoilRatio = 1f;

        [Space(10)] [Header("Reloading")] [SerializeField]
        private int maxReserveAmmo;

        [SerializeField] private int magSize;
        [SerializeField] private float fireRate;
        [SerializeField] private float reloadTime;
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

        [SerializeField] private Transform bolt;

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
            return;

            bool CanReload() => currentAmmo < magSize && ReserveAmmo > 0 && this.gameObject.activeSelf;
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

                if (!isShotgun)
                {
                    if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, maxDistance, enemiesLayer, QueryTriggerInteraction.Ignore))
                    {
                        Damage(hitInfo);
                        Debug.DrawLine(muzzle.position, hitInfo.point, Color.green, 1f);
                    }
                    else
                    {
                        Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * maxDistance, Color.red, 1f);
                    }
                }
                else if (isShotgun)
                {
                    for (int bulletCounter = bulletAmount; bulletCounter > 0; bulletCounter--)
                    {
                        Quaternion defaultLocalRotation = muzzle.localRotation;
                        muzzle.localRotation = Quaternion.Euler(
                            muzzle.localRotation.x + Random.Range(-maxSprayRange, maxSprayRange),
                            muzzle.localRotation.y + Random.Range(-maxSprayRange, maxSprayRange),
                            muzzle.localRotation.z);

                        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, maxDistance, enemiesLayer, QueryTriggerInteraction.Ignore))
                        {
                            Damage(hitInfo);
                            Debug.DrawLine(muzzle.position, hitInfo.point, Color.green, 1f);
                        }
                        else
                        {
                            Debug.DrawLine(muzzle.position, muzzle.position + muzzle.forward * maxDistance, Color.red, 1f);
                        }

                        muzzle.localRotation = defaultLocalRotation;
                    }
                }
                Recoil();
            }

            return;

            bool CanShoot() => currentAmmo > 0 && timeSinceLastShoot > 1f / (fireRate / 60f);
            
            void Recoil()
            {
                transform.localPosition -= Vector3.forward * powerOfForwardRecoil;
                var fPos = bolt.localPosition;
                DOTween.Sequence()
                    .Append(bolt.DOLocalMove(bolt.localPosition - (Vector3.forward * powerOfBoltRecoil),
                        durationOfBoltRecoil))
                    .Append(bolt.DOLocalMove(fPos, durationOfBoltRecoil));

                transform.localEulerAngles -= Vector3.right * powerOfVerticalRecoil;
                transform.localEulerAngles -=
                    Vector3.up * Random.Range(-powerOfHorizontalRecoil, powerOfHorizontalRecoil);

                fpc.pitch += -powerOfVerticalRecoil * cameraRecoilRatio;

                fpc.transform.localEulerAngles = new Vector3(0,
                    fpc.transform.localEulerAngles.y +
                    Random.Range(-powerOfHorizontalRecoil, powerOfHorizontalRecoil) / 2, 0);
            }
            
            void Damage(RaycastHit hitInfo)
            {
                var entity = hitInfo.transform.GetComponentInParent<IDamagable>();
                entity?.TakeDamage(damage);
                var hole = Instantiate(bulletHole, hitInfo.point, Quaternion.identity);
                hole.transform.SetParent(hitInfo.transform);
                if (hitInfo.transform.TryGetComponent<Rigidbody>(out var rb))
                    rb.AddForceAtPosition(muzzle.forward * powerOfHit, hitInfo.point, ForceMode.Impulse);
            }
        }

        private void Update()
        {
            timeSinceLastShoot += Time.deltaTime;

            Debug.DrawRay(muzzle.position, muzzle.forward, Color.red);
        }
    }
}