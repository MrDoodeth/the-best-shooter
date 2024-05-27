using Shooter.Player;
using UnityEngine;

namespace Shooter.Weapon
{
    [RequireComponent(typeof(Animator))]
    public abstract class Weapon : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] protected int damage;
        [field:SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public int Size { get; protected set; }
        [SerializeField][Min(0.01f)] protected float powerOfHit;
        [SerializeField][Min(0.01f)] private float powerOfDrop = 4f;

        [SerializeField] public Transform rightHandle;
        [SerializeField] public Transform leftHandle;
        [SerializeField] protected LayerMask enemiesLayer;
        protected FirstPersonController fpc;
        protected Animator animator;

        [HideInInspector] public WeaponZoom weaponZoom;
        [HideInInspector] public WeaponSwayAndBobbing weaponSwayAndBobbing;
        [HideInInspector] public Rigidbody weaponRigidbody;
        [HideInInspector] public BoxCollider weaponCollider;

        protected void Initialization()
        {
            weaponZoom = GetComponent<WeaponZoom>();
            weaponSwayAndBobbing = GetComponent<WeaponSwayAndBobbing>();
            weaponRigidbody = GetComponent<Rigidbody>();
            weaponCollider = GetComponent<BoxCollider>();
            animator = GetComponent<Animator>();
        }

        private void SetFPC(FirstPersonController fpc)
        {
            this.fpc = fpc;
            weaponZoom.fpc = fpc;
            weaponSwayAndBobbing.mover = this.fpc;
        }
        protected void TurnOnControl()
        {
            weaponZoom.enabled = true;
            weaponSwayAndBobbing.enabled = true;
        }
        protected void TurnOffControl()
        {
            weaponZoom.enabled = false;
            weaponSwayAndBobbing.enabled = false;
        }
        public void OnPickedUp(Transform weaponPivot, Transform weaponsSwitcher, FirstPersonController FPC)
        {       
            transform.SetPositionAndRotation(weaponPivot.position, weaponPivot.rotation);
            transform.SetParent(weaponsSwitcher);
            SetFPC(FPC);
            weaponRigidbody.isKinematic = true;
            weaponCollider.enabled = false;
            PlayerController.OnWeaponTaken?.Invoke();             
            enabled = true;
        }

        public void Droped()
        {
            weaponRigidbody.isKinematic = false;
            weaponCollider.enabled = true;
            enabled = false;
            SetFPC(null);
            weaponRigidbody.AddForce(transform.forward * powerOfDrop + transform.up * (powerOfDrop / 2), ForceMode.Impulse);
            transform.SetParent(null);
        }
    }
}
