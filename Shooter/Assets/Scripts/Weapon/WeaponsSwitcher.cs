using System.Collections.Generic;
using Shooter.Player;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace Shooter.Weapon
{
    public class WeaponsSwitcher : MonoBehaviour
    {
        [Header("Arms")]
        [SerializeField] private Transform joint;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Rig armsRig;
        [Space(10)]

        [Header("Body")]
        [SerializeField] private MultiAimConstraint bodyAim;
        [SerializeField] private Vector3 takeBodyAimPosition;
        [Space(10)]

        [Header("Settings")]
        [SerializeField] private Transform weaponPivot;
        [SerializeField][Range(1f, 5f)] private int inventorySize;
        [SerializeField] private float delayBeforeSwitching = 0.5f;
        private float timeSinceLastSwitch;
        private int currentInventorySize;
        [Space(10)]

        [SerializeField] private Animator animator;
        [SerializeField] private FirstPersonController fpc;

        [SerializeField] private List<Weapon> weapons;
        public static Weapon currentWeapon { get; private set; }
        private int selectedWeapon;

        private void Awake()
        {
            currentInventorySize = inventorySize;
        }

        private void HandsMovement(Transform rightPosition, Transform leftPosition)
        {
            rightHand.SetParent(rightPosition);
            rightHand.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            leftHand.SetParent(leftPosition);
            leftHand.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            if (rightPosition == joint || leftPosition == joint)
            {
                armsRig.weight = 0f;
                bodyAim.data.offset = Vector3.zero;
                animator.SetLayerWeight(1, 0f);
            }
            else
            {
                armsRig.weight = 1f;
                bodyAim.data.offset = takeBodyAimPosition;
                animator.SetLayerWeight(1, 1f);
            }       
        }
        private void PickUp()
        {
            var hit = PlayerRayCaster.HIT;
            if (PlayerRayCaster.HIT.transform is not null && hit.transform.TryGetComponent(out Weapon weapon))
            {
                if (CanPickUp(weapon.Size, hit.transform))
                {
                    currentWeapon = weapon;
                    weapons.Add(currentWeapon);
                    weapon.OnPickedUp(weaponPivot, this.transform, fpc);
                    currentInventorySize -= currentWeapon.Size;
                    ChangeSelectedWeapon("Up");
                }
            }

            return;

            bool CanPickUp(int gunInventorySize, Transform tr) => currentInventorySize >= gunInventorySize && tr.CompareTag("Weapon");
        }
        private void Drop()
        {     
            if (currentWeapon is null) return;

            weapons.Remove(currentWeapon);
            currentWeapon.Droped();
            currentInventorySize += currentWeapon.Size;
            ChangeSelectedWeapon("Down");
        }
        private void Select(int weaponIndex)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].gameObject.SetActive(i == weaponIndex);
            }
        }

        #region ChangeWeapon
        /// <summary>
        /// Scrolls the index in a circle
        /// </summary>
        /// <param name="type">Up or Down</param>
        private void ChangeSelectedWeapon(string type)
        {
            if (currentWeapon is null)
                return;

            if (type == "Up")
            {
                if (selectedWeapon >= weapons.Count - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
        
            if (type == "Down")
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = weapons.Count - 1;
                else
                    selectedWeapon--;
            }

            ChangeSelectedWeapon();
        }

        private void ChangeSelectedWeapon(int selectedWeapon)
        {
            this.selectedWeapon = selectedWeapon;

            ChangeSelectedWeapon();
        }
        private void ChangeSelectedWeapon()
        {      

            if (weapons.Count == 0)
            {
                //Важен именно такой порядок
                currentWeapon = null;
                PlayerController.OnWeaponDroped?.Invoke();

                HandsMovement(joint, joint);

            }
            else
            {
                //Важен именно такой порядок
                currentWeapon = weapons[selectedWeapon];
                PlayerController.OnWeaponChanged?.Invoke();

                HandsMovement(weapons[selectedWeapon].rightHandle, weapons[selectedWeapon].leftHandle);

            }

            timeSinceLastSwitch = 0f;
        }
        #endregion

        private void Update()
        {
            UpdateInventoryState();
        }
        private void UpdateInventoryState()
        {
            if (!PlayerController.isControllerAvailable)
                return;

            int previousSelectedWeapon = selectedWeapon + 1;

            timeSinceLastSwitch += Time.deltaTime;


            if (Input.GetAxis("Mouse ScrollWheel") > 0 && timeSinceLastSwitch >= delayBeforeSwitching)  
                ChangeSelectedWeapon("Up");

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && timeSinceLastSwitch >= delayBeforeSwitching)  
                ChangeSelectedWeapon("Down");

            if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count >= 1 && timeSinceLastSwitch >= delayBeforeSwitching)
            {
                ChangeSelectedWeapon(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2 && timeSinceLastSwitch >= delayBeforeSwitching)
            {
                ChangeSelectedWeapon(1);
            }

            if (Input.GetKeyDown(KeyCode.E) && timeSinceLastSwitch >= delayBeforeSwitching)
            {
                PickUp();
            }

            if (Input.GetKeyDown(KeyCode.G) && timeSinceLastSwitch >= delayBeforeSwitching)
            {
                Drop();
            }

            if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);
        }
    }
}
