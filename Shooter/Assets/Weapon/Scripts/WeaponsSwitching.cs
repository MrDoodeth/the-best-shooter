using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using Unity.Burst.CompilerServices;

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

    [SerializeField] private List<Transform> weapons;
    private int selectedWeapon;

    [HideInInspector] public Gun gun { get; private set; }
    private Rigidbody gunRb;
    private BoxCollider col;

    private void Start()
    {
        currentInventorySize = inventorySize;
    }

    private void HandMovements(Transform rightPos, Transform leftPos)
    {
        rightHand.SetParent(rightPos);
        //rightHand.DOLocalMove(Vector3.zero, 0.4f);  //smooth movement
        rightHand.localPosition = Vector3.zero;
        rightHand.localRotation = Quaternion.identity;

        leftHand.SetParent(leftPos);
        //leftHand.DOLocalMove(Vector3.zero, 0.4f);   //smooth movement
        leftHand.localPosition = Vector3.zero;
        leftHand.localRotation = Quaternion.identity;

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
    private bool CanPickUp(int gunInvSize) => currentInventorySize >= gunInvSize;
    private void PickUp()
    {
        if (gun != null && gun.reloading)
            return;

        var _hit = PlayerRayCaster.HIT;

        if (_hit.transform.tag == "Weapone" && CanPickUp(_hit.transform.GetComponent<Gun>().Size))
        {
            PickUpWeapon(_hit.transform);
        }
    }
    private void Drop()
    {
        if (weapons.Count != 0 && !gun.reloading)
        {
            gun.SetFPC(null);
            currentInventorySize += gun.Size;
            gun.enabled = false;

            gunRb.isKinematic = false;
            col.enabled = true;
            gunRb.AddForce(transform.forward * 4f + transform.up * 2f, ForceMode.Impulse);  

            weapons[selectedWeapon].SetParent(null);
            weapons.Remove(weapons[selectedWeapon]);

            gun = null;
            gunRb = null;
            col = null;

            ChangeSelectedWeapon("Down");
        }     
    }
    private void PickUpWeapon(Transform wp)
    {

        gun = wp.GetComponent<Gun>();
        gunRb = wp.GetComponent<Rigidbody>();
        col = wp.GetComponent<BoxCollider>();

        weapons.Add(wp);
        
        wp.position = gunPivot.position;
        wp.rotation = gunPivot.rotation;
        gun.weaponZoom.gunPivot = gunPivot;
        wp.SetParent(transform);

        gunRb.isKinematic = true;
        col.enabled = false;

        gun.enabled = true;
        gun.SetFPC(FPC);

        PlayerController.OnTakenGun?.Invoke();
        currentInventorySize -= gun.Size;

        ChangeSelectedWeapon("Up");
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
        OnWeaponSelected();
    }
    private void OnWeaponSelected()
    {

    }

    #region ChangeWeapon
    /// <summary>
    /// Scrolls the index in a circle
    /// </summary>
    /// <param name="type">Up or Down</param>
    private void ChangeSelectedWeapon(string type)
    {
        if (gun != null && gun.reloading)
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
            gun = null;
            gunRb = null;
            col = null;

            PlayerController.OnDropGun?.Invoke();
            HandMovements(joint, joint);
        }
        else
        {
            gun = weapons[selectedWeapon].GetComponent<Gun>();
            gunRb = weapons[selectedWeapon].GetComponent<Rigidbody>();
            col = weapons[selectedWeapon].GetComponent<BoxCollider>();

            PlayerController.OnChangedGun?.Invoke();
            HandMovements(gun.rightHandle, gun.leftHandle);
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

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count >= 1 && !gun.reloading && timeSinceLastSwitch >= delayBeforeSwitching)
        {
            ChangeSelectedWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2 && !gun.reloading && timeSinceLastSwitch >= delayBeforeSwitching)
        {
            ChangeSelectedWeapon(1);
        }

        if (Input.GetKey(KeyCode.E) && timeSinceLastSwitch >= delayBeforeSwitching)
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
