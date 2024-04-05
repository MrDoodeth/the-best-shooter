using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

public class WeaponsSwitching : MonoBehaviour
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
    [SerializeField] private Transform gunPivot;
    [SerializeField][Range(1f, 5f)] private int inventorySize;
    [SerializeField] private float delayBeforeSwitching = 0.5f;
    private float timeSinceLastSwitch;
    private int currentInventorySize;
    [Space(10)]

    [SerializeField] private Animator animator;
    [SerializeField] private FirstPersonController FPC;

    [SerializeField] private List<Gun> weapons;
    public Gun currentGun;
    private int selectedWeapon;

    private void Awake()
    {
        currentInventorySize = inventorySize;
    }

    private void HandMovements(Transform rightPos, Transform leftPos)
    {
        rightHand.SetParent(rightPos);
        rightHand.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        leftHand.SetParent(leftPos);
        leftHand.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (rightPos == joint || leftPos == joint)
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
        if (currentGun != null && currentGun.reloading)
            return;

        var _hit = PlayerRayCaster.HIT;
        if (PlayerRayCaster.HIT.transform != null && _hit.transform.TryGetComponent(out Gun gun))
        {
            if (CanPickUp(gun.Size, _hit.transform))
            {
                currentGun = gun;
                PickUpWeapon(gun);
            }
        }

        void PickUpWeapon(Gun wp)
        {
            weapons.Add(wp);

            wp.transform.SetPositionAndRotation(gunPivot.position, gunPivot.rotation);
            currentGun.weaponZoom.gunPivot = gunPivot;
            wp.transform.SetParent(transform);

            wp.weaponRigidbody.isKinematic = true;
            wp.weaponCollider.enabled = false;

            currentGun.enabled = true;
            currentGun.SetFPC(FPC);

            PlayerController.OnGunTaken?.Invoke();
            currentInventorySize -= currentGun.Size;

            ChangeSelectedWeapon("Up");
        }

        bool CanPickUp(int gunInvSize, Transform tr) => currentInventorySize >= gunInvSize && tr.CompareTag("Weapone");
    }
    private void Drop()
    {     
        if ((currentGun != null && currentGun.reloading) || weapons.Count == 0)
            return;

        currentGun.SetFPC(null);
        currentInventorySize += currentGun.Size;
        currentGun.enabled = false;

        currentGun.weaponRigidbody.isKinematic = false;
        currentGun.weaponCollider.enabled = true;
        currentGun.weaponRigidbody.AddForce(transform.forward * 4f + transform.up * 2f, ForceMode.Impulse);

        currentGun.transform.SetParent(null);
        weapons.Remove(currentGun);

        ChangeSelectedWeapon("Down");
    }
    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == weaponIndex)
                weapons[i].gameObject.SetActive(true);
            else
                weapons[i].gameObject.SetActive(false);
        }
    }

    #region ChangeWeapon
    /// <summary>
    /// Scrolls the index in a circle
    /// </summary>
    /// <param name="type">Up or Down</param>
    private void ChangeSelectedWeapon(string type)
    {
        if (currentGun != null && currentGun.reloading)
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

    private void ChangeSelectedWeapon(int _selectedWeapon)
    {
        selectedWeapon = _selectedWeapon;

        ChangeSelectedWeapon();
    }
    private void ChangeSelectedWeapon()
    {      

        if (weapons.Count == 0)
        {
            PlayerController.OnGunDroped?.Invoke();
            HandMovements(joint, joint);

            currentGun = null;
        }
        else
        {
            PlayerController.OnGunChanged?.Invoke();
            HandMovements(weapons[selectedWeapon].rightHandle, weapons[selectedWeapon].leftHandle);

            currentGun = weapons[selectedWeapon];
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
