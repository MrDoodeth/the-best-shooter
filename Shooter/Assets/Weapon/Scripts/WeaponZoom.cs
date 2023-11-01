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
    public FirstPersonController fpc;

    private void OnEnable()
    {
        switch (PlayerController.TypeOfClick)
        {
            case TypeOfClick.Click:
                PlayerController.OnZoomInputDown += Zoom;
                break;
            case TypeOfClick.Hold:
                PlayerController.OnZoomInput += Zoom;
                break;
        }
    }

    private void OnDisable()
    {
        switch (PlayerController.TypeOfClick)
        {
            case TypeOfClick.Click:
                PlayerController.OnZoomInputDown -= Zoom;
                break;
            case TypeOfClick.Hold:
                PlayerController.OnZoomInput -= Zoom;
                break;
        }

        Zoom(false);
        transform.localPosition = defaultGunPosition;
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, zoomPosition, ergonomics * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultGunPosition, ergonomics * Time.deltaTime);
        }
    }
}
