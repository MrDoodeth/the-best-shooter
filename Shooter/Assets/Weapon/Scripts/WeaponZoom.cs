using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class WeaponZoom : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField][Range(0.01f, 20f)] private float ergonomics;
    [SerializeField][Range(0.01f, 1f)] private float zoomSize = 1f;
    public Vector3 zoomPosition;
    public Vector3 defaultGunPosition;

    private bool isZoom = false;
    [HideInInspector] public FirstPersonController fpc;

    private void OnEnable()
    {
        PlayerController.OnZoomInput += Zoom;
    }

    private void OnDisable()
    {
        PlayerController.OnZoomInput -= Zoom;
        transform.localPosition = defaultGunPosition;
        Zoom(false);
    }
    private void Zoom(bool isPressed)
    {
        if (enabled == true)
            fpc.zoomFOV = fpc.fov * zoomSize;

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
            transform.localPosition = Vector3.Slerp(transform.localPosition, defaultGunPosition, ergonomics * Time.deltaTime);
        }
    }
}
