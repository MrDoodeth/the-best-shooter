using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum TypeOfClick
    {
        Click,
        Hold
    }

    [SerializeField] private TypeOfClick typeOfClick;
    public static event Action OnShootInput;
    public static event Action OnShootInputDown;

    [SerializeField] private KeyCode reloadKey;
    public static event Action OnReloadInput;

    public static event Action<bool> OnZoomInput;
    public static Action OnGunTaken;
    public static Action OnGunChanged;
    public static Action OnGunDroped;

    public static bool isControllerAvailable;
    private bool isZoom;

    private void Update()
    {
        if (!isControllerAvailable)
            return;

        if (Input.GetMouseButton(0))
            OnShootInput?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnShootInputDown?.Invoke();

        switch (typeOfClick)
        {
            case TypeOfClick.Click:
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        isZoom = !isZoom;
                        OnZoomInput?.Invoke(isZoom);
                    }
                }
                break;
            case TypeOfClick.Hold:
                {
                    if (Input.GetMouseButton(1))
                        OnZoomInput?.Invoke(true);
                    else
                        OnZoomInput?.Invoke(false);
                }
                break;
        }

        if (Input.GetKeyDown(reloadKey))
            OnReloadInput?.Invoke();
    }
}
