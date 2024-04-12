using UnityEngine;
public class WeaponRunPosition : MonoBehaviour
{
    [SerializeField] private WeaponsSwitcher weaponSwitching;
    [SerializeField] private FirstPersonController fpc;
    public Transform runTransform;
    public Vector3 defaultPosition;
    public float speed;

    private void Awake()
    {
        weaponSwitching = GetComponent<WeaponsSwitcher>();
    }
    //void Update()
    //{
    //    if (fpc.isWalking)
    //    {
    //        if (fpc.isSprinting)
    //        {
    //            SprintState();
    //        }
    //        else
    //        {
    //            DefaultState();
    //        }
    //    }
    //    else
    //    {
    //        DefaultState();
    //    }
    //}

    //private void SprintState()
    //{
    //    //if man running - STOP reloading
    //    if (weaponSwitching.currentGun.reloading)
    //        return;

    //    //disable ZOOM
    //    if (weaponSwitching.currentGun != null)
    //        weaponSwitching.currentGun.weaponZoom.enabled = false;

    //    PlayerController.isControllerAvailable = false;
    //    transform.localPosition = Vector3.Lerp(transform.localPosition, runTransform.localPosition, speed * Time.deltaTime);
    //    transform.localRotation = Quaternion.Lerp(transform.localRotation, runTransform.localRotation, speed * Time.deltaTime);
    //}

    //private void DefaultState()
    //{
    //    //enable ZOOM
    //    if (weaponSwitching.currentGun != null)
    //        weaponSwitching.currentGun.weaponZoom.enabled = true;

    //    PlayerController.isControllerAvailable = true;
    //    transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, speed * Time.deltaTime);
    //    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, speed * Time.deltaTime);
    //}
}