using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField][Range(0.01f, 20f)] private float ergonomics;
    [SerializeField][Range(0.01f, 1f)] private float zoomSize = 1f;
    public Transform weaponPivot;
    public Vector3 zoomPosition;
    public Vector3 defaultWeaponPosition;
    private bool isZoom = false;
    [HideInInspector] public FirstPersonController FPC;

    private void OnEnable()
    {
        PlayerController.OnZoomInput += Zoom;
    }

    private void OnDisable()
    {
        PlayerController.OnZoomInput -= Zoom;    
        if (weaponPivot != null)
            transform.localPosition = weaponPivot.localPosition;
        Zoom(false);
    }
    private void Zoom(bool isPressed)
    {
        if (enabled == true)
            FPC.zoomFOV = FPC.fov * zoomSize;

        isZoom = isPressed;
    }

    private void Update()
    {
        if (isZoom)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, zoomPosition, ergonomics * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, defaultWeaponPosition, ergonomics * Time.deltaTime);
        }
    }
}
