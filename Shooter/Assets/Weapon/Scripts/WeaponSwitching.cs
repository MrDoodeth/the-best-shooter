using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Animations.Rigging;

public class WeaponSwitching : MonoBehaviour
{

    [SerializeField] private Transform joint;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Rig armsRig;
    [SerializeField] private Animator animator;
    [SerializeField] private FirstPersonController FPC;

    [Header("References")]
    [SerializeField] private List<Transform> weapons;
    [Space(10)]

    [Header("Settings")]
    //[SerializeField] private float switchTime;

    //Inventoty
    [SerializeField][Range(1f, 5f)] private int inventorySize;
    [SerializeField] private float pickUpDistance;
    [SerializeField] private int currentInventorySize;

    [SerializeField] private float selectionSpeed = 4f;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    public Gun gun;
    [SerializeField] private Rigidbody gunRb;
    [SerializeField] private BoxCollider col;


    private void Start()
    {
        timeSinceLastSwitch = 0f;
        currentInventorySize = inventorySize;
    }

    private void HandMovements(Transform rightPos, Transform leftPos, Transform parent)
    {
        rightHand.SetParent(parent);
        leftHand.SetParent(parent);

        if (rightPos == null || leftPos == null)
        {
            rightHand.DOLocalMove(Vector3.zero, 0.4f);
            leftHand.DOLocalMove(Vector3.zero, 0.4f);
            rightHand.localRotation = Quaternion.identity;
            leftHand.localRotation = Quaternion.identity;
        }
        else
        {
            rightHand.DOLocalMove(rightPos.localPosition, 0.4f);
            leftHand.DOLocalMove(leftPos.localPosition, 0.4f);
            rightHand.localRotation = rightPos.localRotation;
            leftHand.localRotation = leftPos.localRotation;
        }


        
    }
    private bool CanPickUp(int gunInvSize) => currentInventorySize >= gunInvSize;
    private void PickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpDistance))
        {
            if (hit.transform.tag == "Weapone" && CanPickUp(hit.transform.GetComponent<Gun>().Size))
            {
                GiveWeapon(hit.transform);
            }
        }
    }


    #region Future
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Weapone" && CanPickUp())
    //    {
    //        Collider previousCollider = other;
    //        if (previousCollider != other)
    //        {
    //            GiveWeapon(other.transform);
    //        }
    //    }
    //}
    #endregion

    private void Drop()
    {
        if (weapons.Count != 0)
        {
            gunRb.isKinematic = false;
            gun.enabled = false;
            col.enabled = true;
            gunRb.AddForce(transform.forward * 4f + transform.up * 2f, ForceMode.Impulse);  
            weapons[selectedWeapon].SetParent(null);

            gun.SetFPC(null);
            currentInventorySize += gun.Size;
            weapons.Remove(weapons[selectedWeapon]);

            gun = null;
            gunRb = null;
            col = null;

            ChangeSelectedWeapon("Down");
        }     
    }



    public void DeleteWeapon(int _selectedWeapon)
    {
        if (weapons.Count != 0)
        {           
            gun = null;
            gunRb = null;
            col = null;

            weapons.Remove(weapons[_selectedWeapon]);
            currentInventorySize += gun.Size;
            Destroy(weapons[_selectedWeapon].gameObject);
            ChangeSelectedWeapon("Down");
        }
    }
    public void GetWeapon(GameObject _weapon)
    {
        GameObject weapon = Instantiate(_weapon, _weapon.GetComponent<Gun>().weaponZoom.defaultGunPosition, Quaternion.identity);

        if (weapon.transform.tag == "Weapone" && CanPickUp(weapon.transform.GetComponent<Gun>().Size))
        {
            GiveWeapon(weapon.transform);
        }
    }

    private void GiveWeapon(Transform wp)
    {
        weapons.Add(wp);

        gun = wp.GetComponent<Gun>();
        gunRb = wp.GetComponent<Rigidbody>();
        col = wp.GetComponent<BoxCollider>();

        wp.position = Vector3.Lerp(wp.position, gun.weaponZoom.defaultGunPosition, selectionSpeed * Time.deltaTime);
        PlayerController.OnTakenGun?.Invoke();
        wp.SetParent(transform);
        gunRb.isKinematic = true;
        col.enabled = false;
        gun.enabled = true;
        gun.SetFPC(FPC);
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

        timeSinceLastSwitch = 0f;

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

        if (weapons.Count == 0)
        {
            gun = null;
            gunRb = null;
            col = null;

            PlayerController.OnDropGun?.Invoke();
            HandMovements(null, null, joint);
            armsRig.weight = 0f;
            animator.SetLayerWeight(1, 0f);
        }
        else
        {
            gun = weapons[selectedWeapon].GetComponent<Gun>();
            gunRb = weapons[selectedWeapon].GetComponent<Rigidbody>();
            col = weapons[selectedWeapon].GetComponent<BoxCollider>();

            PlayerController.OnChangedGun?.Invoke();
            HandMovements(gun.rightHandle, gun.leftHandle, gun.transform);
            armsRig.weight = 1f;
            animator.SetLayerWeight(1, 1f);
        }
    }

    private void ChangeSelectedWeapon()
    {
        if (weapons.Count == 0)
        {
            gun = null;
            gunRb = null;
            col = null;

            PlayerController.OnDropGun?.Invoke();
            HandMovements(null, null, joint);
            armsRig.weight = 0f;
            animator.SetLayerWeight(1, 0f);
        }
        else
        {
            gun = weapons[selectedWeapon].GetComponent<Gun>();
            gunRb = weapons[selectedWeapon].GetComponent<Rigidbody>();
            col = weapons[selectedWeapon].GetComponent<BoxCollider>();

            PlayerController.OnChangedGun?.Invoke();
            HandMovements(gun.rightHandle, gun.leftHandle, gun.transform);
            armsRig.weight = 1f;
            animator.SetLayerWeight(1, 1f);
        }
    }
    #endregion
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        InventoryState();
    }
    private void InventoryState()
    {
        int previousSelectedWeapon = selectedWeapon + 1;


        if (Input.GetAxis("Mouse ScrollWheel") > 0)  //timeSinceLastSwitch >= switchTime
            ChangeSelectedWeapon("Up");

        if (Input.GetAxis("Mouse ScrollWheel") < 0)  //timeSinceLastSwitch >= switchTime
            ChangeSelectedWeapon("Up");

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count >= 1)
        {
            ChangeSelectedWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2)
        {
            ChangeSelectedWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }

        if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);


        timeSinceLastSwitch += Time.deltaTime;
    }
}
